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
        /// <returns></returns>
        IList<Bucket> ListBuckets();

        /// <summary>
        /// Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
        /// </summary>
        /// <param name="listBucketRequest"></param>
        /// <returns></returns>
        IList<Bucket> ListBuckets(ListBucketsRequest listBucketRequest);

        /// <summary>
        /// Deletes the specified bucket. 
        /// </summary>
        /// <param name="bucketName"></param>
        void DeleteBucket(string bucketName);

        /// <summary>
        ///  Deletes the specified bucket. 
        /// </summary>
        /// <param name="deleteBucketRequest"></param>
        void DeleteBucket(DeleteBucketRequest deleteBucketRequest);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        AccessControlList GetBucketAcl(String bucketName);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="getBucketAclRequest"></param>
        /// <returns></returns>
        AccessControlList GetBucketAcl(GetBucketAclRequest getBucketAclRequest);

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Bucket CreateBucket(string bucketName);

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="createBucketRequest"></param>
        /// <returns></returns>
        Bucket CreateBucket(CreateBucketRequest createBucketRequest);

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="acl"></param>
        void SetBucketAcl(string bucketName, AccessControlList acl);

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="cannedAcl"></param>
        void SetBucketAcl(string bucketName, CannedAccessControlList cannedAcl);

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="setBucketAclRequset"></param>
        void SetBucketAcl(SetBucketAclRequest setBucketAclRequset);

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ObjectListing ListObjects(String bucketName);

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        ObjectListing ListObjects(string bucketName, string prefix);

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="listObjectRequest"></param>
        /// <returns></returns>
        ObjectListing ListObjects(ListObjectsRequest listObjectRequest);

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        void DeleteObject(string bucketName, string key);

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="deleteObjectRequest"></param>
        void DeleteObject(DeleteObjectRequest deleteObjectRequest);

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        KS3Object GetObject(string bucketName, string key);

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key, and saves the object contents to the specified file.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="destinationFile"></param>
        /// <returns></returns>
        KS3Object GetObject(string bucketName, string key, FileInfo destinationFile);

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="getObjectRequest"></param>
        /// <returns></returns>
        KS3Object GetObject(GetObjectRequest getObjectRequest);

        /// <summary>
        /// Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        ObjectMetadata GetObjectMetadata(string bucketName, string key);

        /// <summary>
        /// Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="getObjectMetadataRequest"></param>
        /// <returns></returns>
        ObjectMetadata GetObjectMetadata(GetObjectMetadataRequest getObjectMetadataRequest);

        /// <summary>
        /// Uploads the specified file to KS3 under the specified bucket and key name.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        PutObjectResult PutObject(string bucketName, string key, FileInfo file);

        /// <summary>
        /// Uploads the specified input stream and object metadata to KS3 under the specified bucket and key name. 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        PutObjectResult PutObject(string bucketName, string key, Stream input, ObjectMetadata metadata);

        /// <summary>
        /// Uploads a new object to the specified KS3 bucket.
        /// </summary>
        /// <param name="putObjectRequest"></param>
        /// <returns></returns>
        PutObjectResult PutObject(PutObjectRequest putObjectRequest);

        InitiateMultipartUploadResult InitiateMultipartUpload(string bucketname, string objectkey);

        InitiateMultipartUploadResult InitiateMultipartUpload(InitiateMultipartUploadRequest request);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified object in KS3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        AccessControlList GetObjectAcl(string bucketName, string key);

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified object in KS3.
        /// </summary>
        /// <param name="getObjectAclRequest"></param>
        /// <returns></returns>
        AccessControlList GetObjectAcl(GetObjectAclRequest getObjectAclRequest);

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="acl"></param>
        void SetObjectAcl(string bucketName, string key, AccessControlList acl);

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="cannedAcl"></param>
        void SetObjectAcl(string bucketName, string key, CannedAccessControlList cannedAcl);

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="setObjectRequestAcl"></param>
        void SetObjectAcl(SetObjectAclRequest setObjectRequestAcl);
    }
}
