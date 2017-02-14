using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

using KS3.Model;
using KS3.Auth;
using KS3.Http;
using KS3.Internal;
using KS3.Transform;
using KS3.KS3Exception;


namespace KS3
{
    public class KS3Client : KS3
    {
        private XmlResponseHandler<Type> voidResponseHandler = new XmlResponseHandler<Type>(null);

        /** KS3 credentials. */
        private KS3Credentials ks3Credentials;

        /** The service endpoint to which this client will send requests. */
        private Uri endpoint;

        /** The client configuration */
        private ClientConfiguration clientConfiguration;

        /** Low level client for sending requests to KS3. */
        private KS3HttpClient client;

        /** Optional offset (in seconds) to use when signing requests */
        private int timeOffset;

        /**
         * Constructs a new KS3Client object using the specified Access Key ID and Secret Key.
         */
        public KS3Client(String accessKey, String secretKey)
            : this(new BasicKS3Credentials(accessKey, secretKey)) { }

        /**
         * Constructs a new KS3Client object using the specified configuration.
         */
        public KS3Client(KS3Credentials ks3Credentials)
            : this(ks3Credentials, new ClientConfiguration()) { }

        /**
         * Constructs a new KS3Client object using the specified Access Key ID, Secret Key and configuration.
         */
        public KS3Client(String accessKey, String secretKey, ClientConfiguration clientConfiguration)
            : this(new BasicKS3Credentials(accessKey, secretKey), clientConfiguration) { }

        /**
         * Constructs a new KS3Client object using the specified credential and configuration.
         */
        public KS3Client(KS3Credentials ks3Credentials, ClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
            this.client = new KS3HttpClient(clientConfiguration);
            this.ks3Credentials = ks3Credentials;

            this.init();
        }

        private void init()
        {
            this.setEndpoint(Constants.KS3_HOSTNAME);
        }

        public void setEndpoint(String endpoint)
        {
            if (!endpoint.Contains("://"))
                endpoint = clientConfiguration.getProtocol() + "://" + endpoint;
            this.endpoint = new Uri(endpoint);
        }

        public void setConfiguration(ClientConfiguration clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
            client = new KS3HttpClient(clientConfiguration);
        }

        /**
         * Sets the optional value for time offset for this client.  This
         * value will be applied to all requests processed through this client.
         * Value is in seconds, positive values imply the current clock is "fast",
         * negative values imply clock is slow.
         */
        public void setTimeOffset(int timeOffset)
        {
            this.timeOffset = timeOffset;
        }

        /**
         * Returns the optional value for time offset for this client.  This
         * value will be applied to all requests processed through this client.
         * Value is in seconds, positive values imply the current clock is "fast",
         * negative values imply clock is slow.
         */
        public int getTimeOffset()
        {
            return this.timeOffset;
        }

        /**
         * Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
         */
        public IList<Bucket> listBuckets()
        {
            return this.listBuckets(new ListBucketsRequest());
        }

        /**
         * Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
         */
        public IList<Bucket> listBuckets(ListBucketsRequest listBucketsRequest)
        {
            Request<ListBucketsRequest> request = this.createRequest(null, null, listBucketsRequest, HttpMethod.GET);
            return this.invoke(request, new ListBucketsUnmarshaller(), null, null);
        }

        /**
         * Deletes the specified bucket. 
         */
        public void deleteBucket(String bucketName)
        {
            this.deleteBucket(new DeleteBucketRequest(bucketName));
        }

        /**
         * Deletes the specified bucket. 
         */
        public void deleteBucket(DeleteBucketRequest deleteBucketRequest)
        {
            String bucketName = deleteBucketRequest.getBucketName();

            Request<DeleteBucketRequest> request = this.createRequest(bucketName, null, deleteBucketRequest, HttpMethod.DELETE);
            this.invoke(request, voidResponseHandler, bucketName, null);
        }

        /**
         * Gets the AccessControlList (ACL) for the specified KS3 bucket.
         */
        public AccessControlList getBucketAcl(String bucketName)
        {
            return this.getBucketAcl(new GetBucketAclRequest(bucketName));
        }

        /**
         * Gets the AccessControlList (ACL) for the specified KS3 bucket.
         */
        public AccessControlList getBucketAcl(GetBucketAclRequest getBucketAclRequest)
        {
            String bucketName = getBucketAclRequest.getBucketName();

            Request<GetBucketAclRequest> request = createRequest(bucketName, null, getBucketAclRequest, HttpMethod.GET);
            request.setParameter("acl", null);

            return this.invoke(request, new AccessControlListUnmarshaller(), bucketName, null);
        }

        /**
         * Creates a new KS3 bucket. 
         */
        public Bucket createBucket(String bucketName)
        {
            return this.createBucket(new CreateBucketRequest(bucketName));
        }

