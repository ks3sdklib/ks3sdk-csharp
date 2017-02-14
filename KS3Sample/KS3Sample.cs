using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using KS3;
using KS3.Auth;
using KS3.Http;
using KS3.Model;
using KS3.Internal;
using KS3.KS3Exception;
using System.Xml.Linq;
using System.Web;

namespace KS3Sample
{
    class KS3Sample
    {
        static String accessKey = "your access key";
        static String secretKey = "your secret key";

		// KS3 Operation class 
		static KS3Client ks3Client = null;

        static String bucketName = "copy.des.ksc.shanghai";
        static String endPoint = "ks3-cn-shanghai.ksyun.com";
        static String objKeyNameMemoryData	= "short-content";
        static String objKeyNameFileData = "100kb/texthtml";

        static String inFilePath = "E:\\tool\\abc.rar";
		static String outFilePath = "D:/test.out.data";
        static String sk = "sk";
        static String ak = "ak";


        static void Main(string[] args)
        {
            initClient();

            //getObjUrl();

            /*
            if (!init())
                return;		// init failed 

            Console.WriteLine("========== Begin ==========\n");
            */

            //headBucket();
            //getBucketCorsConfig();
            //getBucketLocation();
            //getBucketLogging();
            //setBucketCors();
            //setBucketLogging();
            //deleteBucketCors();
            //deleteMultiObjects();
            //copyObject();
            //headObject();
            //putAdpTask();
            //getAdpTask();

            //createBucket();
            //listBuckets();
            //getBucketACL();
            //setBucketACL();
           putObject();
            //listObjects();
            //listObjectsPage();
            //getObjectACL();
            //setObjectACL();
            //getObject();
            //deleteObject();
            //deleteBucket();
            //catchServiceException();
            //multipartUp();
            //uploadPart();
            //listMultipartUploads(bucketName, objKeyNameFileData, "uploadid");
            //AbortMultipartUpload(bucketName, objKeyNameFileData, "uploadid");
            //generatePresignedUrl();
            Console.WriteLine("\n==========  End  ==========");
		}


        private static void initClient() {
            ClientConfiguration config = new ClientConfiguration();
            config.setTimeout(5*1000);
            config.setReadWriteTimeout(5 * 1000);
            config.setMaxConnections(20);

            String accessKey = "ak";
            String secretKey = "sk";

            String bucketName = "YOUR BUCKET NAME";
            String objKeyName = "YOUR OBJECT KEY";

            /**
             * 设置服务地址</br>
             * 中国（北京）| ks3-cn-beijing.ksyun.com
             * 中国（上海）| ks3-cn-shanghai.ksyun.com
             * 中国（香港）| ks3-cn-hk-1.ksyun.com
             */
            String endPoint = "ks3-cn-beijing.ksyun.com";    //此处以北京region为例

            ks3Client = new KS3Client(accessKey, secretKey);
            ks3Client.setEndpoint(endPoint);

        }

        private static void getObjUrl() {
            DateTime date = DateTime.Now;
            date=date.AddMinutes(5);
            String url = ks3Client.generatePresignedUrl("ksc.harry", "14.jpg", date);
            Console.WriteLine("url:"+url);
        }

