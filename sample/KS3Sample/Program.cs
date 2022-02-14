using KS3;
using KS3.Http;
using KS3.KS3Exception;
using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace KS3Sample
{
    public class Program
    {
        static readonly string accessKey = "your access key";
        static readonly string secretKey = "your secret key";

        // KS3 Operation class 
        static IKS3 ks3Client = null;

        static string bucketName = "copy.des.ksc.shanghai";
        //static readonly string endPoint = "ks3-cn-shanghai.ksyun.com";
        static readonly string objKeyNameMemoryData = "short-content";
        static string objKeyNameFileData = "100kb/texthtml";

        static readonly string inFilePath = "E:\\tool\\abc.rar";
        static readonly string outFilePath = "D:/test.out.data";
        static readonly string sk = "sk";
        static readonly string ak = "ak";

        static void Main(string[] args)
        {
            InitClient();

            //getObjUrl();

            /*
            if (!init())
                return;		// init failed 

            Console.WriteLine("========== Begin ==========\n");
            */

            HeadBucket();
            GetBucketCorsConfig();
            GetBucketLocation();
            GetBucketLogging();
            SetBucketCors();
            SetBucketLogging();
            DeleteBucketCors();
            DeleteMultiObjects();
            CopyObject();
            HeadObject();
            PutAdpTask();
            GetAdpTask();

            CreateBucket();
            ListBuckets();
            GetBucketACL();
            SetBucketACL();
            PutObject();
            ListObjects();
            ListObjectsPage();
            GetObjectACL();
            SetObjectACL();
            GetObject();
            DeleteObject();
            DeleteBucket();
            CatchServiceException();
            MultipartUp();
            UploadPart();
            ListMultipartUploads(bucketName, objKeyNameFileData, "uploadid");
            AbortMultipartUpload(bucketName, objKeyNameFileData, "uploadid");
            GeneratePresignedUrl();
            Console.WriteLine("\n==========  End  ==========");
        }

        private static void InitClient()
        {
            //var config = new ClientConfiguration()
            //{
            //    Timeout = 5 * 1000,
            //    ReadWriteTimeout = 5 * 1000,
            //    MaxConnections = 20
            //};

            var accessKey = "ak";
            var secretKey = "sk";

            //var bucketName = "YOUR BUCKET NAME";
            //var objKeyName = "YOUR OBJECT KEY";

            /**
             * 设置服务地址</br>
             * 中国（北京）| ks3-cn-beijing.ksyun.com
             * 中国（上海）| ks3-cn-shanghai.ksyun.com
             * 中国（香港）| ks3-cn-hk-1.ksyun.com
             */
            var endPoint = "ks3-cn-beijing.ksyun.com";    //此处以北京region为例

            ks3Client = new KS3Client(accessKey, secretKey);
            ks3Client.SetEndpoint(endPoint);
        }

        private static void GetObjUrl()
        {
            DateTime date = DateTime.Now;
            date = date.AddMinutes(5);
            var url = ks3Client.GeneratePresignedUrl("ksc.harry", "14.jpg", date);
            Console.WriteLine("url:" + url);
        }

        private static bool Init()
        {
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
                catch (Exception e)
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
        private static bool HeadBucket()
        {
            // Head Bucket
            try
            {
                Console.WriteLine("--- Head Bucket: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                HeadBucketResult bucket = ks3Client.HeadBucket(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Head Bucket Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        private static bool GetBucketCorsConfig()
        {
            try
            {
                Console.WriteLine("--- getBucketCorsConfig: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                var bucketcors = ks3Client.GetBucketCors(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("getBucketCorsConfig Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        private static bool GetBucketLocation()
        {
            try
            {
                Console.WriteLine("--- getBucketLocation: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                var bucketLocation = ks3Client.GetBucketLocation(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("getBucketLocation Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        private static bool GetBucketLogging()
        {
            try
            {
                Console.WriteLine("--- getBucketLogging: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                var bucketlogging = ks3Client.GetBucketLogging(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("getBucketLogging Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        public static bool SetBucketCors()
        {
            try
            {
                Console.WriteLine("--- setBucketCors: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
                var putBucketCorsRequest = new PutBucketCorsRequest
                {
                    BucketName = bucketName
                };
                var bucketCorsConfiguration = new BucketCorsConfigurationResult();
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

                ks3Client.SetBucketCors(putBucketCorsRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("setBucketCors Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        public static bool SetBucketLogging()
        {
            try
            {
                Console.WriteLine("--- setBucketLogging: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                var bucketLogging = new GetBucketLoggingResult()
                {
                    Enable = true,
                    TargetBucket = "test1-zzy-jr",
                    TargetPrefix = "test"
                };

                /*BucketLogging.Enable 为false 则为关闭日志设置*/
                var putBucketLoggingRequest = new PutBucketLoggingRequest
                {
                    BucketName = bucketName,
                    BucketLogging = bucketLogging
                };

                ks3Client.SetBucketLogging(putBucketLoggingRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("setBucketLogging Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        public static bool DeleteBucketCors()
        {
            try
            {
                Console.WriteLine("--- deleteBucketCors: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
                var deleteBucketCorsRequest = new DeleteBucketCorsRequest
                {
                    BucketName = bucketName
                };

                ks3Client.DeleteBucketCors(deleteBucketCorsRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("deleteBucketCors Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        private static bool DeleteMultiObjects()
        {
            try
            {
                Console.WriteLine("--- deleteMultiObjects: ---");
                Console.WriteLine("Bucket Name: " + bucketName);
                var deleteMultipleObjectsRequest = new DeleteMultipleObjectsRequest
                {
                    BucketName = bucketName,
                    ObjectKeys = new String[] { "过滤条件.txt" }
                };
                var result = ks3Client.DeleteMultiObjects(deleteMultipleObjectsRequest);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("deleteMultiObjects Fail! " + e.ToString());
                return false;
            }
            return true;
        }


        private static bool CreateBucket()
        {
            // Create Bucket
            try
            {
                Console.WriteLine("--- Create Bucket: ---");
                Console.WriteLine("Bucket Name: " + bucketName);

                Bucket bucket = ks3Client.CreateBucket(bucketName);

                Console.WriteLine("Success.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine("Create Bucket Fail! " + e.ToString());
                return false;
            }
            return true;
        }

        private static bool ListBuckets()
        {
            // List Buckets
            try
            {
                Console.WriteLine("--- List Buckets: ---");

                IList<Bucket> bucketsList = ks3Client.ListBuckets();
                foreach (Bucket b in bucketsList)
                {
                    Console.WriteLine(b.ToString());
                }

                Console.WriteLine("---------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool GetBucketACL()
        {
            // Get Bucket ACL
            try
            {
                Console.WriteLine("--- Get Bucket ACL: ---");

                AccessControlList acl = ks3Client.GetBucketAcl(bucketName);
                Console.WriteLine("Bucket Name: " + bucketName);
                Console.WriteLine(acl.ToString());

                Console.WriteLine("-----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool SetBucketACL()
        {
            // Set Bucket ACL
            try
            {
                Console.WriteLine("--- Set Bucket ACL: ---");

                var cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                ks3Client.SetBucketAcl(bucketName, cannedAcl);

                Console.WriteLine("Bucket Name: " + bucketName);
                Console.WriteLine("Success, now the ACL is:\n" + ks3Client.GetBucketAcl(bucketName));
                Console.WriteLine("-----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool PutObject()
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
                var putObjectRequest = new PutObjectRequest(bucketName, objKeyNameFileData, file)
                {
                    CannedAcl = new CannedAccessControlList(CannedAccessControlList.PRIVATE),
                    ProgressListener = new SampleListener(file.Length)
                };

                var putObjectResult = ks3Client.PutObject(putObjectRequest);

                Console.WriteLine("Upload Completed. eTag=" + putObjectResult.ETag + ", MD5=" + putObjectResult.ContentMD5);
                Console.WriteLine("---------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static bool CopyObject()
        {
            try
            {
                Console.WriteLine("--- copyObject: ---");
                var copyObjectRequest = new CopyObjectRequest
                {
                    SourceObject = objKeyNameFileData,
                    SourceBucket = bucketName,
                    DestinationBucket = "test2-zzy-jr",
                    DestinationObject = objKeyNameFileData
                };
                //CannedAccessControlList cannedAcl=new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                //copyObjectRequest.CannedAcl = cannedAcl;

                var result = ks3Client.CopyObject(copyObjectRequest);
                Console.WriteLine("---------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
        public static bool HeadObject()
        {
            try
            {
                Console.WriteLine("--- headObject: ---");
                var headObjectRequest = new HeadObjectRequest
                {
                    BucketName = bucketName,
                    ObjectKey = objKeyNameFileData
                };
                var result = ks3Client.HeadObject(headObjectRequest);
                long length = result.ObjectMetadata.GetContentLength();
                Console.WriteLine("---------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 初始化分块上传，服务端会返回一个全局唯一的uploadid
        /// </summary>
        /// <returns></returns>
        private static InitiateMultipartUploadResult MultipartUp()
        {
            var re = ks3Client.InitiateMultipartUpload(bucketName, objKeyNameFileData);
            Console.WriteLine(re.ToString());
            return re;
        }

        /// <summary>
        ///  分块上传例子
        /// </summary>
        /// <returns></returns>
        private static bool UploadPart()
        {
            string path = @"you file path";//上传文件路径,例如E:\tool\aa.rar
            InitiateMultipartUploadResult result = MultipartUp();
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
                            result.Bucket, result.Key, result.UploadId,
                            i + 1);
                    //每次读取5M文件内容，如果最后一次内容不及5M则按实际大小取值
                    int count = Convert.ToInt32((i * part + part) > file.Length ? file.Length - i * part : part);
                    byte[] data = new byte[count];
                    int n = fs.Read(data, 0, count);

                    request.InputStream = new MemoryStream(data);
                    IProgressListener sampleListener = new SampleListener(count);//实例一个更新进度的监听类，实际使用中可自己定义实现
                    request.ProgressListener = sampleListener;
                    PartETag tag = ks3Client.UploadPart(request);//上传本次分块内容
                    Console.WriteLine(tag.ToString());
                    if (n == 0)
                        break;
                    numBytesToRead -= n;

                    XElement partE = new XElement("Part");
                    partE.Add(new XElement("PartNumber", i + 1));
                    partE.Add(new XElement("ETag", tag.ETag));
                    root.Add(partE);
                    i++;
                }
            }
            //所有分块上传完成后发起complete request，通知服务端合并分块
            var completeRequest = new CompleteMultipartUploadRequest(result.Bucket, result.Key, result.UploadId)
            {
                Content = new MemoryStream(Encoding.Default.GetBytes(root.ToString()))
            };
            _ = ks3Client.CompleteMultipartUpload(completeRequest);
            return true;
        }

        /// <summary>
        /// 获取已经上传的分块列表
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="objKey"></param>
        /// <param name="uploadId"></param>
        /// <returns></returns>
        private static bool ListMultipartUploads(string bucket, string objKey, string uploadId)
        {
            var request = new ListMultipartUploadsRequest(bucket, objKey, uploadId);
            _ = ks3Client.GetListMultipartUploads(request);
            return true;
        }

        /// <summary>
        /// 放弃本次上传
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="objKey"></param>
        /// <param name="uploadId"></param>
        /// <returns></returns>
        private static bool AbortMultipartUpload(string bucket, string objKey, string uploadId)
        {
            var request = new AbortMultipartUploadRequest(bucket, objKey, uploadId);
            ks3Client.AbortMultipartUpload(request);
            return true;
        }

        private static bool ListObjects()
        {
            try
            {
                // List Objects
                Console.WriteLine("--- List Objects: ---");

                //ObjectListing objects = ks3Client.listObjects(bucketName);

                IKS3 ks3Client = new KS3Client("ak", "sk");
                ks3Client.SetEndpoint("kss.ksyun.com");

                var request = new ListObjectsRequest
                {
                    BucketName = "haofenshu",
                    //request.setMarker("PersistenceServiceImpl.java");
                    Prefix = ("file/s/167206/1082/ClipedRecord.xml"),
                    Delimiter = ("/")
                };

                var objects = ks3Client.ListObjects(request);

                //Console.WriteLine(objects.ToString());
                Console.WriteLine("---------------------\n");

                // Get Object Metadata
                Console.WriteLine("--- Get Object Metadata ---");

                //ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameMemoryData); 
                //Console.WriteLine(objMeta.ToString());
                //Console.WriteLine();
                ObjectMetadata objMeta = ks3Client.GetObjectMetadata(bucketName, objKeyNameFileData);
                Console.WriteLine(objMeta.ToString());

                Console.WriteLine("---------------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool ListObjectsPage()
        {
            try
            {
                IKS3 ks3Client = new KS3Client(ak, sk);
                ks3Client.SetEndpoint("kss.ksyun.com");

                var request = new ListObjectsRequest
                {
                    BucketName = ("ksc.harry"),
                    MaxKeys = 20,
                    Delimiter = "/"
                };

                var objects = ks3Client.ListObjects(request);

                Console.WriteLine(objects);
                Console.WriteLine(objects.Truncated);
                Console.WriteLine(objects.NextMarker);

                request.Marker = objects.NextMarker;

                objects = ks3Client.ListObjects(request);

                Console.WriteLine(objects);
                Console.WriteLine(objects.Truncated);
                Console.WriteLine(objects.NextMarker);

                Console.WriteLine("---------------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool GetObjectACL()
        {
            // Get Object ACL
            try
            {
                Console.WriteLine("--- Get Object ACL: ---");

                AccessControlList acl = ks3Client.GetObjectAcl(bucketName, objKeyNameMemoryData);
                Console.WriteLine("Object Key: " + objKeyNameMemoryData);
                Console.WriteLine(acl.ToString());

                Console.WriteLine("-----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;

            }

            return true;
        }

        private static bool SetObjectACL()
        {
            // Set Object ACL
            try
            {
                Console.WriteLine("--- Set Object ACL: ---");

                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                Console.WriteLine("Object Key: " + objKeyNameMemoryData);
                ks3Client.SetObjectAcl(bucketName, objKeyNameMemoryData, cannedAcl);

                Console.WriteLine("Success, now the ACL is:\n" + ks3Client.GetObjectAcl(bucketName, objKeyNameMemoryData));
                Console.WriteLine("-----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool GetObject()
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
                ObjectMetadata objectMetadata = ks3Client.GetObjectMetadata(bucketName, objKeyNameFileData);

                var getObjectRequest = new GetObjectRequest(bucketName, objKeyNameFileData, new FileInfo(outFilePath))
                {
                    ProgressListener = new SampleListener(objectMetadata.GetContentLength())
                };

                var obj = ks3Client.GetObject(getObjectRequest);
                obj.ObjectContent.Close(); // The file was opened in [KS3ObjectResponseHandler], so I close it first. 

                Console.WriteLine("Success. See the file downloaded at {0}", outFilePath);
                Console.WriteLine("-----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool DeleteObject()
        {
            // Delete Object
            try
            {
                Console.WriteLine("--- Delete Object: ---");

                ks3Client.DeleteObject(bucketName, objKeyNameMemoryData);
                ks3Client.DeleteObject(bucketName, objKeyNameFileData);

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

        private static bool DeleteBucket()
        {
            // Delete Bucket
            try
            {
                Console.WriteLine("--- Delete Bucket: ---");

                ks3Client.DeleteBucket(bucketName);

                Console.WriteLine("Delete Bucket completed.");
                Console.WriteLine("----------------------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        private static bool CatchServiceException()
        {
            // This is a sample of catch ServiceException of KS3.
            // You can catch the ServiceException when some illegal operations appear.
            // But note that, if we have done some illegal operations, there also may appear some other unexcepted exceptions too.
            // Now we will see a ServiceException because will try to delete a bucket that does not exist.
            try
            {
                Console.WriteLine("--- Catch ServiceException: ---");

                ks3Client.DeleteBucket(bucketName);

                Console.WriteLine("-------------------------------\n");
            }
            catch (ServiceException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool GeneratePresignedUrl()
        {
            Console.WriteLine("--- generate Presigned Url: ---");
            string url = ks3Client.GeneratePresignedUrl(bucketName, objKeyNameFileData, DateTime.Now.AddMinutes(5));
            Console.WriteLine("success generate presigned url:" + url);
            Console.WriteLine("-------------------------------\n");
            return true;
        }
        private static bool GetAdpTask()
        {
            try
            {
                Console.WriteLine("--- getAdpTask begin: ---");
                GetAdpRequest getAdpRequest = new GetAdpRequest
                {
                    TaskId = "00P99HHVuHlS"
                };
                var result = ks3Client.GetAdpTask(getAdpRequest);

                Console.WriteLine("---------getAdpTask end;--------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private static bool PutAdpTask()
        {
            try
            {
                Console.WriteLine("--- putAdpTask begin: ---");
                var putAdpRequest = new PutAdpRequest
                {
                    BucketName = "kingsoft.test.ml",
                    ObjectKey = "testPut.mp4"
                };
                IList<Adp> fops = new List<Adp>();
                Adp fop12 = new Adp
                {
                    Command = "tag=avscrnshot&ss=5",
                    Bucket = "kingsoft.test.ml",
                    Key = "testAdp.jpg"
                };
                fops.Add(fop12);
                putAdpRequest.Adps = fops;
                putAdpRequest.NotifyURL = "http://10.4.2.38:19090/";
                String taskid = ks3Client.PutAdpTask(putAdpRequest);

                Console.WriteLine("---------putAdpTask end; taskid:" + taskid + "---------\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }
    }

    class SampleListener : IProgressListener
    {
        readonly long _size = -1;
        long _completedSize = 0;
        int _rate = 0;
        readonly bool _cancled = false;

        public SampleListener() { }

        public SampleListener(long size)
        {
            _size = size;
        }

        public void ProgressChanged(ProgressEvent progressEvent)
        {
            int eventCode = progressEvent.EventCode;

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
                _completedSize += progressEvent.BytesTransferred;
                int newRate = (int)((double)_completedSize / _size * 100 + 0.5);
                if (newRate > _rate)
                {
                    _rate = newRate;
                    Console.WriteLine("Processing ... " + _rate + "%");
                }
            }
        }

        public bool AskContinue()
        {
            return !_cancled;
        }
    }
}