        /**
         * Creates a new KS3 bucket. 
         */
        public Bucket createBucket(CreateBucketRequest createBucketRequest)
        {
            String bucketName = createBucketRequest.getBucketName();

            Request<CreateBucketRequest> request = this.createRequest(bucketName, null, createBucketRequest, HttpMethod.PUT);
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";
            
            if (createBucketRequest.getAcl() != null)
                addAclHeaders(request, createBucketRequest.getAcl());
            else if (createBucketRequest.getCannedAcl() != null)
                request.setHeader(Headers.KS3_CANNED_ACL, createBucketRequest.getCannedAcl().getCannedAclHeader());

            this.invoke(request, voidResponseHandler, bucketName, null);

            return new Bucket(bucketName);
        }
        /// <summary>
        /// This operation is useful to determine if a bucket exists and you have permission to access it
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public HeadBucketResult headBucket(String bucketName) {
            return headBucket(new HeadBucketRequest(bucketName));
        }
        public HeadBucketResult headBucket(HeadBucketRequest headBucketRequest) {
            String bucketname = headBucketRequest.BucketName;
            Request<HeadBucketRequest> request = this.createRequest(bucketname, null, headBucketRequest, HttpMethod.HEAD);
            return this.invoke(request, new HeadBucketResponseHandler(), bucketname, null);
        }
        /// <summary>
        /// Returns the cors configuration information set for the bucket.
        /// To use this operation, you must have permission to perform the s3:GetBucketCORS action. By default, the bucket owner has this permission and can grant it to others.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public BucketCorsConfigurationResult getBucketCors(String bucketName) {
            return this.getBucketCors(new GetBucketCorsRequest(bucketName));
        }
        public BucketCorsConfigurationResult getBucketCors(GetBucketCorsRequest getBucketCorsRequest)
        {
            BucketCorsConfigurationResult result = new BucketCorsConfigurationResult();
            String bucketname = getBucketCorsRequest.BucketName;
            Request<GetBucketCorsRequest> request = this.createRequest(bucketname, null, getBucketCorsRequest, HttpMethod.GET);
            request.getParameters().Add("cors", null);
            result = this.invoke(request, new BucketCorsConfigurationResultUnmarshaller(), bucketname, null);
            return result;
        }
        /// <summary>
        /// This implementation of the GET operation uses the location subresource to return a bucket's region. You set the bucket's region using the LocationConstraint request parameter in a PUT Bucket request. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public GetBucketLocationResult getBucketLocation(String bucketName)
        {
            return getBucketLocation(new GetBucketLocationRequest(bucketName));
        }
        public GetBucketLocationResult getBucketLocation(GetBucketLocationRequest getBucketLocationRequest)
        {
            GetBucketLocationResult result = new GetBucketLocationResult();
            Request<GetBucketLocationRequest> request = this.createRequest(getBucketLocationRequest.BucketName, null, getBucketLocationRequest, HttpMethod.GET);
            request.getParameters().Add("location", null);
            result=this.invoke(request, new GetBucketLocationResultUnmarshaller(), getBucketLocationRequest.BucketName, null);
            return result;
        }
        /// <summary>
        /// This implementation of the GET operation uses the logging subresource to return the logging status of a bucket and the permissions users have to view and modify that status. To use GET, you must be the bucket owner. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public GetBucketLoggingResult getBucketLogging(String bucketName)
        {
            return getBucketLogging(new GetBucketLoggingRequest(bucketName));
        }
        public GetBucketLoggingResult getBucketLogging(GetBucketLoggingRequest getBucketLoggingRequest)
        {
            GetBucketLoggingResult result = new GetBucketLoggingResult();
            Request<GetBucketLoggingRequest> request = this.createRequest(getBucketLoggingRequest.BucketName, null, getBucketLoggingRequest, HttpMethod.GET);
            request.getParameters().Add("logging",null);
            result=this.invoke(request, new GetBucketLoggingResultUnmarshaller(), getBucketLoggingRequest.BucketName, null);
            return result;
        }

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        public void setBucketAcl(String bucketName, AccessControlList acl)
        {
            this.setBucketAcl(new SetBucketAclRequest(bucketName, acl));
        }

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        public void setBucketAcl(String bucketName, CannedAccessControlList cannedAcl)
        {
            this.setBucketAcl(new SetBucketAclRequest(bucketName, cannedAcl));
        }

