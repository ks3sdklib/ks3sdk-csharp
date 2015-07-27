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

namespace KS3Sample
{
    class KS3Sample
    {
        static String accessKey = "YOUR ACCESS KEY";
        static String secretKey = "YOUR SECRET KEY";

		// KS3 Operation class 
		static KS3Client ks3Client = null;

        static String bucketName = "test-bucketname";
		static String objKeyNameMemoryData	= "short-content";
		static String objKeyNameFileData	= "file-data";

        static String inFilePath = "E:\\tool\\abc.rar";
		static String outFilePath = "D:/test.out.data";


		static void Main(string[] args)
        {
            if (!init())
                return;		// init failed 

            Console.WriteLine("========== Begin ==========\n");
            //createBucket();
            //listBuckets();
            //getBucketACL();
            //setBucketACL();
            //putObject();
            //listObjects();
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
            generatePresignedUrl();
            Console.WriteLine("\n==========  End  ==========");
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

            bucketName = "test-ks3-bucket-" + new Random().Next();
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
                 //Put Object(upload a short content)
                Console.WriteLine("--- Upload a Short Content: ---");

                String sampleContent = "This is a sample content.(25 characters before, included the 4 spaces)";
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleContent));
                PutObjectResult shortContentResult = ks3Client.putObject(bucketName, objKeyNameMemoryData, stream, null);
	
                Console.WriteLine("Upload Completed. eTag=" + shortContentResult.getETag() + ", MD5=" + shortContentResult.getContentMD5());
                Console.WriteLine("-------------------------------\n");

                 //Put Object(upload a file)
				Console.WriteLine("--- Upload a File ---");

				FileInfo file = new FileInfo("E:\\tool\\eclipse.rar");
				PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, objKeyNameFileData, file);
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

				ObjectListing objects = ks3Client.listObjects(bucketName);

				Console.WriteLine(objects.ToString());
				Console.WriteLine("---------------------\n");

				// Get Object Metadata
				Console.WriteLine("--- Get Object Metadata ---");

				ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameMemoryData); 
				Console.WriteLine(objMeta.ToString());
                Console.WriteLine();
				objMeta = ks3Client.getObjectMetadata(bucketName, objKeyNameFileData);
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