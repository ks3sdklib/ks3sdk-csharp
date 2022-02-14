using KS3.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace KS3
{
    /// <summary>
    /// Provides an interface for accessing the KS3.
    /// </summary>
    public interface IKS3
    {
        /// <summary>
        /// Overrides the default endpoint for this client.
        /// </summary>
        /// <param name="endpoint"></param>
        void SetEndpoint(string endpoint);

        /// <summary>
        /// Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
        /// </summary>
        /// <param name="listBucketRequest"></param>
        /// <returns></returns>
        IList<Bucket> ListBuckets(ListBucketsRequest listBucketRequest);

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="createBucketRequest"></param>
        /// <returns></returns>
        Bucket CreateBucket(CreateBucketRequest createBucketRequest);

        /// <summary>
        ///  Deletes the specified bucket. 
        /// </summary>
        /// <param name="deleteBucketRequest"></param>
        void DeleteBucket(DeleteBucketRequest deleteBucketRequest);

        /// <summary>
        /// This operation is useful to determine if a bucket exists and you have permission to access it
        /// </summary>
        /// <param name="headBucketRequest"></param>
        /// <returns></returns>
        HeadBucketResult HeadBucket(HeadBucketRequest headBucketRequest);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="getBucketAclRequest"></param>
        /// <returns></returns>
        AccessControlList GetBucketAcl(GetBucketAclRequest getBucketAclRequest);

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="setBucketAclRequset"></param>
        void SetBucketAcl(SetBucketAclRequest setBucketAclRequset);

        /// <summary>
        /// Returns the cors configuration information set for the bucket.
        /// To use this operation, you must have permission to perform the s3:GetBucketCORS action. By default, the bucket owner has this permission and can grant it to others.
        /// </summary>
        /// <param name="getBucketCorsRequest"></param>
        /// <returns></returns>
        BucketCorsConfigurationResult GetBucketCors(GetBucketCorsRequest getBucketCorsRequest);

        /// <summary>
        /// Sets the cors configuration for your bucket. If the configuration exists, Amazon S3 replaces it. 
        /// </summary>
        /// <param name="putBucketCorsRequest"></param>
        void SetBucketCors(PutBucketCorsRequest putBucketCorsRequest);

        /// <summary>
        /// Deletes the cors configuration information set for the bucket.
        /// </summary>
        /// <param name="deleteBucketCorsRequest"></param>
        void DeleteBucketCors(DeleteBucketCorsRequest deleteBucketCorsRequest);

        /// <summary>
        /// This implementation of the GET operation uses the location subresource to return a bucket's region. You set the bucket's region using the LocationConstraint request parameter in a PUT Bucket request. 
        /// </summary>
        /// <param name="getBucketLocationRequest"></param>
        /// <returns></returns>
        GetBucketLocationResult GetBucketLocation(GetBucketLocationRequest getBucketLocationRequest);

        /// <summary>
        /// This implementation of the GET operation uses the logging subresource to return the logging status of a bucket and the permissions users have to view and modify that status. To use GET, you must be the bucket owner. 
        /// </summary>
        /// <param name="getBucketLoggingRequest"></param>
        /// <returns></returns>
        GetBucketLoggingResult GetBucketLogging(GetBucketLoggingRequest getBucketLoggingRequest);

        /// <summary>
        /// This implementation of the PUT operation uses the logging subresource to set the logging parameters for a bucket and to specify permissions for who can view and modify the logging parameters. To set the logging status of a bucket, you must be the bucket owner.
        /// </summary>
        /// <param name="putBucketLoggingRequest"></param>
        void SetBucketLogging(PutBucketLoggingRequest putBucketLoggingRequest);

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="listObjectRequest"></param>
        /// <returns></returns>
        ObjectListing ListObjects(ListObjectsRequest listObjectRequest);

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="getObjectRequest"></param>
        /// <returns></returns>
        KS3Object GetObject(GetObjectRequest getObjectRequest);

        /// <summary>
        /// Uploads a new object to the specified KS3 bucket.
        /// </summary>
        /// <param name="putObjectRequest"></param>
        /// <returns></returns>
        PutObjectResult PutObject(PutObjectRequest putObjectRequest);

        /// <summary>
        /// init multi upload big file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest request);

        /// <summary>
        /// upload multi file by part
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        PartETag UploadPart(UploadPartRequest param);

        /// <summary>
        /// getlist had uploaded part list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        ListMultipartUploadsResult GetListMultipartUploads(ListMultipartUploadsRequest param);

        /// <summary>
        /// submit the all part,the server will complete join part
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        CompleteMultipartUploadResult CompleteMultipartUpload(CompleteMultipartUploadRequest param);

        /// <summary>
        /// Abort the upload opertion by uploadid
        /// </summary>
        /// <param name="param"></param>
        void AbortMultipartUpload(AbortMultipartUploadRequest param);

        /// <summary>
        /// This implementation of the PUT operation creates a copy of an object that is already stored in S3. A PUT copy operation is the same as performing a GET and then a PUT. Adding the request header, x-amz-copy-source, makes the PUT operation copy the source object into the destination bucket.
        /// </summary>
        /// <param name="copyObjectRequest"></param>
        /// <returns></returns>
        CopyObjectResult CopyObject(CopyObjectRequest copyObjectRequest);

        /// <summary>
        ///  The HEAD operation retrieves metadata from an object without returning the object itself. This operation is useful if you are interested only in an object's metadata. To use HEAD, you must have READ access to the object.
        /// </summary>
        /// <param name="headObjectRequest"></param>
        /// <returns></returns>
        HeadObjectResult HeadObject(HeadObjectRequest headObjectRequest);

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="deleteObjectRequest"></param>
        void DeleteObject(DeleteObjectRequest deleteObjectRequest);

        /// <summary>
        /// The Multi-Object Delete operation enables you to delete multiple objects from a bucket using a single HTTP request.
        /// </summary>
        /// <param name="deleteMultipleObjectsRequest"></param>
        /// <returns></returns>
        DeleteMultipleObjectsResult DeleteMultiObjects(DeleteMultipleObjectsRequest deleteMultipleObjectsRequest);

        /// <summary>
        /// Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="getObjectMetadataRequest"></param>
        /// <returns></returns>
        ObjectMetadata GetObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified object in KS3.
        /// </summary>
        /// <param name="getObjectAclRequest"></param>
        /// <returns></returns>
        AccessControlList GetObjectAcl(GetObjectAclRequest getObjectAclRequest);

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="setObjectRequestAcl"></param>
        void SetObjectAcl(SetObjectAclRequest setObjectRequestAcl);

        /// <summary>
        /// generate PresignedUrl the url can apply for other user
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        string GeneratePresignedUrl(string bucketName, string key, DateTime expiration, ResponseHeaderOverrides overrides);

        /// <summary>
        /// Get adp task
        /// </summary>
        /// <param name="getAdpRequest"></param>
        /// <returns></returns>
        GetAdpResult GetAdpTask(GetAdpRequest getAdpRequest);

        /// <summary>
        /// add Asynchronous Data Processing 可以通过adp执行图片缩略图处理、执行转码操作等
        /// </summary>
        /// <param name="putAdpRequest"></param>
        /// <returns></returns>
        string PutAdpTask(PutAdpRequest putAdpRequest);
    }
}
