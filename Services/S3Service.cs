using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using MusicApp.Settings;

namespace MusicApp.Services
{
    public class S3Service
    {
        private readonly AmazonS3Client _client;
        private readonly AwsSettings _settings;

        public S3Service(IConfiguration config)
        {
            _settings = config.GetSection("AWS").Get<AwsSettings>()!;
            _client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, RegionEndpoint.GetBySystemName(_settings.Region));


        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var key = Guid.NewGuid().ToString() + "_" + file.FileName;
            using var stream = file.OpenReadStream();
            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType
            };

            await _client.PutObjectAsync(putRequest);
            return $"{key}";
        }
        public string GetFileUrl(string key)
        {
            return $"https://{_settings.BucketName}.s3.{_settings.Region}.amazonaws.com/{key}";
        }

        public async Task DeleteFileAsync(string key)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key
            };
            await _client.DeleteObjectAsync(deleteRequest);
        }
    }
}
