using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KS3;
using System.IO;
using KS3.Model;
using System.Collections.Generic;
using KS3.Http;
using System.Text;
using System.Xml.Linq;

namespace UnitTestKs3
{
    [TestClass]
    public class UnitTestKs3
    {
        static String accessKey = "your access key";
        static String secretKey = "your secret key";

        // KS3 Operation class 
        static KS3Client ks3Client = null;

        static String bucketName = "test1-zzy-jr";
        static String objKeyNameMemoryData = "short-content";
        static String objKeyNameFileData = "abc.txt";

        static String inFilePath = "E:\\tool\\abc.rar";
        static String outFilePath = "D:/test.out.data";

        private static bool init() {
            if (accessKey.Equals("YOUR ACCESS KEY") || secretKey.Equals("YOUR SECRET KEY"))
            {
                Console.WriteLine("You should be set your Access Key and Secret Key");
                return false;
            }
            ks3Client = new KS3Client(accessKey, secretKey);

            FileInfo fi = new FileInfo(inFilePath);
            if (!fi.Exists)
            {
                FileStream fs = null;
                try
                {
                    fs = fi.OpenWrite();
                    for (int i = 0; i < 1024 * 1024; i++)
                        fs.WriteByte((byte)i);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine("Init Data File Fail");
                    Console.WriteLine(e.ToString());
                    return false;
                }
                finally
                {
                    fs.Close();
                }
            }

            //bucketName = "test-ks3-bucket-" + new Random().Next();
            return true;
        }
        [TestMethod]
        public void TestHeadBucket()
        {
            if (init()) {
                HeadBucketResult bucket = ks3Client.headBucket(bucketName);
                Assert.IsNotNull(bucket);
            }
        }
        [TestMethod]
        public void TestGetBucketCorsConfig() {
            if (init()) {
                BucketCorsConfigurationResult bucketcors = ks3Client.getBucketCors(bucketName);
                Assert.IsNotNull(bucketcors);
            }
        }
        [TestMethod]
        public void TestGetBucketLocation()
        {
            if (init())
            {
                GetBucketLocationResult bucketLocation = ks3Client.getBucketLocation(bucketName);
                Assert.IsNotNull(bucketLocation);
            }
        }
        [TestMethod]
        public void TestGetBucketLogging()
        {
            if (init())
            {
                GetBucketLoggingResult bucketlogging = ks3Client.getBucketLogging(bucketName);
                Assert.IsNotNull(bucketlogging);
            }
        }
        [TestMethod]
        public void TestSetBucketCors()
        {
            if (init())
            {
                PutBucketCorsRequest putBucketCorsRequest = new PutBucketCorsRequest();
                putBucketCorsRequest.BucketName = bucketName;
                BucketCorsConfigurationResult bucketCorsConfiguration = new BucketCorsConfigurationResult();
                IList<CorsRule> corsRule = new List<CorsRule>();
                CorsRule rule = new CorsRule();
                rule.AllowedMethods.Add(HttpMethod.PUT);
                rule.AllowedHeaders.Add("*");
                rule.AllowedOrigins.Add("http://www.example.com");
                rule.AllowedOrigins.Add("http://www.example.a.com");
                rule.ExposedHeaders.Add("*");
                rule.MaxAgeSeconds = 200;
                corsRule.Add(rule);
                bucketCorsConfiguration.Rules = corsRule;
                putBucketCorsRequest.BucketCorsConfiguration = bucketCorsConfiguration;

                ks3Client.setBucketCors(putBucketCorsRequest);
            }
        }
        [TestMethod]
        public void TestSetBucketLogging()
        {
            if (init())
            {
                PutBucketLoggingRequest putBucketLoggingRequest = new PutBucketLoggingRequest();
                putBucketLoggingRequest.BucketName = bucketName;
                GetBucketLoggingResult bucketLogging = new GetBucketLoggingResult();
                //bucketLogging.Enable = true;
                //bucketLogging.TargetBucket = "test1-zzy-jr";
                //bucketLogging.TargetPrefix = "test";

                /*BucketLogging.Enable 为false 则为关闭日志设置*/
                putBucketLoggingRequest.BucketLogging = bucketLogging;

                ks3Client.setBucketLogging(putBucketLoggingRequest);
            }
        }
        [TestMethod]
        public void TestDeleteBucketCors()
        {
            if (init())
            {
                DeleteBucketCorsRequest deleteBucketCorsRequest = new DeleteBucketCorsRequest();
                deleteBucketCorsRequest.BucketName = bucketName;

                ks3Client.deleteBucketCors(deleteBucketCorsRequest);
            }
        }
        [TestMethod]
        public void TestDeleteMultiObjects()
        {
            if (init())
            {
                DeleteMultipleObjectsRequest deleteMultipleObjectsRequest = new DeleteMultipleObjectsRequest();
                deleteMultipleObjectsRequest.BucketName = bucketName;
                deleteMultipleObjectsRequest.ObjectKeys = new String[] { "过滤条件.txt" };
                DeleteMultipleObjectsResult result = ks3Client.deleteMultiObjects(deleteMultipleObjectsRequest);
            }
        }
        [TestMethod]
        public void TestCreateBucket()
        {
            if (init())
            {
                Bucket bucket = ks3Client.createBucket(bucketName);
                Assert.IsNotNull(bucket);
            }
        }
        [TestMethod]
        public void TestListBuckets()
        {
            if (init())
            {
                IList<Bucket> bucketsList = ks3Client.listBuckets();
                foreach (Bucket b in bucketsList)
                {
                    Console.WriteLine(b.ToString());
                }
            }
        }
        [TestMethod]
        public void TestGetBucketACL()
        {
            if (init())
            {
                AccessControlList acl = ks3Client.getBucketAcl(bucketName);
                Console.WriteLine("Bucket Name: " + bucketName);
                Console.WriteLine(acl.ToString());
                Assert.IsNotNull(acl);
            }
        }
        [TestMethod]
        public void TestSetBucketACL()
        {
            if (init())
            {
                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                ks3Client.setBucketAcl(bucketName, cannedAcl);
            }
        }
        [TestMethod]
        public void TestPutObject()
        {
            if (init())
            {
                Stream streamNull = new MemoryStream();
                PutObjectResult createFolder = ks3Client.putObject("test2-zzy-jr", "jrtest", streamNull, null);

                // Put Object(upload a short content)

                String sampleContent = "This is a sample content.(25 characters before, included the 4 spaces)";
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleContent));
                PutObjectResult shortContentResult = ks3Client.putObject("test2-zzy-jr", "jrtest/aa", stream, null);