        /**
         * Sets the AccessControlList for the specified KS3 bucket.
         */
        public void setBucketAcl(SetBucketAclRequest setBucketAclRequest)
        {
            String bucketName = setBucketAclRequest.getBucketName();
            AccessControlList acl = setBucketAclRequest.getAcl();
            CannedAccessControlList cannedAcl = setBucketAclRequest.getCannedAcl();

            Request<SetBucketAclRequest> request = this.createRequest(bucketName, null, setBucketAclRequest, HttpMethod.PUT);
            
            if (acl != null)
            {
                String xml = AclXmlFactory.convertToXmlString(acl);
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                
                request.setContent(memoryStream);
                request.setHeader(Headers.CONTENT_LENGTH, memoryStream.Length.ToString());
            }
            else if (cannedAcl != null)
            {
                request.setHeader(Headers.KS3_CANNED_ACL, cannedAcl.getCannedAclHeader());
                request.setHeader(Headers.CONTENT_LENGTH, "0");
            }

            request.setParameter("acl", null);

            this.invoke(request, this.voidResponseHandler, bucketName, null);
        }
        /// <summary>
        /// Sets the cors configuration for your bucket. If the configuration exists, Amazon S3 replaces it. 
        /// </summary>
        /// <param name="putBucketCorsRequest"></param>
        public void setBucketCors(PutBucketCorsRequest putBucketCorsRequest)
        {
            Request<PutBucketCorsRequest> request = this.createRequest(putBucketCorsRequest.BucketName, null, putBucketCorsRequest, HttpMethod.PUT);
            request.getParameters().Add("cors", null);
            request.setHeader(Headers.CONTENT_LENGTH, putBucketCorsRequest.toXmlAdapter().Length.ToString());
            request.setHeader(Headers.CONTENT_TYPE, "application/xml");
            request.setHeader(Headers.CONTENT_MD5,putBucketCorsRequest.getMd5());
            request.setContent(putBucketCorsRequest.toXmlAdapter());
            this.invoke(request, this.voidResponseHandler, putBucketCorsRequest.BucketName, null);
        }
        /// <summary>
        /// This implementation of the PUT operation uses the logging subresource to set the logging parameters for a bucket and to specify permissions for who can view and modify the logging parameters. To set the logging status of a bucket, you must be the bucket owner.
        /// </summary>
        /// <param name="putBucketLoggingRequest"></param>
        public void setBucketLogging(PutBucketLoggingRequest putBucketLoggingRequest)
        {
            Request<PutBucketLoggingRequest> request = this.createRequest(putBucketLoggingRequest.BucketName, null, putBucketLoggingRequest, HttpMethod.PUT);
            request.getParameters().Add("logging",null);
            request.setHeader(Headers.CONTENT_LENGTH, putBucketLoggingRequest.toXmlAdapter().Length.ToString());
            request.setHeader(Headers.CONTENT_TYPE, "application/xml");
            request.setContent(putBucketLoggingRequest.toXmlAdapter());
            this.invoke(request, this.voidResponseHandler, putBucketLoggingRequest.BucketName, null);
        }
        /// <summary>
        /// Deletes the cors configuration information set for the bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        public void deleteBucketCors(String bucketName) {
            deleteBucketCors(new DeleteBucketCorsRequest(bucketName));
        }
        public void deleteBucketCors(DeleteBucketCorsRequest deleteBucketCorsRequest)
        {
            Request<DeleteBucketCorsRequest> request = this.createRequest(deleteBucketCorsRequest.BucketName, null, deleteBucketCorsRequest, HttpMethod.DELETE);
            request.getParameters().Add("cors",null);
            this.invoke(request, this.voidResponseHandler, deleteBucketCorsRequest.BucketName, null);
        }
        /// <summary>
        /// The Multi-Object Delete operation enables you to delete multiple objects from a bucket using a single HTTP request.
        /// </summary>
        /// <param name="deleteMultipleObjectsRequest"></param>
        /// <returns></returns>
        public DeleteMultipleObjectsResult deleteMultiObjects(DeleteMultipleObjectsRequest deleteMultipleObjectsRequest)
        {
            Request<DeleteMultipleObjectsRequest> request = this.createRequest(deleteMultipleObjectsRequest.BucketName, null, deleteMultipleObjectsRequest, HttpMethod.POST);
            request.getParameters().Add("delete",null);
            request.setHeader(Headers.CONTENT_LENGTH, deleteMultipleObjectsRequest.toXmlAdapter().Length.ToString());
            request.setHeader(Headers.CONTENT_TYPE, "application/xml");
            request.setHeader(Headers.CONTENT_MD5, deleteMultipleObjectsRequest.getMd5());
            request.setContent(deleteMultipleObjectsRequest.toXmlAdapter());
            return this.invoke(request, new DeleteMultipleObjectsResultUnmarshaller(), deleteMultipleObjectsRequest.BucketName, null);
        }
        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        public ObjectListing listObjects(String bucketName)
        {
            return this.listObjects(new ListObjectsRequest(bucketName, null, null, null, null));
        }

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        public ObjectListing listObjects(String bucketName, String prefix)
        {
            return this.listObjects(new ListObjectsRequest(bucketName, prefix, null, null, null));
        }

        /**
         * Returns a list of summary information about the objects in the specified bucket.
         */
        public ObjectListing listObjects(ListObjectsRequest listObjectRequest)
        {
            String bucketName = listObjectRequest.getBucketName();
            Request<ListObjectsRequest> request = this.createRequest(bucketName, null, listObjectRequest, HttpMethod.GET);

            if (listObjectRequest.getPrefix() != null)
                request.setParameter("prefix", listObjectRequest.getPrefix());
            if (listObjectRequest.getMarker() != null)
                request.setParameter("marker", listObjectRequest.getMarker());
            if (listObjectRequest.getDelimiter() != null)
                request.setParameter("delimiter", listObjectRequest.getDelimiter());
            if (listObjectRequest.getMaxKeys() != null && listObjectRequest.getMaxKeys() >= 0)
                request.setParameter("max-keys", listObjectRequest.getMaxKeys().ToString());

            return this.invoke(request, new ListObjectsUnmarshallers(), bucketName, null);
        }

        /**
         * Deletes the specified object in the specified bucket.
         */
        public void deleteObject(String bucketName, String key)
        {
            this.deleteObject(new DeleteObjectRequest(bucketName, key));
        }

        /**
         * Deletes the specified object in the specified bucket.
         */
        public void deleteObject(DeleteObjectRequest deleteObjectRequest)
        { 
            String bucketName = deleteObjectRequest.getBucketName();
            String key = deleteObjectRequest.getKey();
            Request<DeleteObjectRequest> request = this.createRequest(bucketName, key, deleteObjectRequest, HttpMethod.DELETE);

            this.invoke(request, voidResponseHandler, bucketName, key);
        }

        /**
         * Gets the object stored in KS3 under the specified bucket and key.
         */
        public KS3Object getObject(String bucketName, String key)
        {
            return this.getObject(new GetObjectRequest(bucketName, key));
        }

        /**
         * Gets the object stored in KS3 under the specified bucket and key, and saves the object contents to the specified file.
         */
        public KS3Object getObject(String bucketName, String key, FileInfo destinationFile)
        {
            return this.getObject(new GetObjectRequest(bucketName, key, destinationFile));
        }

        /**
         * Gets the object stored in KS3 under the specified bucket and key.
         */
        public KS3Object getObject(GetObjectRequest getObjectRequest)
        {
            String bucketName = getObjectRequest.getBucketName();
            String key = getObjectRequest.getKey();

            Request<GetObjectRequest> request = this.createRequest(bucketName, key, getObjectRequest, HttpMethod.GET);

            if (getObjectRequest.getRange() != null)
            {
                long[] range = getObjectRequest.getRange();
                request.setHeader(Headers.RANGE, range[0].ToString() + "-" + range[1].ToString());
            }

            addDateHeader(request, Headers.GET_OBJECT_IF_MODIFIED_SINCE, getObjectRequest.getModifiedSinceConstraint());
            addDateHeader(request, Headers.GET_OBJECT_IF_UNMODIFIED_SINCE, getObjectRequest.getUnmodifiedSinceConstraint());
            addStringListHeader(request, Headers.GET_OBJECT_IF_MATCH, getObjectRequest.getMatchingETagConstraints());
            addStringListHeader(request, Headers.GET_OBJECT_IF_NONE_MATCH, getObjectRequest.getNonmatchingETagConstraints());

            ProgressListener progressListener = getObjectRequest.getProgressListener();

            fireProgressEvent(progressListener, ProgressEvent.STARTED);

            KS3Object ks3Object = null;
            try
            {
                ks3Object = this.invoke(request, new ObjectResponseHandler(getObjectRequest), bucketName, key);
            }
            catch (ProgressInterruptedException e)
            {
                fireProgressEvent(progressListener, ProgressEvent.CANCELED);
                throw e;
            }
            catch (Exception e)
            {
                fireProgressEvent(progressListener, ProgressEvent.FAILED);
                throw e;
            }
            fireProgressEvent(progressListener, ProgressEvent.COMPLETED);

            ks3Object.setBucketName(bucketName);
            ks3Object.setKey(key);
            
            return ks3Object;
        }

        /*
         * Gets the metadata for the specified KS3 object without actually fetching the object itself.
         */
        public ObjectMetadata getObjectMetadata(String bucketName, String key)
        {
            return this.getObjectMetadata(new GetObjectMetadataRequest(bucketName, key));
        }

        /*
         * Gets the metadata for the specified KS3 object without actually fetching the object itself.
         */
        public ObjectMetadata getObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest)
        {
            String bucketName = getObjectMetadataRequest.getBucketName();
            String key = getObjectMetadataRequest.getKey();

            Request<GetObjectMetadataRequest> request = this.createRequest(bucketName, key, getObjectMetadataRequest, HttpMethod.HEAD);

            return invoke(request, new MetadataResponseHandler(), bucketName, key);
        }

        /**
         * Uploads the specified file to KS3 under the specified bucket and key name.
         */
        public PutObjectResult putObject(String bucketName, String key, FileInfo file)
        {
            PutObjectRequest putObjectRequest = new PutObjectRequest(bucketName, key, file);
            putObjectRequest.setMetadata(new ObjectMetadata());
            return this.putObject(putObjectRequest);
        }

        /**
         * Uploads the specified input stream and object metadata to KS3 under the specified bucket and key name. 
         */
        public PutObjectResult putObject(String bucketName, String key, Stream input, ObjectMetadata metadata)
        {
            return this.putObject(new PutObjectRequest(bucketName, key, input, metadata));
        }

        /**
         * Uploads a new object to the specified KS3 bucket.
         */
        public PutObjectResult putObject(PutObjectRequest putObjectRequest)
        {
            String bucketName = putObjectRequest.getBucketName();
            String key = putObjectRequest.getKey();
            ObjectMetadata metadata = putObjectRequest.getMetadata();
            Stream input = putObjectRequest.getInputStream();
            ProgressListener progressListener = putObjectRequest.getProgressListener();
            if (metadata == null)
                metadata = new ObjectMetadata();

            // If a file is specified for upload, we need to pull some additional
            // information from it to auto-configure a few options
            if (putObjectRequest.getFile() != null)
            {
                FileInfo file = putObjectRequest.getFile();
                
                // Always set the content length, even if it's already set
                metadata.setContentLength(file.Length);

                // Only set the content type if it hasn't already been set
                if (metadata.getContentType() == null)
                    metadata.setContentType(Mimetypes.getMimetype(file));

                if (metadata.getContentMD5() == null)
                {
                    using (FileStream fileStream = file.OpenRead())
                    {
                        MD5 md5 = MD5.Create();
                        metadata.setContentMD5(Convert.ToBase64String(md5.ComputeHash(fileStream)));
                    }
                }

                input = file.OpenRead();
            }
            else
            {
                metadata.setContentLength(input.Length);

                if (metadata.getContentType() == null)
                    metadata.setContentType(Mimetypes.DEFAULT_MIMETYPE);
                if (metadata.getContentMD5() == null)
                {
                    using(MD5 md5 = MD5.Create())
                    {
                        metadata.setContentMD5(Convert.ToBase64String(md5.ComputeHash(input)));
                    }

                    input.Seek(0, SeekOrigin.Begin); // It is needed after calculated MD5.
                }
            }
            
            Request<PutObjectRequest> request = this.createRequest(bucketName, key, putObjectRequest, HttpMethod.PUT);

            if (putObjectRequest.getAcl() != null)
                addAclHeaders(request, putObjectRequest.getAcl());
            else if (putObjectRequest.getCannedAcl() != null)
                request.setHeader(Headers.KS3_CANNED_ACL, putObjectRequest.getCannedAcl().getCannedAclHeader());

            if (progressListener != null)
            {
                input = new ProgressReportingInputStream(input, progressListener);
                fireProgressEvent(progressListener, ProgressEvent.STARTED);
            }

            populateRequestMetadata(metadata, request);
            request.setContent(input);

            //-----------------------------------------------

            ObjectMetadata returnedMetadata = null;
            try
            {
                returnedMetadata = this.invoke(request, new MetadataResponseHandler(), bucketName, key);
            }
            catch (ProgressInterruptedException e)
            {
                fireProgressEvent(progressListener, ProgressEvent.CANCELED);
                throw e;
            }
            catch (Exception e)
            {
                fireProgressEvent(progressListener, ProgressEvent.FAILED);
                throw e;
            }
            finally
            {
                if (input != null)
                    input.Close();
            }

            fireProgressEvent(progressListener, ProgressEvent.COMPLETED);

            PutObjectResult result = new PutObjectResult();
            result.setETag(returnedMetadata.getETag());
            result.setContentMD5(metadata.getContentMD5());

            return result;
        }
        /// <summary>
        /// This implementation of the PUT operation creates a copy of an object that is already stored in S3. A PUT copy operation is the same as performing a GET and then a PUT. Adding the request header, x-amz-copy-source, makes the PUT operation copy the source object into the destination bucket.
        /// </summary>
        /// <param name="copyObjectRequest"></param>
        /// <returns></returns>
        public CopyObjectResult copyObject(CopyObjectRequest copyObjectRequest)
        {
            Request<CopyObjectRequest> request = this.createRequest(copyObjectRequest.DestinationBucket, copyObjectRequest.DestinationObject, copyObjectRequest, HttpMethod.PUT);
            request.getHeaders().Add(Headers.XKssCopySource, "/" + copyObjectRequest.SourceBucket + "/" + UrlEncoder.encode(copyObjectRequest.SourceObject, Encoding.UTF8));
            if (copyObjectRequest.AccessControlList != null)
                addAclHeaders(request, copyObjectRequest.AccessControlList);
            else if (copyObjectRequest.CannedAcl != null)
                request.setHeader(Headers.KS3_CANNED_ACL, copyObjectRequest.CannedAcl.getCannedAclHeader());
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";
            return this.invoke(request, new CopyObjectResultUnmarshaller(), copyObjectRequest.DestinationBucket, copyObjectRequest.DestinationObject);
        }
        /// <summary>
        /// The HEAD operation retrieves metadata from an object without returning the object itself. This operation is useful if you are interested only in an object's metadata. To use HEAD, you must have READ access to the object.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectKey"></param>
        /// <returns></returns>
        public HeadObjectResult headObject(String bucketName, String objectKey)
        {
            return headObject(bucketName,objectKey);
        }
        public HeadObjectResult headObject(HeadObjectRequest headObjectRequest)
        {
            Request<HeadObjectRequest> request = this.createRequest(headObjectRequest.BucketName, headObjectRequest.ObjectKey, headObjectRequest, HttpMethod.HEAD);
            headObjectRequest.validate();
            if (headObjectRequest.MatchingETagConstraints.Count>0)
            {
                StringBuilder Etags = new StringBuilder();
                foreach(String Etag in headObjectRequest.MatchingETagConstraints){
                    Etags.Append(Etag);
                    Etags.Append(",");
                }
                request.getHeaders().Add(Headers.GET_OBJECT_IF_MATCH, Etags.ToString().TrimEnd(','));
            }
            if(headObjectRequest.NonmatchingEtagConstraints.Count>0){
                StringBuilder noEtags = new StringBuilder();
                foreach(String etag in headObjectRequest.NonmatchingEtagConstraints){
                    noEtags.Append(etag);
                    noEtags.Append(",");
                }
                request.getHeaders().Add(Headers.GET_OBJECT_IF_NONE_MATCH, noEtags.ToString().TrimEnd(','));
            }
            if (!headObjectRequest.ModifiedSinceConstraint.Equals(DateTime.MinValue))
            {
                request.getHeaders().Add(Headers.GET_OBJECT_IF_MODIFIED_SINCE,headObjectRequest.ModifiedSinceConstraint.ToUniversalTime().ToString("r"));
            }
            if (!headObjectRequest.UnmodifiedSinceConstraint.Equals(DateTime.MinValue))
            {
                request.getHeaders().Add(Headers.GET_OBJECT_IF_UNMODIFIED_SINCE, headObjectRequest.UnmodifiedSinceConstraint.ToUniversalTime().ToString("r"));
            }

            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.CacheControl))
                request.getParameters().Add("response-cache-control", headObjectRequest.Overrides.CacheControl);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentType))
                request.getParameters().Add("&response-content-type" , headObjectRequest.Overrides.ContentType);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentLanguage))
                request.getParameters().Add("&response-content-language" , headObjectRequest.Overrides.ContentLanguage);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.Expires))
                request.getParameters().Add("&response-expires" , headObjectRequest.Overrides.Expires);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentDisposition))
                request.getParameters().Add("&response-content-disposition" , headObjectRequest.Overrides.ContentDisposition);
            if (!string.IsNullOrEmpty(headObjectRequest.Overrides.ContentEncoding))
                request.getParameters().Add("&response-content-encoding" , headObjectRequest.Overrides.ContentEncoding);

            return this.invoke(request, new HeadObjectResultHandler(), headObjectRequest.BucketName, headObjectRequest.ObjectKey);
        }
        /**
         * init multi upload big file
         * **/
        public InitiateMultipartUploadResult initiateMultipartUpload(string bucketname, string objectkey)
        {
            return initiateMultipartUpload(new InitiateMultipartUploadRequest(bucketname, objectkey));
        }
        public InitiateMultipartUploadResult initiateMultipartUpload(InitiateMultipartUploadRequest param)
        {
            Request<InitiateMultipartUploadRequest> request = this.createRequest(param.Bucketname, param.Objectkey, param, HttpMethod.POST);
            request.setParameter("uploads",null);
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";
            InitiateMultipartUploadResult result = new InitiateMultipartUploadResult();
            result = this.invoke(request, new MultipartUploadResultUnmarshaller(), param.Bucketname, param.Objectkey);
            return result;
        }
        /**
         * upload multi file by part
         * **/
        public PartETag uploadPart(UploadPartRequest param) {
            String bucketName = param.getBucketname();
            String key = param.getObjectkey();
            ObjectMetadata metadata = param.getMetadata();
            Stream input = param.getInputStream();
            ProgressListener progressListener = param.getProgressListener();

            if (metadata == null)
                metadata = new ObjectMetadata();

            // If a file is specified for upload, we need to pull some additional
            // information from it to auto-configure a few options
            metadata.setContentLength(input.Length);

            if (metadata.getContentType() == null)
                metadata.setContentType(Mimetypes.DEFAULT_MIMETYPE);
            if (metadata.getContentMD5() == null)
            {
                using (MD5 md5 = MD5.Create())
                {
                    metadata.setContentMD5(Convert.ToBase64String(md5.ComputeHash(input)));
                }

                input.Seek(0, SeekOrigin.Begin); // It is needed after calculated MD5.
            }

            Request<UploadPartRequest> request = this.createRequest(param.getBucketname(), param.getObjectkey(), param, HttpMethod.PUT);
            request.setParameter("partNumber", param.getPartNumber().ToString());
            request.setParameter("uploadId", param.getUploadId());



            if (progressListener != null)
            {
                input = new ProgressReportingInputStream(input, progressListener);
                fireProgressEvent(progressListener, ProgressEvent.STARTED);
            }

            populateRequestMetadata(metadata, request);
            request.setContent(input);

            //-----------------------------------------------

            ObjectMetadata returnedMetadata = null;
            try
            {
                returnedMetadata = this.invoke(request, new MetadataResponseHandler(), bucketName, key);
            }
            catch (ProgressInterruptedException e)
            {
                fireProgressEvent(progressListener, ProgressEvent.CANCELED);
                throw e;
            }
            catch (Exception e)
            {
                fireProgressEvent(progressListener, ProgressEvent.FAILED);
                throw e;
            }
            finally
            {
                if (input != null)
                    input.Close();
            }

            fireProgressEvent(progressListener, ProgressEvent.COMPLETED);

            PartETag result = new PartETag();
            result.seteTag(returnedMetadata.getETag());
            result.setPartNumber(param.getPartNumber());

            return result;
	    }
        /**
         * getlist had uploaded part list
         * **/
        public ListMultipartUploadsResult getListMultipartUploads(ListMultipartUploadsRequest param) {
            Request<ListMultipartUploadsRequest> request = this.createRequest(param.getBucketname(), param.getObjectkey(), param, HttpMethod.GET);
            request.setParameter("uploadId", param.getUploadId());
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";
            ListMultipartUploadsResult result = new ListMultipartUploadsResult();
            result = this.invoke(request, new ListMultipartUploadsResultUnmarshaller(), param.getBucketname(), param.getObjectkey());
            return result;
        }
        /**
         * submit the all part,the server will complete join part
         * **/
        public CompleteMultipartUploadResult completeMultipartUpload(CompleteMultipartUploadRequest param)
        {
            Request<CompleteMultipartUploadRequest> request = this.createRequest(param.getBucketname(), param.getObjectkey(), param, HttpMethod.POST);
            request.setParameter("uploadId", param.getUploadId());
            request.setHeader(Headers.CONTENT_LENGTH, param.getContent().Length.ToString());
            request.setContent(param.getContent());
            CompleteMultipartUploadResult result = new CompleteMultipartUploadResult();
            result = this.invoke(request, new CompleteMultipartUploadResultUnmarshaller(), param.getBucketname(), param.getObjectkey());
            return result;
        }
        /**
         * abort the upload opertion by uploadid
         * **/
        public void AbortMultipartUpload(AbortMultipartUploadRequest param) {
            Request<AbortMultipartUploadRequest> request = this.createRequest(param.getBucketname(), param.getObjectkey(), param, HttpMethod.DELETE);
            request.setParameter("uploadId", param.getUploadId());
            request.getHeaders()[Headers.CONTENT_LENGTH] = "0";
            this.invoke(request,voidResponseHandler, param.getBucketname(), param.getObjectkey());
        }

        /**
         * Gets the AccessControlList (ACL) for the specified object in KS3.
         */
        public AccessControlList getObjectAcl(String bucketName, String key)
        {
            return this.getObjectAcl(new GetObjectAclRequest(bucketName, key));
        }

        /**
         * Gets the AccessControlList (ACL) for the specified object in KS3.
         */
        public AccessControlList getObjectAcl(GetObjectAclRequest getObjectAclRequest)
        {
            String bucketName = getObjectAclRequest.getBucketName();
            String key = getObjectAclRequest.getKey();

            Request<GetObjectAclRequest> request = this.createRequest(bucketName, key, getObjectAclRequest, HttpMethod.GET);
            request.setParameter("acl", null);

            return this.invoke(request, new AccessControlListUnmarshaller(), bucketName, key);
        }

        /**
         * Sets the AccessControlList for the specified object in KS3.
         */
        public void setObjectAcl(String bucketName, String key, AccessControlList acl)
        {
            this.setObjectAcl(new SetObjectAclRequest(bucketName, key, acl));
        }

        /**
         * Sets the AccessControlList for the specified object in KS3.
         */
        public void setObjectAcl(String bucketName, String key, CannedAccessControlList cannedAcl)
        {
            this.setObjectAcl(new SetObjectAclRequest(bucketName, key, cannedAcl));
        }

        /**
         * Sets the AccessControlList for the specified object in KS3.
         */
        public void setObjectAcl(SetObjectAclRequest setObjectAclRequest)
        {
            String bucketName = setObjectAclRequest.getBucketName();
            String key = setObjectAclRequest.getKey();
            AccessControlList acl = setObjectAclRequest.getAcl();
            CannedAccessControlList cannedAcl = setObjectAclRequest.getCannedAcl();

            Request<SetObjectAclRequest> request = this.createRequest(bucketName, key, setObjectAclRequest, HttpMethod.PUT);

            if (acl != null)
            {
                String xml = AclXmlFactory.convertToXmlString(acl);
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

                request.setContent(memoryStream);
                request.setHeader(Headers.CONTENT_LENGTH, memoryStream.Length.ToString());
            }
            else if (cannedAcl != null)
            {
                request.setHeader(Headers.KS3_CANNED_ACL, cannedAcl.getCannedAclHeader());
                request.setHeader(Headers.CONTENT_LENGTH, "0");
            }
            request.setParameter("acl", null);

            this.invoke(request, this.voidResponseHandler, bucketName, key);
        }
        /// <summary>
        /// generate presignerd url for private object with in limit times
        /// </summary>
        /// <param name="bucketName">bucketname</param>
        /// <param name="key">objectkey</param>
        /// <param name="expiration">expire time</param>
        /// <returns>url</returns>
        public string generatePresignedUrl(string bucketName, string key, DateTime expiration)
        {
            return this.generatePresignedUrl(bucketName, key, expiration, null);
        }
        /// <summary>
        /// generate PresignedUrl the url can apply for other user
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public string generatePresignedUrl(string bucketName, string key,DateTime expiration, ResponseHeaderOverrides overrides)
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
                KS3Signer<NoneKS3Request> ks3Signer = createSigner<NoneKS3Request>(HttpMethod.GET.ToString(), bucketName, key);
                string signer = ks3Signer.getSignature(this.ks3Credentials, expires.ToString());
                url += @"http://" + bucketName + "." + Constants.KS3_HOSTNAME
                             + "/" + filterSpecial(UrlEncoder.encode(key, Constants.DEFAULT_ENCODING)) + "?AccessKeyId="
                             + UrlEncoder.encode(this.ks3Credentials.getKS3AccessKeyId(), Constants.DEFAULT_ENCODING)
                             + "&Expires=" + expires + "&Signature="
                             + UrlEncoder.encode(signer, Constants.DEFAULT_ENCODING) + "&" + param;

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
        public String putAdpTask(PutAdpRequest putAdpRequest)
        {
            Request<PutAdpRequest> request = this.createRequest(putAdpRequest.BucketName, putAdpRequest.ObjectKey, putAdpRequest, HttpMethod.PUT);
            request.getParameters().Add("adp", null);

            request.getHeaders().Add(Headers.AsynchronousProcessingList, putAdpRequest.convertAdpsToString());
            request.getHeaders().Add(Headers.NotifyURL,putAdpRequest.NotifyURL);
            request.setHeader(Headers.CONTENT_LENGTH, "0");
            PutAdpResult result= this.invoke(request, new PutAdpResponseHandler(), putAdpRequest.BucketName, putAdpRequest.ObjectKey);
            return result.TaskId;
        }
        public GetAdpResult getAdpTask(GetAdpRequest getAdpRequest)
        {
            Request<GetAdpRequest> request = this.createRequest(getAdpRequest.TaskId, null, getAdpRequest, HttpMethod.GET);
            request.getParameters().Add("queryadp",null);
            return this.invoke(request, new GetAdpResultUnmarshaller(), null, null);
        }
        ////////////////////////////////////////////////////////////////////////////////////////


        /**
         * Creates and initializes a new request object for the specified KS3 resource.
         * Three parameters needed to be set
         * 1. http method (GET, PUT, HEAD or DELETE)
         * 2. endpoint (http or https, and the host name. e.g. http://kss.ksyun.com)
         * 3. resource path (bucketName/[key], e.g. my-bucket/my-object)
         */
        private Request<X> createRequest<X>(String bucketName, String key, X originalRequest, HttpMethod httpMethod) where X : KS3Request 
        {
            Request<X> request = new DefaultRequest<X>(originalRequest);
            request.setHttpMethod(httpMethod);
            request.setEndpoint(endpoint);
            
            String resourcePath = "/" + (bucketName != null ? bucketName + "/" : "") + (key != null ? key : "");
            resourcePath = UrlEncoder.encode(resourcePath, Constants.DEFAULT_ENCODING);
            //resourcePath = filterSpecial(resourcePath);
            
            request.setResourcePath(resourcePath);

            return request;
        }

        private X invoke<X, Y>(Request<Y> request, Unmarshaller<X, Stream> unmarshaller, String bucketName, String key) where Y : KS3Request
        {
            return this.invoke(request, new XmlResponseHandler<X>(unmarshaller), bucketName, key);
        }

        /**
         * Before the KS3HttpClient deal with the request, we want the request looked like a collection of that:
         * 1. http method
         * 2. endpoint
         * 3. resource path
         * 4. headers
         * 5. parameters
         * 6. content
         * 7. time offset
         * 
         * The first three points are done in "createRequest".
         * The content was set before "createRequest" when we need to put a object to server. And some metadata like Content-Type, Content-Length, etc.
         * So at here, we need to complete 4, 5, and 7.
         */
        private X invoke<X, Y>(Request<Y> request, HttpResponseHandler<X> responseHandler, String bucket, String key) where Y : KS3Request
        {
            IDictionary<String, String> parameters = request.getOriginalRequest().copyPrivateRequestParameters();
            foreach (String name in parameters.Keys)
                request.setParameter(name, parameters[name]);

            request.setTimeOffset(timeOffset);

            /**
             * The string we sign needs to include the exact headers that we
             * send with the request, but the client runtime layer adds the
             * Content-Type header before the request is sent if one isn't set, so
             * we have to set something here otherwise the request will fail.
             */
            if (!request.getHeaders().ContainsKey(Headers.CONTENT_TYPE))
                request.setHeader(Headers.CONTENT_TYPE, Mimetypes.DEFAULT_MIMETYPE);
            
            /**
             * Set the credentials which will be used by the KS3Signer later.
             */
            if(request.getOriginalRequest().getRequestCredentials() == null)
                request.getOriginalRequest().setRequestCredentials(this.ks3Credentials);

            return client.excute(request, responseHandler, createSigner(request, bucket, key));
        }

        private KS3Signer<T> createSigner<T>(Request<T> request, String bucketName, String key) where T : KS3Request
        {
            return createSigner<T>(request.getHttpMethod().ToString(), bucketName, key);
        }
        private KS3Signer<T> createSigner<T>(String httpMethod, String bucketName, String key) where T : KS3Request
        {
            String resourcePath = "/" + (bucketName != null ? bucketName + "/" : "") + (key != null ? key : "");
            resourcePath = UrlEncoder.encode(resourcePath, Constants.DEFAULT_ENCODING);
            return new KS3Signer<T>(httpMethod, resourcePath);
        }
        /**
         * Fires a progress event with the specified event type to the specified
         * listener.
         */
        private static void fireProgressEvent(ProgressListener listener, int eventType) {
            if (listener == null) return;

            ProgressEvent e = new ProgressEvent(eventType);
            listener.progressChanged(e);
        }

        /**
         * Populates the specified request object with the appropriate headers from
         * the {@link ObjectMetadata} object.
         */
        private static void populateRequestMetadata<X>(ObjectMetadata metadata, Request<X> request) where X : KS3Request
        {
            IDictionary<String, Object> rawMetadata = metadata.getRawMetadata();
            if (rawMetadata != null)
            {
                foreach (String name in rawMetadata.Keys)
                    request.setHeader(name, rawMetadata[name].ToString());
            }

            IDictionary<String, String> userMetadata = metadata.getUserMetadata();
            if (userMetadata != null)
            {
                foreach (String name in userMetadata.Keys)
                    request.setHeader(Headers.KS3_USER_METADATA_PREFIX + name, userMetadata[name]);
            }
        }

	    /**
	     * Adds the specified date header in RFC 822 date format to the specified
	     * request. This method will not add a date header if the specified date
	     * value is <code>null</code>.
	     */
        private static void addDateHeader<X>(Request<X> request, String header, DateTime? value)
        {
            if (value != null)
                request.setHeader(header, SignerUtils.getSignatrueDate(value.Value));
        }

        /*
         * Adds the specified string list header, joined together separated with
         * commas, to the specified request.
         * This method will not add a string list header if the specified values
         * are <code>null</code> or empty.
         */
        private static void addStringListHeader<X>(Request<X> request, String header, IList<String> values)
        {
            if (values != null && values.Count > 0)
                request.setHeader(header, String.Join(", ", values));
        }
        private static string filterSpecial(string key) {
            if(!String.IsNullOrEmpty(key)){
                key = key.Replace("%5C","/").Replace("//", "/%2F");
            }
            return key;
        }
        /**
         * Sets the acccess control headers for the request given.
         */
        private static void addAclHeaders<X>(Request<X> request, AccessControlList acl) where X : KS3Request
        {
            ISet<Grant> grants = acl.getGrants();
            IDictionary<String, IList<Grantee>> grantsByPermission = new Dictionary<String, IList<Grantee>>();
            foreach (Grant grant in grants)
            {
                if (!grantsByPermission.ContainsKey(grant.getPermission()))
                    grantsByPermission[grant.getPermission()] = new List<Grantee>();
                grantsByPermission[grant.getPermission()].Add(grant.getGrantee());
            }
            foreach (String permission in Permission.listPermissions())
            {
                if (grantsByPermission.ContainsKey(permission))
                {
                    IList<Grantee> grantees = grantsByPermission[permission];
                    bool first = true;
                    StringBuilder granteeString = new StringBuilder();
                    foreach (Grantee grantee in grantees)
                    {
                        if (first)
                            first = false;
                        else
                            granteeString.Append(", ");
                        granteeString.Append(grantee.getTypeIdentifier() + "=\"" + grantee.getIdentifier() + "\"");
                    }
                    request.setHeader(Permission.getHeaderName(permission), granteeString.ToString());
                }
            }
        } // end of addAclHeader
    } // end of class KS3Client
}
