using KS3.Auth;
using KS3.Extensions;
using KS3.Http;
using KS3.Internal;
using KS3.KS3Exception;
using KS3.Model;
using KS3.Transform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace KS3
{
    public class KS3Client : IKS3
    {
        private readonly XmlResponseHandler<Type> _voidResponseHandler = new XmlResponseHandler<Type>(null);

        /// <summary>
        /// KS3 credentials.
        /// </summary>
        private readonly IKS3Credentials _ks3Credentials;

        /// <summary>
        /// The service endpoint to which this client will send requests.
        /// </summary>
        private Uri _endpoint;

        /// <summary>
        /// The client configuration
        /// </summary>
        private ClientConfiguration _clientConfiguration;

        /// <summary>
        /// Low level client for sending requests to KS3.
        /// </summary>
        private KS3HttpClient _client;

        /// <summary>
        /// Optional offset (in seconds) to use when signing requests
        /// </summary>
        private int _timeOffset;

        /// <summary>
        /// Constructs a new KS3Client object using the specified Access Key ID and Secret Key.
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public KS3Client(string accessKey, string secretKey) : this(new BasicKS3Credentials(accessKey, secretKey))
        {
        }

        /// <summary>
        /// Constructs a new KS3Client object using the specified configuration.
        /// </summary>
        /// <param name="ks3Credentials"></param>
        public KS3Client(IKS3Credentials ks3Credentials) : this(ks3Credentials, new ClientConfiguration())
        {
        }

        /// <summary>
        ///  Constructs a new KS3Client object using the specified Access Key ID, Secret Key and configuration.
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="clientConfiguration"></param>
        public KS3Client(string accessKey, string secretKey, ClientConfiguration clientConfiguration)
            : this(new BasicKS3Credentials(accessKey, secretKey), clientConfiguration)
        {
        }

        /// <summary>
        /// Constructs a new KS3Client object using the specified credential and configuration.
        /// </summary>
        /// <param name="ks3Credentials"></param>
        /// <param name="clientConfiguration"></param>
        public KS3Client(IKS3Credentials ks3Credentials, ClientConfiguration clientConfiguration)
        {
            _clientConfiguration = clientConfiguration;
            _client = new KS3HttpClient(clientConfiguration);
            _ks3Credentials = ks3Credentials;

            Init();
        }

        private void Init()
        {
            SetEndpoint(Constants.KS3_HOSTNAME);
        }

        public void SetEndpoint(string endpoint)
        {
            if (!endpoint.Contains("://"))
            {
                endpoint = _clientConfiguration.Protocol + "://" + endpoint;
            }
            _endpoint = new Uri(endpoint);
        }

        public void SetConfiguration(ClientConfiguration clientConfiguration)
        {
            _clientConfiguration = clientConfiguration;
            _client = new KS3HttpClient(clientConfiguration);
        }

        /// <summary>
        ///  * Sets the optional value for time offset for this client.  This
        ///  * value will be applied to all requests processed through this client.
        ///  * Value is in seconds, positive values imply the current clock is "fast",
        ///  * negative values imply clock is slow.
        /// </summary>
        /// <param name="timeOffset"></param>
        public void SetTimeOffset(int timeOffset)
        {
            _timeOffset = timeOffset;
        }

        /// <summary>
        /// * Returns the optional value for time offset for this client.  This
        /// * value will be applied to all requests processed through this client.
        /// * Value is in seconds, positive values imply the current clock is "fast",
        /// * negative values imply clock is slow.
        /// </summary>
        /// <returns></returns>
        public int GetTimeOffset()
        {
            return _timeOffset;
        }

        /// <summary>
        /// Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
        /// </summary>
        /// <returns></returns>
        public IList<Bucket> ListBuckets()
        {
            return ListBuckets(new ListBucketsRequest());
        }

        /// <summary>
        /// Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
        /// </summary>
        /// <param name="listBucketsRequest"></param>
        /// <returns></returns>
        public IList<Bucket> ListBuckets(ListBucketsRequest listBucketsRequest)
        {
            IRequest<ListBucketsRequest> request = CreateRequest(
                null,
                null,
                listBucketsRequest,
                HttpMethod.GET);

            return Invoke(request, new ListBucketsUnmarshaller(), null, null);
        }

        /// <summary>
        ///  Deletes the specified bucket. 
        /// </summary>
        /// <param name="bucketName"></param>
        public void DeleteBucket(string bucketName)
        {
            DeleteBucket(new DeleteBucketRequest(bucketName));
        }

        /// <summary>
        /// Deletes the specified bucket. 
        /// </summary>
        /// <param name="deleteBucketRequest"></param>
        public void DeleteBucket(DeleteBucketRequest deleteBucketRequest)
        {
            var bucketName = deleteBucketRequest.BucketName;

            IRequest<DeleteBucketRequest> request = CreateRequest(
                bucketName,
                null,
                deleteBucketRequest,
                HttpMethod.DELETE);
            Invoke(request, _voidResponseHandler, bucketName, null);
        }

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public AccessControlList GetBucketAcl(String bucketName)
        {
            return GetBucketAcl(new GetBucketAclRequest(bucketName));
        }

        /// <summary>
        ///  Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="getBucketAclRequest"></param>
        /// <returns></returns>
        public AccessControlList GetBucketAcl(GetBucketAclRequest getBucketAclRequest)
        {
            var bucketName = getBucketAclRequest.BucketName;

            IRequest<GetBucketAclRequest> request = CreateRequest(
                bucketName, null,
                getBucketAclRequest,
                HttpMethod.GET);

            request.SetParameter("acl", null);

            return Invoke(request, new AccessControlListUnmarshaller(), bucketName, null);
        }

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public Bucket CreateBucket(String bucketName)
        {
            return CreateBucket(new CreateBucketRequest(bucketName));
        }

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="createBucketRequest"></param>
        /// <returns></returns>
        public Bucket CreateBucket(CreateBucketRequest createBucketRequest)
        {
            var bucketName = createBucketRequest.BucketName;

            IRequest<CreateBucketRequest> request = CreateRequest(
                bucketName,
                null,
                createBucketRequest,
                HttpMethod.PUT);

            request.SetHeader(Headers.CONTENT_LENGTH, "0");

            if (createBucketRequest.Acl != null)
            {
                addAclHeaders(request, createBucketRequest.Acl);
            }
            else if (createBucketRequest.CannedAcl != null)
            {
                request.SetHeader(Headers.KS3_CANNED_ACL, createBucketRequest.CannedAcl.CannedAclHeader);
            }

            Invoke(request, _voidResponseHandler, bucketName, null);

            return new Bucket(bucketName);
        }
        /// <summary>
        /// This operation is useful to determine if a bucket exists and you have permission to access it
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public HeadBucketResult HeadBucket(string bucketName)
        {
            return HeadBucket(new HeadBucketRequest(bucketName));
        }

        /// <summary>
        /// This operation is useful to determine if a bucket exists and you have permission to access it
        /// </summary>
        /// <param name="headBucketRequest"></param>
        /// <returns></returns>
        public HeadBucketResult HeadBucket(HeadBucketRequest headBucketRequest)
        {
            String bucketname = headBucketRequest.BucketName;
            IRequest<HeadBucketRequest> request = this.CreateRequest(bucketname, null, headBucketRequest, HttpMethod.HEAD);
            return this.Invoke(request, new HeadBucketResponseHandler(), bucketname, null);
        }

        /// <summary>
        /// Returns the cors configuration information set for the bucket.
        /// To use this operation, you must have permission to perform the s3:GetBucketCORS action. By default, the bucket owner has this permission and can grant it to others.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public BucketCorsConfigurationResult GetBucketCors(string bucketName)
        {
            return GetBucketCors(new GetBucketCorsRequest(bucketName));
        }

        /// <summary>
        /// Returns the cors configuration information set for the bucket.
        /// To use this operation, you must have permission to perform the s3:GetBucketCORS action. By default, the bucket owner has this permission and can grant it to others.
        /// </summary>
        /// <param name="getBucketCorsRequest"></param>
        /// <returns></returns>
        public BucketCorsConfigurationResult GetBucketCors(GetBucketCorsRequest getBucketCorsRequest)
        {
            var bucketname = getBucketCorsRequest.BucketName;
            IRequest<GetBucketCorsRequest> request = CreateRequest(
                bucketname,
                null,
                getBucketCorsRequest,
                HttpMethod.GET);
            request.SetParameter("cors", null);

            var result = Invoke(
                request,
                new BucketCorsConfigurationResultUnmarshaller(),
                bucketname,
                null);
            return result;
        }

        /// <summary>
        /// This implementation of the GET operation uses the location subresource to return a bucket's region. You set the bucket's region using the LocationConstraint request parameter in a PUT Bucket request. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public GetBucketLocationResult GetBucketLocation(string bucketName)
        {
            return GetBucketLocation(new GetBucketLocationRequest(bucketName));
        }

        /// <summary>
        /// This implementation of the GET operation uses the location subresource to return a bucket's region. You set the bucket's region using the LocationConstraint request parameter in a PUT Bucket request. 
        /// </summary>
        /// <param name="getBucketLocationRequest"></param>
        /// <returns></returns>
        public GetBucketLocationResult GetBucketLocation(GetBucketLocationRequest getBucketLocationRequest)
        {
            var request = CreateRequest(
                getBucketLocationRequest.BucketName,
                null,
                getBucketLocationRequest,
                HttpMethod.GET);

            request.Parameters.Add("location", null);

            var result = Invoke(
                request,
                new GetBucketLocationResultUnmarshaller(),
                getBucketLocationRequest.BucketName,
                null);
            return result;
        }

        /// <summary>
        /// This implementation of the GET operation uses the logging subresource to return the logging status of a bucket and the permissions users have to view and modify that status. To use GET, you must be the bucket owner. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public GetBucketLoggingResult GetBucketLogging(string bucketName)
        {
            return GetBucketLogging(new GetBucketLoggingRequest(bucketName));
        }

        /// <summary>
        /// This implementation of the GET operation uses the logging subresource to return the logging status of a bucket and the permissions users have to view and modify that status. To use GET, you must be the bucket owner. 
        /// </summary>
        /// <param name="getBucketLoggingRequest"></param>
        /// <returns></returns>
        public GetBucketLoggingResult GetBucketLogging(GetBucketLoggingRequest getBucketLoggingRequest)
        {
            var request = CreateRequest(
                getBucketLoggingRequest.BucketName,
                null,
                getBucketLoggingRequest,
                HttpMethod.GET);

            request.Parameters.Add("logging", null);
            var result = Invoke(
                request,
                new GetBucketLoggingResultUnmarshaller(),
                getBucketLoggingRequest.BucketName,
                null);

            return result;
        }

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="acl"></param>
        public void SetBucketAcl(string bucketName, AccessControlList acl)
        {
            SetBucketAcl(new SetBucketAclRequest(bucketName, acl));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="cannedAcl"></param>
        public void SetBucketAcl(string bucketName, CannedAccessControlList cannedAcl)
        {
            SetBucketAcl(new SetBucketAclRequest(bucketName, cannedAcl));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="setBucketAclRequest"></param>
        public void SetBucketAcl(SetBucketAclRequest setBucketAclRequest)
        {

            var request = CreateRequest(
                setBucketAclRequest.BucketName,
                null,
                setBucketAclRequest,
                HttpMethod.PUT);

            if (setBucketAclRequest.Acl != null)
            {
                var xml = AclXmlFactory.ConvertToXmlString(setBucketAclRequest.Acl);
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

                request.Content = (memoryStream);
                request.SetHeader(Headers.CONTENT_LENGTH, memoryStream.Length.ToString());
            }
            else if (setBucketAclRequest.CannedAcl != null)
            {
                request.SetHeader(Headers.KS3_CANNED_ACL, setBucketAclRequest.CannedAcl.CannedAclHeader);
                request.SetHeader(Headers.CONTENT_LENGTH, "0");
            }

            request.SetParameter("acl", null);

            Invoke(request, _voidResponseHandler, setBucketAclRequest.BucketName, null);
        }

        /// <summary>
        /// Sets the cors configuration for your bucket. If the configuration exists, Amazon S3 replaces it. 
        /// </summary>
        /// <param name="putBucketCorsRequest"></param>
        public void SetBucketCors(PutBucketCorsRequest putBucketCorsRequest)
        {
            var request = CreateRequest(putBucketCorsRequest.BucketName, null, putBucketCorsRequest, HttpMethod.PUT);

            request.SetParameter("cors", null);
            request.SetHeader(Headers.CONTENT_LENGTH, putBucketCorsRequest.ToXmlAdapter().Length.ToString());
            request.SetHeader(Headers.CONTENT_TYPE, "application/xml");
            request.SetHeader(Headers.CONTENT_MD5, putBucketCorsRequest.GetMd5());
            request.Content = (putBucketCorsRequest.ToXmlAdapter());
            Invoke(request, _voidResponseHandler, putBucketCorsRequest.BucketName, null);
        }

        /// <summary>
        /// This implementation of the PUT operation uses the logging subresource to set the logging parameters for a bucket and to specify permissions for who can view and modify the logging parameters. To set the logging status of a bucket, you must be the bucket owner.
        /// </summary>
        /// <param name="putBucketLoggingRequest"></param>
        public void SetBucketLogging(PutBucketLoggingRequest putBucketLoggingRequest)
        {
            var request = CreateRequest(putBucketLoggingRequest.BucketName, null, putBucketLoggingRequest, HttpMethod.PUT);
            request.SetParameter("logging", null);
            request.SetHeader(Headers.CONTENT_LENGTH, putBucketLoggingRequest.ToXmlAdapter().Length.ToString());
            request.SetHeader(Headers.CONTENT_TYPE, "application/xml");
            request.Content = (putBucketLoggingRequest.ToXmlAdapter());
            Invoke(request, _voidResponseHandler, putBucketLoggingRequest.BucketName, null);
        }


        /// <summary>
        /// Deletes the cors configuration information set for the bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        public void DeleteBucketCors(string bucketName)
        {
            DeleteBucketCors(new DeleteBucketCorsRequest(bucketName));
        }

        /// <summary>
        /// Deletes the cors configuration information set for the bucket.
        /// </summary>
        /// <param name="deleteBucketCorsRequest"></param>
        public void DeleteBucketCors(DeleteBucketCorsRequest deleteBucketCorsRequest)
        {
            var request = CreateRequest(deleteBucketCorsRequest.BucketName, null, deleteBucketCorsRequest, HttpMethod.DELETE);
            request.SetParameter("cors", null);
            Invoke(request, _voidResponseHandler, deleteBucketCorsRequest.BucketName, null);
        }

        /// <summary>
        /// The Multi-Object Delete operation enables you to delete multiple objects from a bucket using a single HTTP request.
        /// </summary>
        /// <param name="deleteMultipleObjectsRequest"></param>
        /// <returns></returns>
        public DeleteMultipleObjectsResult DeleteMultiObjects(DeleteMultipleObjectsRequest deleteMultipleObjectsRequest)
        {
            var request = CreateRequest(deleteMultipleObjectsRequest.BucketName, null, deleteMultipleObjectsRequest, HttpMethod.POST);
            request.SetParameter("delete", null);
            request.SetHeader(Headers.CONTENT_LENGTH, deleteMultipleObjectsRequest.ToXmlAdapter().Length.ToString());
            request.SetHeader(Headers.CONTENT_TYPE, "application/xml");
            request.SetHeader(Headers.CONTENT_MD5, deleteMultipleObjectsRequest.GetMd5());
            request.Content = (deleteMultipleObjectsRequest.ToXmlAdapter());
            return Invoke(request, new DeleteMultipleObjectsResultUnmarshaller(), deleteMultipleObjectsRequest.BucketName, null);
        }

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public ObjectListing ListObjects(String bucketName)
        {
            return ListObjects(new ListObjectsRequest(bucketName, null, null, null, null));
        }

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public ObjectListing ListObjects(string bucketName, string prefix)
        {
            return ListObjects(new ListObjectsRequest(bucketName, prefix, null, null, null));
        }

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="listObjectRequest"></param>
        /// <returns></returns>
        public ObjectListing ListObjects(ListObjectsRequest listObjectRequest)
        {
            var request = CreateRequest(listObjectRequest.BucketName, null, listObjectRequest, HttpMethod.GET);

            if (!listObjectRequest.Prefix.IsNullOrWhiteSpace())
            {
                request.SetParameter("prefix", listObjectRequest.Prefix);
            }

            if (!listObjectRequest.Marker.IsNullOrWhiteSpace())
            {
                request.SetParameter("marker", listObjectRequest.Marker);
            }

            if (!listObjectRequest.Delimiter.IsNullOrWhiteSpace())
            {
                request.SetParameter("delimiter", listObjectRequest.Delimiter);
            }

            if (listObjectRequest.MaxKeys.HasValue && listObjectRequest.MaxKeys.Value > 0)
            {
                request.SetParameter("max-keys", listObjectRequest.MaxKeys.ToString());
            }

            return Invoke(request, new ListObjectsUnmarshallers(), listObjectRequest.BucketName, null);
        }

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        public void DeleteObject(string bucketName, string key)
        {
            DeleteObject(new DeleteObjectRequest(bucketName, key));
        }

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="deleteObjectRequest"></param>
        public void DeleteObject(DeleteObjectRequest deleteObjectRequest)
        {
            var request = CreateRequest(deleteObjectRequest.BucketName, deleteObjectRequest.Key, deleteObjectRequest, HttpMethod.DELETE);
            Invoke(request, _voidResponseHandler, deleteObjectRequest.BucketName, deleteObjectRequest.Key);
        }

        /// <summary>
        ///  Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public KS3Object GetObject(string bucketName, string key)
        {
            return GetObject(new GetObjectRequest(bucketName, key));
        }

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key, and saves the object contents to the specified file.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="destinationFile"></param>
        /// <returns></returns>
        public KS3Object GetObject(string bucketName, string key, FileInfo destinationFile)
        {
            return GetObject(new GetObjectRequest(bucketName, key, destinationFile));
        }

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="getObjectRequest"></param>
        /// <returns></returns>
        public KS3Object GetObject(GetObjectRequest getObjectRequest)
        {
            var request = CreateRequest(getObjectRequest.BucketName, getObjectRequest.Key, getObjectRequest, HttpMethod.GET);

            if (getObjectRequest.Range != null)
            {
                var range = getObjectRequest.Range;
                request.SetHeader(Headers.RANGE, range[0].ToString() + "-" + range[1].ToString());
            }

            AddDateHeader(request, Headers.GET_OBJECT_IF_MODIFIED_SINCE, getObjectRequest.ModifiedSinceConstraint);
            AddDateHeader(request, Headers.GET_OBJECT_IF_UNMODIFIED_SINCE, getObjectRequest.UnmodifiedSinceConstraint);
            AddStringListHeader(request, Headers.GET_OBJECT_IF_MATCH, getObjectRequest.MatchingETagConstraints);
            AddStringListHeader(request, Headers.GET_OBJECT_IF_NONE_MATCH, getObjectRequest.NonmatchingETagContraints);

            var progressListener = getObjectRequest.ProgressListener;

            FireProgressEvent(progressListener, ProgressEvent.STARTED);

            KS3Object ks3Object;
            try
            {
                ks3Object = Invoke(request, new ObjectResponseHandler(getObjectRequest), getObjectRequest.BucketName, getObjectRequest.Key);
            }
            catch (ProgressInterruptedException e)
            {
                FireProgressEvent(progressListener, ProgressEvent.CANCELED);
                throw e;
            }
            catch (Exception e)
            {
                FireProgressEvent(progressListener, ProgressEvent.FAILED);
                throw e;
            }
            FireProgressEvent(progressListener, ProgressEvent.COMPLETED);

            ks3Object.BucketName = getObjectRequest.BucketName;
            ks3Object.Key = getObjectRequest.Key;

            return ks3Object;
        }

        /// <summary>
        /// Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ObjectMetadata GetObjectMetadata(string bucketName, string key)
        {
            return GetObjectMetadata(new GetObjectMetadataRequest(bucketName, key));
        }

        /// <summary>
        ///  Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="getObjectMetadataRequest"></param>
        /// <returns></returns>
        public ObjectMetadata GetObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest)
        {
            var request = CreateRequest(getObjectMetadataRequest.BucketName, getObjectMetadataRequest.Key, getObjectMetadataRequest, HttpMethod.HEAD);

            return Invoke(request, new MetadataResponseHandler(), getObjectMetadataRequest.BucketName, getObjectMetadataRequest.Key);
        }

        /// <summary>
        /// Uploads the specified file to KS3 under the specified bucket and key name.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public PutObjectResult PutObject(string bucketName, string key, FileInfo file)
        {
            var putObjectRequest = new PutObjectRequest(bucketName, key, file)
            {
                Metadata = new ObjectMetadata()
            };
            return PutObject(putObjectRequest);
        }

        /// <summary>
        /// Uploads the specified input stream and object metadata to KS3 under the specified bucket and key name.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public PutObjectResult PutObject(string bucketName, string key, Stream input, ObjectMetadata metadata)
        {
            return PutObject(new PutObjectRequest(bucketName, key, input, metadata));
        }

        /// <summary>
        /// Uploads a new object to the specified KS3 bucket.
        /// </summary>
        /// <param name="putObjectRequest"></param>
        /// <returns></returns>
        public PutObjectResult PutObject(PutObjectRequest putObjectRequest)
        {
            string bucketName = putObjectRequest.BucketName;
            string key = putObjectRequest.Key;
            ObjectMetadata metadata = putObjectRequest.Metadata;
            Stream input = putObjectRequest.InputStream;
            IProgressListener progressListener = putObjectRequest.ProgressListener;
            if (metadata == null)
            {
                metadata = new ObjectMetadata();
            }

            // If a file is specified for upload, we need to pull some additional
            // information from it to auto-configure a few options
            if (putObjectRequest.File != null)
            {
                FileInfo file = putObjectRequest.File;

                // Always set the content length, even if it's already set
                metadata.SetContentLength(file.Length);

                // Only set the content type if it hasn't already been set
                if (metadata.GetContentType() == null)
                {
                    metadata.SetContentType(Mimetypes.GetMimetype(file));
                }
                if (metadata.GetContentMD5() == null)
                {
                    using (FileStream fileStream = file.OpenRead())
                    {
                        MD5 md5 = MD5.Create();
                        metadata.SetContentMD5(Convert.ToBase64String(md5.ComputeHash(fileStream)));
                    }
                }

                input = file.OpenRead();
            }
            else
            {
                metadata.SetContentLength(input.Length);

                if (metadata.GetContentType().IsNullOrWhiteSpace())
                {
                    metadata.SetContentType(Mimetypes.DEFAULT_MIMETYPE);
                }

                if (metadata.GetContentMD5().IsNullOrWhiteSpace())
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        metadata.SetContentMD5(Convert.ToBase64String(md5.ComputeHash(input)));
                    }

                    input.Seek(0, SeekOrigin.Begin); // It is needed after calculated MD5.
                }
            }

            var request = CreateRequest(bucketName, key, putObjectRequest, HttpMethod.PUT);

            if (putObjectRequest.Acl != null)
            {
                addAclHeaders(request, putObjectRequest.Acl);
            }
            else if (putObjectRequest.CannedAcl != null)
            {
                request.SetHeader(Headers.KS3_CANNED_ACL, putObjectRequest.CannedAcl.CannedAclHeader);
            }

            if (progressListener != null)
            {
                input = new ProgressReportingInputStream(input, progressListener);
                FireProgressEvent(progressListener, ProgressEvent.STARTED);
            }

            PopulateRequestMetadata(metadata, request);
            request.Content = (input);

            //-----------------------------------------------

            ObjectMetadata returnedMetadata = null;
            try
            {
                returnedMetadata = Invoke(request, new MetadataResponseHandler(), bucketName, key);
            }
            catch (ProgressInterruptedException e)
            {
                FireProgressEvent(progressListener, ProgressEvent.CANCELED);
                throw e;
            }
            catch (Exception e)
            {
                FireProgressEvent(progressListener, ProgressEvent.FAILED);
                throw e;
            }
            finally
            {
                input?.Close();
                input?.Dispose();
            }

            FireProgressEvent(progressListener, ProgressEvent.COMPLETED);

            var result = new PutObjectResult()
            {
                ETag = returnedMetadata.GetETag(),
                ContentMD5 = metadata.GetContentMD5()
            };

            return result;
        }
        /// <summary>
        /// This implementation of the PUT operation creates a copy of an object that is already stored in S3. A PUT copy operation is the same as performing a GET and then a PUT. Adding the request header, x-amz-copy-source, makes the PUT operation copy the source object into the destination bucket.
        /// </summary>
        /// <param name="copyObjectRequest"></param>
        /// <returns></returns>
        public CopyObjectResult copyObject(CopyObjectRequest copyObjectRequest)
        {
            IRequest<CopyObjectRequest> request = this.CreateRequest(copyObjectRequest.DestinationBucket, copyObjectRequest.DestinationObject, copyObjectRequest, HttpMethod.PUT);
            request.SetHeader(Headers.XKssCopySource, "/" + copyObjectRequest.SourceBucket + "/" + UrlEncoder.Encode(copyObjectRequest.SourceObject, Encoding.UTF8));
            if (copyObjectRequest.AccessControlList != null)
                addAclHeaders(request, copyObjectRequest.AccessControlList);
            else if (copyObjectRequest.CannedAcl != null)
                request.SetHeader(Headers.KS3_CANNED_ACL, copyObjectRequest.CannedAcl.CannedAclHeader);
            request.SetHeader(Headers.CONTENT_LENGTH, "0");
            return this.Invoke(request, new CopyObjectResultUnmarshaller(), copyObjectRequest.DestinationBucket, copyObjectRequest.DestinationObject);
        }
        /// <summary>
        /// The HEAD operation retrieves metadata from an object without returning the object itself. This operation is useful if you are interested only in an object's metadata. To use HEAD, you must have READ access to the object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <returns></returns>
        public HeadObjectResult headObject(String bucketName, String objectKey)
        {
            return headObject(bucketName, objectKey);
        }
        public HeadObjectResult headObject(HeadObjectRequest headObjectRequest)
        {
            IRequest<HeadObjectRequest> request = this.CreateRequest(headObjectRequest.BucketName, headObjectRequest.ObjectKey, headObjectRequest, HttpMethod.HEAD);
            headObjectRequest.Validate();
            if (headObjectRequest.MatchingETagConstraints.Count > 0)
            {
                StringBuilder Etags = new StringBuilder();
                foreach (String Etag in headObjectRequest.MatchingETagConstraints)
                {
                    Etags.Append(Etag);
                    Etags.Append(",");
                }
                request.SetHeader(Headers.GET_OBJECT_IF_MATCH, Etags.ToString().TrimEnd(','));
            }
            if (headObjectRequest.NonmatchingEtagConstraints.Count > 0)
            {
                StringBuilder noEtags = new StringBuilder();
                foreach (String etag in headObjectRequest.NonmatchingEtagConstraints)
                {
                    noEtags.Append(etag);
                    noEtags.Append(",");
                }
                request.SetHeader(Headers.GET_OBJECT_IF_NONE_MATCH, noEtags.ToString().TrimEnd(','));
            }
            if (!headObjectRequest.ModifiedSinceConstraint.Equals(DateTime.MinValue))
            {
                request.SetHeader(Headers.GET_OBJECT_IF_MODIFIED_SINCE, headObjectRequest.ModifiedSinceConstraint.ToUniversalTime().ToString("r"));
            }
            if (!headObjectRequest.UnmodifiedSinceConstraint.Equals(DateTime.MinValue))
            {
                request.SetHeader(Headers.GET_OBJECT_IF_UNMODIFIED_SINCE, headObjectRequest.UnmodifiedSinceConstraint.ToUniversalTime().ToString("r"));
            }

            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.CacheControl))
                request.SetParameter("response-cache-control", headObjectRequest.Overrides.CacheControl);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentType))
                request.SetParameter("&response-content-type", headObjectRequest.Overrides.ContentType);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentLanguage))
                request.SetParameter("&response-content-language", headObjectRequest.Overrides.ContentLanguage);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.Expires))
                request.SetParameter("&response-expires", headObjectRequest.Overrides.Expires);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentDisposition))
                request.SetParameter("&response-content-disposition", headObjectRequest.Overrides.ContentDisposition);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentEncoding))
                request.SetParameter("&response-content-encoding", headObjectRequest.Overrides.ContentEncoding);

            return this.Invoke(request, new HeadObjectResultHandler(), headObjectRequest.BucketName, headObjectRequest.ObjectKey);
        }

        /// <summary>
        /// init multi upload big file
        /// </summary>
        /// <param name="bucketname"></param>
        /// <param name="objectkey"></param>
        /// <returns></returns>
        public InitiateMultipartUploadResult InitiateMultipartUpload(string bucketname, string objectkey)
        {
            return InitiateMultipartUpload(new InitiateMultipartUploadRequest(bucketname, objectkey));
        }

        public InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest param)
        {
            IRequest<InitiateMultipartUploadRequest> request = CreateRequest(param.BucketName, param.ObjectKey, param, HttpMethod.POST);
            request.SetParameter("uploads", null);
            request.SetHeader(Headers.CONTENT_LENGTH, "0");
            var result = Invoke(request, new MultipartUploadResultUnmarshaller(), param.BucketName, param.ObjectKey);
            return result;
        }

        /// <summary>
        /// upload multi file by part
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PartETag UploadPart(UploadPartRequest param)
        {
            var bucketName = param.BucketName;
            var key = param.ObjectKey;
            var metadata = param.Metadata;
            var input = param.InputStream;
            var progressListener = param.ProgressListener;

            if (metadata == null)
            {
                metadata = new ObjectMetadata();
            }

            // If a file is specified for upload, we need to pull some additional
            // information from it to auto-configure a few options
            metadata.SetContentLength(input.Length);

            if (metadata.GetContentType() == null)
            {
                metadata.SetContentType(Mimetypes.DEFAULT_MIMETYPE);
            }
            if (metadata.GetContentMD5() == null)
            {
                using (MD5 md5 = MD5.Create())
                {
                    metadata.SetContentMD5(Convert.ToBase64String(md5.ComputeHash(input)));
                }

                input.Seek(0, SeekOrigin.Begin); // It is needed after calculated MD5.
            }

            IRequest<UploadPartRequest> request = CreateRequest(
                param.BucketName,
                param.ObjectKey,
                param, HttpMethod.PUT);
            request.SetParameter("partNumber", param.PartNumber.ToString());
            request.SetParameter("uploadId", param.UploadId);

            if (progressListener != null)
            {
                input = new ProgressReportingInputStream(input, progressListener);
                FireProgressEvent(progressListener, ProgressEvent.STARTED);
            }

            PopulateRequestMetadata(metadata, request);
            request.Content = (input);

            //-----------------------------------------------

            ObjectMetadata returnedMetadata = null;
            try
            {
                returnedMetadata = this.Invoke(request, new MetadataResponseHandler(), bucketName, key);
            }
            catch (ProgressInterruptedException e)
            {
                FireProgressEvent(progressListener, ProgressEvent.CANCELED);
                throw e;
            }
            catch (Exception e)
            {
                FireProgressEvent(progressListener, ProgressEvent.FAILED);
                throw e;
            }
            finally
            {
                if (input != null)
                    input.Close();
            }

            FireProgressEvent(progressListener, ProgressEvent.COMPLETED);

            var result = new PartETag()
            {
                PartNumber = param.PartNumber,
                ETag = returnedMetadata.GetETag()
            };
            return result;
        }


        /// <summary>
        /// getlist had uploaded part list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ListMultipartUploadsResult GetListMultipartUploads(ListMultipartUploadsRequest param)
        {
            IRequest<ListMultipartUploadsRequest> request = CreateRequest(param.BucketName, param.ObjectKey, param, HttpMethod.GET);
            request.SetParameter("uploadId", param.UploadId);
            request.SetHeader(Headers.CONTENT_LENGTH, "0");
            var result = Invoke(
                request,
                new ListMultipartUploadsResultUnmarshaller(),
                param.BucketName,
                param.ObjectKey);

            return result;
        }


        /// <summary>
        /// submit the all part,the server will complete join part
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public CompleteMultipartUploadResult CompleteMultipartUpload(CompleteMultipartUploadRequest param)
        {
            IRequest<CompleteMultipartUploadRequest> request = CreateRequest(
                param.BucketName,
                param.ObjectKey,
                param,
                HttpMethod.POST);

            request.SetParameter("uploadId", param.UploadId);
            request.SetHeader(Headers.CONTENT_LENGTH, param.Content.Length.ToString());
            request.Content = param.Content;

            var result = Invoke(
                request,
                new CompleteMultipartUploadResultUnmarshaller(),
                param.BucketName,
                param.ObjectKey);

            return result;
        }

        /// <summary>
        /// Abort the upload opertion by uploadid
        /// </summary>
        /// <param name="param"></param>
        public void AbortMultipartUpload(AbortMultipartUploadRequest param)
        {
            IRequest<AbortMultipartUploadRequest> request = this.CreateRequest(param.BucketName, param.ObjectKey, param, HttpMethod.DELETE);
            request.SetParameter("uploadId", param.UploadId);
            request.SetHeader(Headers.CONTENT_LENGTH, "0");
            this.Invoke(request, _voidResponseHandler, param.BucketName, param.ObjectKey);
        }

        /**
         * Gets the AccessControlList (ACL) for the specified object in KS3.
         */
        public AccessControlList GetObjectAcl(String bucketName, String key)
        {
            return this.GetObjectAcl(new GetObjectAclRequest(bucketName, key));
        }

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified object in KS3.
        /// </summary>
        /// <param name="getObjectAclRequest"></param>
        /// <returns></returns>
        public AccessControlList GetObjectAcl(GetObjectAclRequest getObjectAclRequest)
        {
            var bucketName = getObjectAclRequest.BucketName;
            var key = getObjectAclRequest.Key;

            IRequest<GetObjectAclRequest> request = CreateRequest(
                bucketName,
                key,
                getObjectAclRequest,
                HttpMethod.GET);

            request.SetParameter("acl", null);

            return Invoke(request, new AccessControlListUnmarshaller(), bucketName, key);
        }

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="acl"></param>
        public void SetObjectAcl(string bucketName, string key, AccessControlList acl)
        {
            SetObjectAcl(new SetObjectAclRequest(bucketName, key, acl));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="cannedAcl"></param>
        public void SetObjectAcl(string bucketName, string key, CannedAccessControlList cannedAcl)
        {
            SetObjectAcl(new SetObjectAclRequest(bucketName, key, cannedAcl));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="setObjectAclRequest"></param>
        public void SetObjectAcl(SetObjectAclRequest setObjectAclRequest)
        {
            var bucketName = setObjectAclRequest.BucketName;
            var key = setObjectAclRequest.Key;
            var acl = setObjectAclRequest.Acl;
            var cannedAcl = setObjectAclRequest.CannedAcl;

            IRequest<SetObjectAclRequest> request = this.CreateRequest(bucketName, key, setObjectAclRequest, HttpMethod.PUT);

            if (acl != null)
            {
                string xml = AclXmlFactory.ConvertToXmlString(acl);
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

                request.Content = (memoryStream);
                request.SetHeader(Headers.CONTENT_LENGTH, memoryStream.Length.ToString());
            }
            else if (cannedAcl != null)
            {
                request.SetHeader(Headers.KS3_CANNED_ACL, cannedAcl.CannedAclHeader);
                request.SetHeader(Headers.CONTENT_LENGTH, "0");
            }
            request.SetParameter("acl", null);

            Invoke(request, this._voidResponseHandler, bucketName, key);
        }

        /// <summary>
        /// generate presignerd url for private object with in limit times
        /// </summary>
        /// <param name="bucketName">bucketname</param>
        /// <param name="key">objectkey</param>
        /// <param name="expiration">expire time</param>
        /// <returns>url</returns>
        public string GeneratePresignedUrl(string bucketName, string key, DateTime expiration)
        {
            return GeneratePresignedUrl(bucketName, key, expiration, null);
        }

        /// <summary>
        /// generate PresignedUrl the url can apply for other user
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public string GeneratePresignedUrl(string bucketName, string key, DateTime expiration, ResponseHeaderOverrides overrides)
        {
            string url = "";
            string param = "";

            overrides = overrides == null ? new ResponseHeaderOverrides() : overrides;
            if (!string.IsNullOrEmpty(overrides.CacheControl))
                param += "response-cache-control=" + overrides.CacheControl;
            if (!string.IsNullOrEmpty(overrides.ContentType))
                param += "&response-content-type=" + overrides.ContentType;
            if (!string.IsNullOrEmpty(overrides.ContentLanguage))
                param += "&response-content-language=" + overrides.ContentLanguage;
            if (!string.IsNullOrEmpty(overrides.Expires))
                param += "&response-expires=" + overrides.Expires;
            if (!string.IsNullOrEmpty(overrides.ContentDisposition))
                param += "&response-content-disposition=" + overrides.ContentDisposition;
            if (!string.IsNullOrEmpty(overrides.ContentEncoding))
                param += "&response-content-encoding=" + overrides.ContentEncoding;

            var baselineTime = new DateTime(1970, 1, 1);
            var expires = Convert.ToInt64((expiration.ToUniversalTime() - baselineTime).TotalSeconds);
            try
            {
                KS3Signer<NoneKS3Request> ks3Signer = CreateSigner<NoneKS3Request>(HttpMethod.GET.ToString(), bucketName, key);
                string signer = ks3Signer.GetSignature(this._ks3Credentials, expires.ToString());
                url += @"http://" + bucketName + "." + Constants.KS3_HOSTNAME
                             + "/" + filterSpecial(UrlEncoder.Encode(key, Constants.DEFAULT_ENCODING)) + "?AccessKeyId="
                             + UrlEncoder.Encode(this._ks3Credentials.GetKS3AccessKeyId(), Constants.DEFAULT_ENCODING)
                             + "&Expires=" + expires + "&Signature="
                             + UrlEncoder.Encode(signer, Constants.DEFAULT_ENCODING) + "&" + param;

            }
            catch (Exception e)
            {
                throw e;
            }

            return url;
        }
        /// <summary>
        /// add Asynchronous Data Processing 可以通过adp执行图片缩略图处理、执行转码操作等
        /// </summary>
        /// <param name="putAdpRequest"></param>
        /// <returns></returns>
        public string PutAdpTask(PutAdpRequest putAdpRequest)
        {
            IRequest<PutAdpRequest> request = this.CreateRequest(putAdpRequest.BucketName, putAdpRequest.ObjectKey, putAdpRequest, HttpMethod.PUT);
            request.SetParameter("adp", null);

            request.SetHeader(Headers.AsynchronousProcessingList, putAdpRequest.ConvertAdpsToString());
            request.SetHeader(Headers.NotifyURL, putAdpRequest.NotifyURL);
            request.SetHeader(Headers.CONTENT_LENGTH, "0");
            PutAdpResult result = this.Invoke(request, new PutAdpResponseHandler(), putAdpRequest.BucketName, putAdpRequest.ObjectKey);
            return result.TaskId;
        }
        public GetAdpResult getAdpTask(GetAdpRequest getAdpRequest)
        {
            IRequest<GetAdpRequest> request = this.CreateRequest(getAdpRequest.TaskId, null, getAdpRequest, HttpMethod.GET);
            request.SetParameter("queryadp", string.Empty);
            return this.Invoke(request, new GetAdpResultUnmarshaller(), null, null);
        }
        ////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// * Creates and initializes a new request object for the specified KS3 resource.
        /// * Three parameters needed to be set
        /// * 1. http method(GET, PUT, HEAD or DELETE)
        /// * 2. endpoint(http or https, and the host name. e.g.http://kss.ksyun.com)
        /// * 3. resource path (bucketName/[key], e.g.my-bucket/my-object)
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="originalRequest"></param>
        /// <param name="httpMethod"></param>
        /// <returns></returns>
        private IRequest<X> CreateRequest<X>(
            string bucketName,
            string key,
            X originalRequest,
            HttpMethod httpMethod) where X : KS3Request
        {
            IRequest<X> request = new DefaultRequest<X>(originalRequest)
            {
                HttpMethod = (httpMethod),
                Endpoint = (_endpoint)
            };
            var bucketEncode = UrlEncoder.Encode(bucketName != null ? bucketName : "", Constants.DEFAULT_ENCODING);
            var keyEncode = UrlEncoder.Encode(key != null ? key : "", Constants.DEFAULT_ENCODING);
            var resourcePath = "/" + (bucketEncode != null ? bucketEncode + "/" : "") + (keyEncode != null ? keyEncode : "");
            resourcePath = filterSpecial(resourcePath);

            request.ResourcePath = (resourcePath);

            return request;
        }

        private X Invoke<X, Y>(
            IRequest<Y> request,
            IUnmarshaller<X, Stream> unmarshaller,
            string bucketName,
            string key) where Y : KS3Request
        {
            return Invoke(request, new XmlResponseHandler<X>(unmarshaller), bucketName, key);
        }

        /// <summary>
        /// * Before the KS3HttpClient deal with the request, we want the request looked like a collection of that:
        /// * 1. http method
        /// * 2. endpoint
        /// * 3. resource path
        /// * 4. headers
        /// * 5. parameters
        /// * 6. content
        /// * 7. time offset
        /// * The first three points are done in "createRequest".
        /// * The content was set before "createRequest" when we need to put a object to server.And some metadata like Content-Type, Content-Length, etc.
        ///  * So at here, we need to complete 4, 5, and 7.
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <typeparam name="Y"></typeparam>
        /// <param name="request"></param>
        /// <param name="responseHandler"></param>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private X Invoke<X, Y>(
            IRequest<Y> request,
            IHttpResponseHandler<X> responseHandler,
            string bucket,
            string key) where Y : KS3Request
        {
            IDictionary<string, string> parameters = request.OriginalRequest.CopyPrivateRequestParameters();
            foreach (var name in parameters.Keys)
            {
                request.SetParameter(name, parameters[name]);
            }
            request.TimeOffset = _timeOffset;

            //The string we sign needs to include the exact headers that we send with the request, but the client runtime layer adds the Content-Type header before the request is sent if one isn't set, so we have to set something here otherwise the request will fail.
            if (!request.Headers.ContainsKey(Headers.CONTENT_TYPE))
            {
                request.SetHeader(Headers.CONTENT_TYPE, Mimetypes.DEFAULT_MIMETYPE);
            }

            //Set the credentials which will be used by the KS3Signer later.
            if (request.OriginalRequest.Credentials == null)
            {
                request.OriginalRequest.Credentials = (_ks3Credentials);
            }
            return _client.Excute(request, responseHandler, CreateSigner(request, bucket, key));
        }

        private KS3Signer<T> CreateSigner<T>(IRequest<T> request, String bucketName, String key) where T : KS3Request
        {
            return CreateSigner<T>(request.HttpMethod.ToString(), bucketName, key);
        }
        private KS3Signer<T> CreateSigner<T>(string httpMethod, string bucketName, string key) where T : KS3Request
        {
            string bucketEncode = UrlEncoder.Encode(string.IsNullOrWhiteSpace(bucketName) ? "" : bucketName, Constants.DEFAULT_ENCODING);
            string keyEncode = UrlEncoder.Encode(string.IsNullOrWhiteSpace(key) ? "" : key, Constants.DEFAULT_ENCODING);

            string resourcePath = "/" + (string.IsNullOrWhiteSpace(bucketEncode) ? "" : $"{bucketEncode}/") + (string.IsNullOrWhiteSpace(keyEncode) ? "" : keyEncode);
            resourcePath = filterSpecial(resourcePath);
            return new KS3Signer<T>(httpMethod, resourcePath);
        }

        /// <summary>
        /// Fires a progress event with the specified event type to the specified listener.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="eventType"></param>
        private static void FireProgressEvent(IProgressListener listener, int eventType)
        {
            if (listener == null)
            {
                return;
            }

            ProgressEvent e = new ProgressEvent(eventType);
            listener.ProgressChanged(e);
        }

        /// <summary>
        /// Populates the specified request object with the appropriate headers from the {@link ObjectMetadata} object.
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        private static void PopulateRequestMetadata<X>(ObjectMetadata metadata, IRequest<X> request) where X : KS3Request
        {
            IDictionary<string, object> rawMetadata = metadata.GetRawMetadata();
            if (rawMetadata != null)
            {
                foreach (var name in rawMetadata.Keys)
                {
                    request.SetHeader(name, rawMetadata[name].ToString());
                }
            }

            IDictionary<String, String> userMetadata = metadata.GetUserMetadata();
            if (userMetadata != null)
            {
                foreach (String name in userMetadata.Keys)
                {
                    request.SetHeader(Headers.KS3_USER_METADATA_PREFIX + name, userMetadata[name]);
                }
            }
        }

        /// <summary>
        /// Adds the specified date header in RFC 822 date format to the specified request. This method will not add a date header if the specified date value is <code>null</code>.
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="request"></param>
        /// <param name="header"></param>
        /// <param name="value"></param>
        private static void AddDateHeader<X>(IRequest<X> request, string header, DateTime? value)
        {
            if (value.HasValue)
            {
                request.SetHeader(header, SignerUtils.GetSignatrueDate(value.Value));
            }
        }

        /*
         * Adds the specified string list header, joined together separated with
         * commas, to the specified request.
         * This method will not add a string list header if the specified values
         * are <code>null</code> or empty.
         */
        private static void AddStringListHeader<X>(IRequest<X> request, String header, IList<String> values)
        {
            if (values != null && values.Count > 0)
                request.SetHeader(header, String.Join(", ", values));
        }
        private static string filterSpecial(string key)
        {
            if (!String.IsNullOrEmpty(key))
            {
                key = key.Replace("%5C", "/").Replace("//", "/%2F").Replace("%28", "(").Replace("%29", ")");
            }
            return key;
        }

        /// <summary>
        /// Sets the acccess control headers for the request given.
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <param name="request"></param>
        /// <param name="acl"></param>
        private static void addAclHeaders<X>(IRequest<X> request, AccessControlList acl) where X : KS3Request
        {
            var grants = acl.Grants;
            IDictionary<String, IList<IGrantee>> grantsByPermission = new Dictionary<String, IList<IGrantee>>();
            foreach (var grant in grants)
            {
                if (!grantsByPermission.ContainsKey(grant.Permission))
                {
                    grantsByPermission[grant.Permission] = new List<IGrantee>();
                }
                grantsByPermission[grant.Permission].Add(grant.Grantee);
            }
            foreach (String permission in Permission.ListPermissions())
            {
                if (grantsByPermission.ContainsKey(permission))
                {
                    IList<IGrantee> grantees = grantsByPermission[permission];
                    bool first = true;
                    StringBuilder granteeString = new StringBuilder();
                    foreach (IGrantee grantee in grantees)
                    {
                        if (first)
                            first = false;
                        else
                            granteeString.Append(", ");
                        granteeString.Append(grantee.GetTypeIdentifier() + "=\"" + grantee.GetIdentifier() + "\"");
                    }
                    request.SetHeader(Permission.GetHeaderName(permission), granteeString.ToString());
                }
            }
        }
    }
}