                //FileInfo file = new FileInfo("E:\\tool\\eclipse.rar");
                //PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, objKeyNameFileData, file);
                //PutObjectResult putObjectResult = ks3Client.putObject(putObjectRequest);

                Assert.IsNotNull(shortContentResult.getETag());
            }
        }
        [TestMethod]
        public void TestCopyObject()
        {
            if (init())
            {
                CopyObjectRequest copyObjectRequest = new CopyObjectRequest();
                copyObjectRequest.SourceObject = objKeyNameFileData;
                copyObjectRequest.SourceBucket = bucketName;
                copyObjectRequest.DestinationBucket = "test2-zzy-jr";
                copyObjectRequest.DestinationObject = objKeyNameFileData;
                //CannedAccessControlList cannedAcl=new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                //copyObjectRequest.CannedAcl = cannedAcl;

                CopyObjectResult result = ks3Client.copyObject(copyObjectRequest);
                Assert.IsNotNull(result);
            }
        }
        [TestMethod]
        public void TestHeadObject()
        {
            if (init())
            {
                HeadObjectRequest headObjectRequest = new HeadObjectRequest();
                headObjectRequest.BucketName = bucketName;
                headObjectRequest.ObjectKey = objKeyNameFileData;
                HeadObjectResult result = ks3Client.headObject(headObjectRequest);
            }
        }
        [TestMethod]
        public InitiateMultipartUploadResult TestMultipartInit()
        {
            if (init())
            {
                InitiateMultipartUploadResult re = ks3Client.initiateMultipartUpload(bucketName, objKeyNameFileData);
                Assert.IsNotNull(re);
                return re;
            }
            return null;
        }
        [TestMethod]
        public void TestgetMultiUploadPart()
        {
            if (init())
            {
                string path = @"E:\tool\eclipse.rar";//上传文件路径,例如E:\tool\aa.rar
                InitiateMultipartUploadResult result = TestMultipartInit();
                FileInfo file = new FileInfo(path);
                int part = 5 * 1024 * 1024;
                int numBytesToRead = (int)file.Length;
                int i = 0;
                XElement root = new XElement("CompleteMultipartUpload");//初始化一个xml，以备分块上传完成后调用complete方法提交本次上传的文件以通知服务端合并分块
                //开始读取文件
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    while (numBytesToRead > 0)
                    {
                        UploadPartRequest request = new UploadPartRequest(
                                result.getBucket(), result.getKey(), result.getUploadId(),
                                i + 1);
                        //每次读取5M文件内容，如果最后一次内容不及5M则按实际大小取值
                        int count = Convert.ToInt32((i * part + part) > file.Length ? file.Length - i * part : part);
                        byte[] data = new byte[count];
                        int n = fs.Read(data, 0, count);
                        request.setInputStream(new MemoryStream(data));
                        
                        PartETag tag = ks3Client.uploadPart(request);//上传本次分块内容
                        Console.WriteLine(tag.ToString());
                        if (n == 0)
                            break;
                        numBytesToRead -= n;

                        XElement partE = new XElement("Part");
                        partE.Add(new XElement("PartNumber", i + 1));
                        partE.Add(new XElement("ETag", tag.geteTag()));
                        root.Add(partE);
                        i++;
                    }
                }
                //所有分块上传完成后发起complete request，通知服务端合并分块
                CompleteMultipartUploadRequest completeRequest = new CompleteMultipartUploadRequest(result.getBucket(), result.getKey(), result.getUploadId());
                completeRequest.setContent(new MemoryStream(System.Text.Encoding.Default.GetBytes(root.ToString())));
                CompleteMultipartUploadResult completeResult = ks3Client.completeMultipartUpload(completeRequest);
            }
        }
        [TestMethod]
        public void TestlistMultipartUploads() { 
            if(!init()){
                ListMultipartUploadsRequest request = new ListMultipartUploadsRequest(bucketName, objKeyNameFileData, "uploadid");
                ListMultipartUploadsResult result = ks3Client.getListMultipartUploads(request);
                Assert.IsNotNull(result);
            }
        }
        [TestMethod]
        public void TestAbortMultipartUpload() { 
            if(!init()){
                AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucketName, objKeyNameFileData, "uploadid");
                ks3Client.AbortMultipartUpload(request);
            }
        }
        [TestMethod]
        public void TestListObjects() { 
            if(!init()){
                ObjectListing objects = ks3Client.listObjects(bucketName);
                //ListObjectsRequest request = new ListObjectsRequest();
                //request.setBucketName(bucketName);
                //request.setMarker("PersistenceServiceImpl.java");
                //ObjectListing objects = ks3Client.listObjects(request);
                //Assert.IsNotNull(objects);
                
				Console.WriteLine(objects.ToString());
				Console.WriteLine("---------------------\n");

				// Get Object Metadata
				Console.WriteLine("--- Get Object Metadata ---");

				ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameMemoryData); 
				Console.WriteLine(objMeta.ToString());
                Console.WriteLine();
				objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData);
                Assert.IsNotNull(objMeta);
            }
        }
        [TestMethod]
        public void TestGetObjectACL() { 
            if(!init()){
                AccessControlList acl = ks3Client.getObjectAcl(bucketName, objKeyNameMemoryData);
                Assert.IsNotNull(acl);
            }
        }
        [TestMethod]
        public void TestSetObjectACL() { 
            if(!init()){
                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                ks3Client.setObjectAcl(bucketName, objKeyNameMemoryData, cannedAcl);
            }
        }
        [TestMethod]
        public void TestGetObject()
        { 
            if(!init()){
                try
                {
                    // Get Object(download and store in memory)
                    Console.WriteLine("--- Download and Store in Memory ---");

                    GetObjectRequest getShortContent = new GetObjectRequest(bucketName, objKeyNameMemoryData);
                    getShortContent.setRange(0, 24);
                    KS3Object ks3Object = ks3Client.getObject(getShortContent);

                    StreamReader sr = new StreamReader(ks3Object.getObjectContent());
                    Console.WriteLine("Content:\n" + sr.ReadToEnd());
                    sr.Close();
                    ks3Object.getObjectContent().Close();

                    Console.WriteLine("------------------------------------\n");
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.ToString());
                }


                try
                {
                    // Get Object(download and save as a file)
                    Console.WriteLine("--- Download a File ---");

                    // I need to get the Content-Length to set the listener.
                    ObjectMetadata objectMetadata = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData);

                    GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, objKeyNameFileData, new FileInfo(outFilePath));
                    KS3Object obj = ks3Client.getObject(getObjectRequest);
                    obj.getObjectContent().Close(); // The file was opened in [KS3ObjectResponseHandler], so I close it first. 

                    Console.WriteLine("Success. See the file downloaded at {0}", outFilePath);
                    Console.WriteLine("-----------------------\n");
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        [TestMethod]
        public void TestDeleteObject() { 
            if(!init()){
                ks3Client.deleteObject(bucketName, objKeyNameFileData);
            }
        }
        [TestMethod]
        public void TestDeleteBucket() { 
            if(!init()){
                ks3Client.deleteBucket(bucketName);
            }
        }
        [TestMethod]
        public void TestGeneratePresignedUrl() { 
            if(!init()){
                 string url=ks3Client.generatePresignedUrl(bucketName, objKeyNameFileData, DateTime.Now.AddMinutes(5));
                 Assert.IsNotNull(url);
            }
        }
        [TestMethod]
        public void TestGetAdpTask() { 
            if(!init()){
                GetAdpRequest getAdpRequest = new GetAdpRequest();
                getAdpRequest.TaskId = "00P99HHVuHlS";
                GetAdpResult result= ks3Client.getAdpTask(getAdpRequest);
                Assert.IsNotNull(result);
            }
        }
        [TestMethod]
        public void TestputAdpTask() { 
            if(!init()){
                PutAdpRequest putAdpRequest = new PutAdpRequest();
                putAdpRequest.BucketName = bucketName;
                putAdpRequest.ObjectKey = objKeyNameFileData;
                IList<Adp> fops = new List<Adp>();
                Adp fop12 = new Adp();
                fop12.Command = "tag=avm3u8&segtime=10&abr=128k&vbr=1000k&&res=1280x720";
                fop12.Key = "野生动物-hls切片.m3u8";
                fops.Add(fop12);
                putAdpRequest.Adps = fops;
                putAdpRequest.NotifyURL = "http://10.4.2.38:19090/";
                String taskid = ks3Client.putAdpTask(putAdpRequest);
                Assert.IsNotNull(taskid);
            }
        }
    }
}
