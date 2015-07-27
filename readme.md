# KS3 SDK For C#使用指南 
---
## 1 概述
此SDK适用于.net framework3.5及以上版本。基于KS3 API 构建。使用此 SDK 构建您的网络应用程序，能让您以非常便捷地方式将数据安全地存储到金山云存储上。无论您的网络应用是一个网站程序，还是包括从云端（服务端程序）到终端（手持设备应用）的架构的服务或应用，通过KS3存储及其 SDK，都能让您应用程序的终端用户高速上传和下载，同时也让您的服务端更加轻盈。  

## 2 环境准备
配置.net framework3.5 以上开发环境  
下载KS3 SDK For C#  
#####由于在App端明文存储AccessKey、SecretKey是极不安全的，因此推荐的使用场景如下图所示：
    ![Alt text](http://androidsdktest21.kssws.ks-cdn.com/ks3-android-sdk-authlistener.png)




## 3 初始化

### 3.1 获取秘钥
1、开通KS3服务，[http://www.ksyun.com/user/register](http://www.ksyun.com/user/register) 注册账号  
2、进入控制台, [http://ks3.ksyun.com/console.html#/setting](http://ks3.ksyun.com/console.html#/setting) 获取AccessKeyID 、AccessKeySecret
### 3.2 初始化客户端
当以上全部完成之后用户便可初始化客户端进行操作了  

	static String accessKey = "YOUR ACCESS KEY";
    static String secretKey = "YOUR SECRET KEY";
    static String bucketName = "YOUR BUCKET NAME";
	static String objKeyName = "YOUR OBJECT KEY";
    static KS3Client ks3Client = new KS3Client(accessKey, secretKey);

## 4 使用示例

#### 4.1 List Buckets
##### 使用示例
列出当前用户的所有bucket,可以查看每个bucket的名称、创建时间以及所有者

	private static bool listBuckets()
		{
            // List Buckets
			try
			{
				IList<Bucket> bucketsList = ks3Client.listBuckets();
				foreach (Bucket b in bucketsList)
				{
					Console.WriteLine(b.ToString());
				}
			}
			catch (System.Exception e)
			{
				return false;
			}
			return true;
		}

#### 4.2 DELETE Bucket
##### 使用示例
删除一个Bucket

	private static bool deleteBucket()
		{
            // Delete Bucket
            try
            {
                ks3Client.deleteBucket(bucketName);
			}
            catch (System.Exception e)
            {
				return false;
            }
			return true;
        }


#### 4.3 List Objects
##### 使用示例
列出<bucket名称>下的所有object，最大上限是1000个

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

				ObjectMetadata objMeta = ks3Client.getObjectMetadata(bucketName, objKeyName); 
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

#### 4.4 GET Bucket acl
#####  使用示例
获取<bucket名称>的acl控制权限

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

#### 4.5 List Multipart Uploads
#####  使用示例
列出当前正在执行的分块上传

	 private static ListMultipartUploadsResult listMultipartUploads(string bucket,string objKey,string uploadId) {
            ListMultipartUploadsRequest request = new ListMultipartUploadsRequest(bucket,objKey,uploadId);
            ListMultipartUploadsResult result =ks3Client.getListMultipartUploads(request);
            return result;
        }

#### 4.6 Create Bucket
##### 使用示例
新建一个<bucket名称>

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
注：这里如果出现409 conflict错误，说明请求的bucket name有冲突，因为bucket name是全局唯一的

#### 4.7 PUT Bucket acl
#####  使用示例
设置<bucket名称>的访问权限

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


#### 4.8 DELETE Object
#####  使用示例
删除<bucket名称>内一个object

	private static bool deleteObject()
		{
			// Delete Object
			try
			{
				Console.WriteLine("--- Delete Object: ---");

				ks3Client.deleteObject(bucketName, objKeyName);

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

#### 4.9 GET Object
#####  使用示例
下载<bucket名称>下的object

	private static bool getObject()
		{
			try
			{
                // GET Object为用户提供了object的下载，用户可以通过控制Range实现分块多线程下载
				Console.WriteLine("--- Download and Store in Memory ---");
				
				GetObjectRequest getShortContent = new GetObjectRequest(bucketName, objKeyName);
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
                // 直接下载并存储成文件
				Console.WriteLine("--- Download a File ---");

				// I need to get the Content-Length to set the listener.
				ObjectMetadata objectMetadata = ks3Client.getObjectMetadata(bucketName, objKeyName); 
				
				SampleListener downloadListener = new SampleListener(objectMetadata.getContentLength());
				GetObjectRequest getObjectRequest = new GetObjectRequest(bucketName, objKeyName, new FileInfo(outFilePath));
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

#### 4.10 GET Object acl
#####  使用示例

获取 <bucket名称>这个bucket下<object key>的权限控制信息

    private static bool getObjectACL()
        {
            // Get Object ACL
            try
            {
                Console.WriteLine("--- Get Object ACL: ---");

                AccessControlList acl = ks3Client.getObjectAcl(bucketName ,objKeyName);
                Console.WriteLine("Object Key: " + objKeyName);
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


#### 4.11 PUT Object
#####  使用示例

将new File("<filePath>")这个文件上传至<bucket名称>这个存储空间下，并命名为<object key>

    private static bool putObject()
		{
			try
            {
                 //Put Object(upload a short content)
                Console.WriteLine("--- Upload a Short Content: ---");

                String sampleContent = "This is a sample content.(25 characters before, included the 4 spaces)";
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(sampleContent));
                PutObjectResult shortContentResult = ks3Client.putObject(bucketName, objKeyName, stream, null);
	
                Console.WriteLine("Upload Completed. eTag=" + shortContentResult.getETag() + ", MD5=" + shortContentResult.getContentMD5());
                Console.WriteLine("-------------------------------\n");

                 //Put Object(upload a file)
				Console.WriteLine("--- Upload a File ---");

				FileInfo file = new FileInfo("E:\\tool\\eclipse.rar");
				PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, objKeyName, file);
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


#### 4.12 PUT Object acl
#####  使用示例
修改<bucket名称>下object的权限控制

	 private static bool setObjectACL()
        {
            // Set Object ACL
            try
            {
                Console.WriteLine("--- Set Object ACL: ---");

                CannedAccessControlList cannedAcl = new CannedAccessControlList(CannedAccessControlList.PUBLICK_READ_WRITE);
                Console.WriteLine("Object Key: " + objKeyName);
                ks3Client.setObjectAcl(bucketName, objKeyName, cannedAcl);

                Console.WriteLine("Success, now the ACL is:\n" + ks3Client.getObjectAcl(bucketName, objKeyName));
                Console.WriteLine("-----------------------\n");
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

#### 4.13 Multipart Upload
#####  使用示例
注：中途想停止分块上传的话请调用AbortMultipartUpload(bucketname, objectkey, uploadId);


	/**
         * 初始化分块上传，服务端会返回一个全局唯一的uploadid
         * **/
        private static InitiateMultipartUploadResult multipartUp()
        {
            InitiateMultipartUploadResult re=ks3Client.initiateMultipartUpload(bucketName, objKeyName);
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
         * 放弃本次上传
         * **/
        private static bool AbortMultipartUpload(string bucket, string objKey, string uploadId)
        {
            AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, objKey, uploadId);
            ks3Client.AbortMultipartUpload(request);
            return true;
        }