using Amazon.S3;
using Amazon.S3.Model;
using Constriva.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constriva.Infrastructure.Services
{
    public class S3StorageService : IFileStorageService
    {
        private readonly IConfiguration _config;
        private readonly IAmazonS3 _s3;

        public S3StorageService(IConfiguration config)
        {
            _config = config;
            var s3Config = new Amazon.S3.AmazonS3Config { ServiceURL = config["Storage:Endpoint"] };
            _s3 = new AmazonS3Client(config["Storage:AccessKey"], config["Storage:SecretKey"], s3Config);
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, string folder = "", CancellationToken ct = default)
        {
            var key = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";
            var request = new PutObjectRequest
            {
                BucketName = _config["Storage:Bucket"],
                Key = key,
                InputStream = stream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };
            await _s3.PutObjectAsync(request, ct);
            return GetPublicUrl(key);
        }

        public async Task DeleteAsync(string url, CancellationToken ct = default)
        {
            var key = url.Replace($"{_config["Storage:PublicUrl"]}/", "");
            await _s3.DeleteObjectAsync(_config["Storage:Bucket"], key, ct);
        }

        public async Task<Stream> DownloadAsync(string url, CancellationToken ct = default)
        {
            var key = url.Replace($"{_config["Storage:PublicUrl"]}/", "");
            var response = await _s3.GetObjectAsync(_config["Storage:Bucket"], key, ct);
            return response.ResponseStream;
        }

        public string GetPublicUrl(string path) => $"{_config["Storage:PublicUrl"]}/{path}";
    }
}