		private static bool init()
		{
			if ( accessKey.Equals("YOUR ACCESS KEY") || secretKey.Equals("YOUR SECRET KEY") )
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
						fs.WriteByte( (byte)i );
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
        private static bool headBucket() {
            // Head Bucket
            try
            {
                Console.WriteLine("--- Head Bucket: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                HeadBucketResult bucket = ks3Client.headBucket(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("Head Bucket Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        private static bool getBucketCorsConfig() {
            try
            {
                Console.WriteLine("--- getBucketCorsConfig: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                BucketCorsConfigurationResult bucketcors = ks3Client.getBucketCors(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("getBucketCorsConfig Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        private static bool getBucketLocation() {
            try
            {
                Console.WriteLine("--- getBucketLocation: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                GetBucketLocationResult bucketLocation = ks3Client.getBucketLocation(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("getBucketLocation Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        private static bool getBucketLogging() {
            try
            {
                Console.WriteLine("--- getBucketLogging: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                GetBucketLoggingResult bucketlogging = ks3Client.getBucketLogging(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("getBucketLogging Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        public static bool setBucketCors() {
            try
            {
                Console.WriteLine("--- setBucketCors: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
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

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("setBucketCors Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        public static bool setBucketLogging() {
            try
            {
                Console.WriteLine("--- setBucketLogging: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
                PutBucketLoggingRequest putBucketLoggingRequest = new PutBucketLoggingRequest();
                putBucketLoggingRequest.BucketName = bucketName;
                GetBucketLoggingResult bucketLogging = new GetBucketLoggingResult();
                //bucketLogging.Enable = true;
                //bucketLogging.TargetBucket = "test1-zzy-jr";
                //bucketLogging.TargetPrefix = "test";

                /*BucketLogging.Enable 为false 则为关闭日志设置*/
                putBucketLoggingRequest.BucketLogging = bucketLogging;

                ks3Client.setBucketLogging(putBucketLoggingRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("setBucketLogging Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        public static bool deleteBucketCors() {
            try
            {
                Console.WriteLine("--- deleteBucketCors: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
                DeleteBucketCorsRequest deleteBucketCorsRequest = new DeleteBucketCorsRequest();
                deleteBucketCorsRequest.BucketName = bucketName;

                ks3Client.deleteBucketCors(deleteBucketCorsRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("deleteBucketCors Fail! " + e.ToString());
                return false;
            }
            return true;
        }
        private static bool deleteMultiObjects() {
            try
            {
                Console.WriteLine("--- deleteMultiObjects: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
                DeleteMultipleObjectsRequest deleteMultipleObjectsRequest = new DeleteMultipleObjectsRequest();
                deleteMultipleObjectsRequest.BucketName = bucketName;
                deleteMultipleObjectsRequest.ObjectKeys = new String[] { "过滤条件.txt" };
                DeleteMultipleObjectsResult result=ks3Client.deleteMultiObjects(deleteMultipleObjectsRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("deleteMultiObjects Fail! " + e.ToString());
                return false;
            }
            return true;
        }


		private static bool createBucket()
		{
            // Create Bucket
			try
			{
                Console.WriteLine("--- Create Bucket: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

				Bucket bucket = ks3Client.createBucket(bucketName);

				Console.WriteLine("Success.");
				Console.WriteLine("----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine("Create Bucket Fail! " + e.ToString());				
				return false;
			}

			return true;
		}

		private static bool listBuckets()
		{
            // List Buckets
			try
			{
				Console.WriteLine("--- List Buckets: ---");

				IList<Bucket> bucketsList = ks3Client.listBuckets();
				foreach (Bucket b in bucketsList)
				{
					Console.WriteLine(b.ToString());
				}
            
				Console.WriteLine("---------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}
			    
			return true;
		}

		private static bool getBucketACL()
		{
            // Get Bucket ACL
            try 
			{
				Console.WriteLine("--- Get Bucket ACL: ---");
            
			    AccessControlList acl = ks3Client.getBucketAcl(bucketName);
                Console.WriteLine("Bucket Name: " + bucketName);
                Console.WriteLine(acl.ToString());

				Console.WriteLine("-----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}

		private static bool setBucketACL()
		{
            // Set Bucket ACL
			try 
			{
				Console.WriteLine("--- Set Bucket ACL: ---");

				CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
				ks3Client.setBucketAcl(bucketName, cannedAcl);

                Console.WriteLine("Bucket Name: " + bucketName);
				Console.WriteLine("Success, now the ACL is:\n" + ks3Client.getBucketAcl(bucketName));
				Console.WriteLine("-----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}

		private static bool putObject()
		{
			try
            {
                //Console.WriteLine("--- create a folder: ---");
                //Stream streamNull = new MemoryStream();
                //PutObjectResult createFolder = ks3Client.putObject("bucketcors", "jrtest", streamNull, null);
                //Console.WriteLine("---------------------\n");

                //// Put Object(upload a short content)
                //Console.WriteLine("--- Upload a Short Content: ---");
                //String sampleContent = "This is a sample content.(25 characters before, included the 4 spaces)";
                //Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleContent));
                //PutObjectResult shortContentResult = ks3Client.putObject("bucketcors", "jrtest/aa", stream, null);

                //Console.WriteLine("Upload Completed. eTag=" + shortContentResult.getETag() + ", MD5=" + shortContentResult.getContentMD5());
                //Console.WriteLine("-------------------------------\n");

                //Put Object(upload a file)
                Console.WriteLine("--- Upload a File ---");
                bucketName = "kingsoft.test.ml";
                objKeyNameFileData = "testPut2.mp4";
                FileInfo file = new FileInfo("d:/hengping.mp4");
                PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, objKeyNameFileData, file);
                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PRIVATE);
                putObjectRequest.setCannedAcl(cannedAcl);

                SampleListener sampleListener = new SampleListener(file.Length);
                putObjectRequest.setProgressListener(sampleListener);
                PutObjectResult putObjectResult = ks3Client.putObject(putObjectRequest);

                Console.WriteLine("Upload Completed. eTag=" + putObjectResult.getETag() + ", MD5=" + putObjectResult.getContentMD5());
                Console.WriteLine("---------------------\n");
			}
			catch (System.Exception e) 
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}
        public static bool copyObject() {
            try
            {
                Console.WriteLine("--- copyObject: ---");
                CopyObjectRequest copyObjectRequest = new CopyObjectRequest();
                copyObjectRequest.SourceObject = objKeyNameFileData;
                copyObjectRequest.SourceBucket = bucketName;
                copyObjectRequest.DestinationBucket = "test2-zzy-jr";
                copyObjectRequest.DestinationObject = objKeyNameFileData;
                //CannedAccessControlList cannedAcl=new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                //copyObjectRequest.CannedAcl = cannedAcl;

                CopyObjectResult result = ks3Client.copyObject(copyObjectRequest);
                Console.WriteLine("---------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
        public static bool headObject() {
            try
            {
                Console.WriteLine("--- headObject: ---");
                HeadObjectRequest headObjectRequest = new HeadObjectRequest();
                headObjectRequest.BucketName = bucketName;
                headObjectRequest.ObjectKey = objKeyNameFileData;
                HeadObjectResult result = ks3Client.headObject(headObjectRequest);
                long length=result.ObjectMetadata.getContentLength();
                Console.WriteLine("---------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
        /**
         * 初始化分块上传，服务端会返回一个全局唯一的uploadid
         * **/
        private static InitiateMultipartUploadResult multipartUp()
        {
            InitiateMultipartUploadResult re=ks3Client.initiateMultipartUpload(bucketName, objKeyNameFileData);
            Console.WriteLine(re.ToString());
            return re;
        }
        /**
         * 分块上传例子
         * **/
        private static bool uploadPart() {
            string path = @"you file path";//上传文件路径,例如E:\tool\aa.rar
            InitiateMultipartUploadResult result=multipartUp();
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
                    ProgressListener sampleListener = new SampleListener(count);//实例一个更新进度的监听类，实际使用中可自己定义实现
                    request.setProgressListener(sampleListener);
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
            return true;
        }
        /**
         * 获取已经上传的分块列表
         * **/
        private static bool listMultipartUploads(string bucket,string objKey,string uploadId) {
            ListMultipartUploadsRequest request = new ListMultipartUploadsRequest(bucket,objKey,uploadId);
            ListMultipartUploadsResult result =ks3Client.getListMultipartUploads(request);
            return true;
        }
        /**
         * 放弃本次上传
         * **/
        private static bool AbortMultipartUpload(string bucket, string objKey, string uploadId)
        {
            AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, objKey, uploadId);
            ks3Client.AbortMultipartUpload(request);
            return true;
        }
		private static bool listObjects()
		{
			try
			{
				// List Objects
				Console.WriteLine("--- List Objects: ---");

                //ObjectListing objects = ks3Client.listObjects(bucketName);

                KS3Client ks3Client = new KS3Client("ak", "sk");
                ks3Client.setEndpoint("kss.ksyun.com");

                ListObjectsRequest request = new ListObjectsRequest();
                request.setBucketName("haofenshu");
                //request.setMarker("PersistenceServiceImpl.java");
                request.setPrefix("file/s/167206/1082/ClipedRecord.xml");
                request.setDelimiter("/");
                ObjectListing objects = ks3Client.listObjects(request);


                //Console.WriteLine(objects.ToString());
                Console.WriteLine("---------------------\n");

				// Get Object Metadata
				Console.WriteLine("--- Get Object Metadata ---");

                //ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameMemoryData); 
                //Console.WriteLine(objMeta.ToString());
                //Console.WriteLine();
                ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData);
				Console.WriteLine(objMeta.ToString());
				
				Console.WriteLine("---------------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());		
				return false;
			}

			return true;
		}



        private static bool listObjectsPage()
        {
            try
            {

                KS3Client ks3Client = new KS3Client(ak, sk);
                ks3Client.setEndpoint("kss.ksyun.com");

                ListObjectsRequest request = new ListObjectsRequest();
                request.setBucketName("ksc.harry");
                request.setMaxKeys(20);
                request.setDelimiter("/");
                ObjectListing objects = ks3Client.listObjects(request);

                Console.WriteLine(objects);
                Console.WriteLine(objects.isTruncated());
                Console.WriteLine(objects.getNextMarker());

                request.setMarker(objects.getNextMarker());

                objects = ks3Client.listObjects(request);

                Console.WriteLine(objects);
                Console.WriteLine(objects.isTruncated());
                Console.WriteLine(objects.getNextMarker());

                Console.WriteLine("---------------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool getObjectACL()
        {
            // Get Object ACL
            try
            {
                Console.WriteLine("--- Get Object ACL: ---");

                AccessControlList acl = ks3Client.getObjectAcl(bucketName ,objKeyNameMemoryData);
                Console.WriteLine("Object Key: " + objKeyNameMemoryData);
                Console.WriteLine(acl.ToString());

                Console.WriteLine("-----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;

            }

            return true;
        }

        private static bool setObjectACL()
        {
            // Set Object ACL
            try
            {
                Console.WriteLine("--- Set Object ACL: ---");

                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                Console.WriteLine("Object Key: " + objKeyNameMemoryData);
                ks3Client.setObjectAcl(bucketName, objKeyNameMemoryData, cannedAcl);

                Console.WriteLine("Success, now the ACL is:\n" + ks3Client.getObjectAcl(bucketName, objKeyNameMemoryData));
                Console.WriteLine("-----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

		private static bool getObject()
		{
            /*
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
				return false;
			}
            */

            try
            {
                // Get Object(download and save as a file)
				Console.WriteLine("--- Download a File ---");

                // I need to get the Content-Length to set the listener.
                ObjectMetadata objectMetadata = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData); 
				
				SampleListener downloadListener = new SampleListener(objectMetadata.getContentLength());
				GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, objKeyNameFileData, new FileInfo(outFilePath));
				getObjectRequest.setProgressListener(downloadListener);
				KS3Object obj = ks3Client.getObject(getObjectRequest);
				obj.getObjectContent().Close(); // The file was opened in [KS3ObjectResponseHandler], so I close it first. 
				
				Console.WriteLine("Success. See the file downloaded at {0}", outFilePath);
				Console.WriteLine("-----------------------\n");
			}
			catch (System.Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}

			return true;
		}

		private static bool deleteObject()
		{
			// Delete Object
			try
			{
				Console.WriteLine("--- Delete Object: ---");

				ks3Client.deleteObject(bucketName, objKeyNameMemoryData);
				ks3Client.deleteObject(bucketName, objKeyNameFileData);

				Console.WriteLine("Delete Object completed.");
				Console.WriteLine("---------------------\n");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return false;
			}
			
			return true;
		}

		private static bool deleteBucket()
		{
            // Delete Bucket
            try
            {
                Console.WriteLine("--- Delete Bucket: ---");

                ks3Client.deleteBucket(bucketName);
                
                Console.WriteLine("Delete Bucket completed.");
				Console.WriteLine("----------------------\n");
			}
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
				return false;
            }
			return true;
        }

        private static bool catchServiceException()
        {
            // This is a sample of catch ServiceException of KS3.
            // You can catch the ServiceException when some illegal operations appear.
            // But note that, if we have done some illegal operations, there also may appear some other unexcepted exceptions too.
            // Now we will see a ServiceException because will try to delete a bucket that does not exist.
            try
            {
                Console.WriteLine("--- Catch ServiceException: ---");

                ks3Client.deleteBucket(bucketName);

                Console.WriteLine("-------------------------------\n");
            }
            catch (ServiceException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
        private static bool generatePresignedUrl() {
            Console.WriteLine("--- generate Presigned Url: ---");
            string url=ks3Client.generatePresignedUrl(bucketName, objKeyNameFileData, DateTime.Now.AddMinutes(5));
            Console.WriteLine("success generate presigned url:"+url);
            Console.WriteLine("-------------------------------\n");
            return true;
        }
        private static bool getAdpTask() {
            try
            {
                Console.WriteLine("--- getAdpTask begin: ---");
                GetAdpRequest getAdpRequest = new GetAdpRequest();
                getAdpRequest.TaskId = "00P99HHVuHlS";
                GetAdpResult result= ks3Client.getAdpTask(getAdpRequest);

                Console.WriteLine("---------getAdpTask end;--------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
        private static bool putAdpTask() {
            try
            {
                Console.WriteLine("--- putAdpTask begin: ---");
                PutAdpRequest putAdpRequest = new PutAdpRequest();
                putAdpRequest.BucketName = "kingsoft.test.ml";
                putAdpRequest.ObjectKey = "testPut.mp4";
                IList<Adp> fops=new List<Adp>();
                Adp fop12 = new Adp();
                fop12.Command= "tag=avscrnshot&ss=5";
                fop12.Bucket = "kingsoft.test.ml";
                fop12.Key= "testAdp.jpg";
                fops.Add(fop12);
                putAdpRequest.Adps = fops;
                putAdpRequest.NotifyURL = "http://10.4.2.38:19090/";
                String taskid=ks3Client.putAdpTask(putAdpRequest);

                Console.WriteLine("---------putAdpTask end; taskid:" + taskid + "---------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }

    class SampleListener : ProgressListener
    {
        long size = -1;
        long completedSize = 0;
        int rate = 0;
        bool cancled = false;

        public SampleListener() { }

        public SampleListener(long size)
        {
            this.size = size;
        }

        public void progressChanged(ProgressEvent progressEvent)
        {
            int eventCode = progressEvent.getEventCode();

            if (eventCode == ProgressEvent.STARTED)
                Console.WriteLine("Started.");
            else if (eventCode == ProgressEvent.COMPLETED)
                Console.WriteLine("Completed.");
            else if (eventCode == ProgressEvent.FAILED)
                Console.WriteLine("Failed.");
            else if (eventCode == ProgressEvent.CANCELED)
                Console.WriteLine("Cancled.");
            else if (eventCode == ProgressEvent.TRANSFERRED)
            {
                this.completedSize += progressEvent.getBytesTransferred();
                int newRate = (int)((double)completedSize / size * 100 + 0.5);
                if (newRate > this.rate)
                {
                    this.rate = newRate;
                    Console.WriteLine("Processing ... " + this.rate + "%");
                }
            }
        } // end of progressChanged

        public bool askContinue()
        {
            return !this.cancled;
        }
    }

}