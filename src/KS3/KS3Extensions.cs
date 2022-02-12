using KS3.Model;
using System.Collections.Generic;
using System.IO;

namespace KS3
{
    public static class KS3Extensions
    {
        /// <summary>
        /// Returns a list of all KS3 buckets that the authenticated sender of the request owns. 
        /// </summary>
        /// <param name="ks3"></param>
        /// <returns></returns>
        public static IList<Bucket> ListBuckets(this IKS3 ks3)
        {
            return ks3.ListBuckets(new ListBucketsRequest());
        }

        /// <summary>
        /// Deletes the specified bucket. 
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        public static void DeleteBucket(this IKS3 ks3, string bucketName)
        {
            ks3.DeleteBucket(new DeleteBucketRequest(bucketName));
        }

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified KS3 bucket.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public static AccessControlList GetBucketAcl(this IKS3 ks3, string bucketName)
        {
            return ks3.GetBucketAcl(new GetBucketAclRequest(bucketName));
        }

        /// <summary>
        /// Creates a new KS3 bucket. 
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public static Bucket CreateBucket(this IKS3 ks3, string bucketName)
        {
            return ks3.CreateBucket(new CreateBucketRequest(bucketName));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="acl"></param>
        public static void SetBucketAcl(this IKS3 ks3, string bucketName, AccessControlList acl)
        {
            ks3.SetBucketAcl(new SetBucketAclRequest(bucketName, acl));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified KS3 bucket.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="cannedAcl"></param>
        public static void SetBucketAcl(this IKS3 ks3, string bucketName, CannedAccessControlList cannedAcl)
        {
            ks3.SetBucketAcl(new SetBucketAclRequest(bucketName, cannedAcl));
        }

        /// <summary>
        ///  Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public static ObjectListing ListObjects(this IKS3 ks3, string bucketName)
        {
            return ks3.ListObjects(new ListObjectsRequest(bucketName, null, null, null, null));
        }

        /// <summary>
        /// Returns a list of summary information about the objects in the specified bucket.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static ObjectListing ListObjects(this IKS3 ks3, string bucketName, string prefix)
        {
            return ks3.ListObjects(new ListObjectsRequest(bucketName, prefix, null, null, null));
        }

        /// <summary>
        /// Deletes the specified object in the specified bucket.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        public static void DeleteObject(this IKS3 ks3, string bucketName, string key)
        {
            ks3.DeleteObject(new DeleteObjectRequest(bucketName, key));
        }

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KS3Object GetObject(this IKS3 ks3, string bucketName, string key)
        {
            return ks3.GetObject(new GetObjectRequest(bucketName, key));
        }

        /// <summary>
        /// Gets the object stored in KS3 under the specified bucket and key, and saves the object contents to the specified file.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="destinationFile"></param>
        /// <returns></returns>
        public static KS3Object GetObject(this IKS3 ks3, string bucketName, string key, FileInfo destinationFile)
        {
            return ks3.GetObject(new GetObjectRequest(bucketName, key, destinationFile));
        }

        /// <summary>
        ///  Gets the metadata for the specified KS3 object without actually fetching the object itself.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ObjectMetadata GetObjectMetadata(this IKS3 ks3, string bucketName, string key)
        {
            return ks3.GetObjectMetadata(new GetObjectMetadataRequest(bucketName, key));
        }

        /// <summary>
        /// Uploads the specified file to KS3 under the specified bucket and key name.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static PutObjectResult PutObject(this IKS3 ks3, string bucketName, string key, FileInfo file)
        {
            var putObjectRequest = new PutObjectRequest(bucketName, key, file)
            {
                Metadata = new ObjectMetadata()
            };
            return ks3.PutObject(putObjectRequest);
        }

        /// <summary>
        /// Uploads the specified input stream and object metadata to KS3 under the specified bucket and key name.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="input"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static PutObjectResult PutObject(this IKS3 ks3, string bucketName, string key, Stream input, ObjectMetadata metadata)
        {
            return ks3.PutObject(new PutObjectRequest(bucketName, key, input, metadata));
        }

        /// <summary>
        /// init multi upload big file
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketname"></param>
        /// <param name="objectkey"></param>
        /// <returns></returns>
        public static InitiateMultipartUploadResult InitiateMultipartUpload(IKS3 ks3, string bucketname, string objectkey)
        {
            return ks3.InitiateMultipartUpload(new InitiateMultipartUploadRequest(bucketname, objectkey));
        }

        /// <summary>
        /// Gets the AccessControlList (ACL) for the specified object in KS3.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static AccessControlList GetObjectAcl(this IKS3 ks3, string bucketName, string key)
        {
            return ks3.GetObjectAcl(new GetObjectAclRequest(bucketName, key));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="acl"></param>
        public static void SetObjectAcl(this IKS3 ks3, string bucketName, string key, AccessControlList acl)
        {
            ks3.SetObjectAcl(new SetObjectAclRequest(bucketName, key, acl));
        }

        /// <summary>
        /// Sets the AccessControlList for the specified object in KS3.
        /// </summary>
        /// <param name="ks3"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        /// <param name="cannedAcl"></param>
        public static void SetObjectAcl(this IKS3 ks3, string bucketName, string key, CannedAccessControlList cannedAcl)
        {
            ks3.SetObjectAcl(new SetObjectAclRequest(bucketName, key, cannedAcl));
        }

    }
}
