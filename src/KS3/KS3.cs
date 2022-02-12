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
        ///  Deletes the specified bucket. 
        /// </summary>
        /// <param name="deleteBucketRequest"></param>
        void DeleteBucket(DeleteBucketRequest deleteBucketRequest);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="getBucketAclRequest"></param>
        /// <returns></returns>
        AccessControlList GetBucketAcl(GetBucketAclRequest getBucketAclRequest);

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="createBucketRequest"></param>
        /// <returns></returns>
        Bucket CreateBucket(CreateBucketRequest createBucketRequest);

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="setBucketAclRequset"></param>
        void SetBucketAcl(SetBucketAclRequest setBucketAclRequset);

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="listObjectRequest"></param>
        /// <returns></returns>
        ObjectListing ListObjects(ListObjectsRequest listObjectRequest);

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="deleteObjectRequest"></param>
        void DeleteObject(DeleteObjectRequest deleteObjectRequest);

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="getObjectRequest"></param>
        /// <returns></returns>
        KS3Object GetObject(GetObjectRequest getObjectRequest);

        /// <summary>
        /// Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="getObjectMetadataRequest"></param>
        /// <returns></returns>
        ObjectMetadata GetObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest);

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
    }
}
