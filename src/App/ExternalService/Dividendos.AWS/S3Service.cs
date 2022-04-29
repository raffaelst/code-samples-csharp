using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Dividendos.AWS
{
    public class S3Service : IS3Service
    {
        private readonly AWSConfig _aWSConfig;
        private readonly IAmazonS3 _amazonS3;

        public S3Service(IAmazonS3 amazonS3, IOptions<AWSConfig> aWSConfig)
        {
            _amazonS3 = amazonS3;
            _aWSConfig = aWSConfig.Value;
        }

        public string GenerateURL(string key)
        {
            string baseUrl = _aWSConfig.AlternateDomainName;

            return $"{baseUrl}/{key}";
        }

        public async Task<string> PutImage(byte[] file, string fileName)
        {
            using (var stream = new MemoryStream(file))
            {
                var request = new PutObjectRequest
                {
                    BucketName = _aWSConfig.BucketName,
                    InputStream = stream,
                    ContentType = "image/jpeg",
                    Key = fileName,
                    CannedACL = S3CannedACL.PublicRead
                };

                await _amazonS3.PutObjectAsync(request);
            }

            return this.GenerateURL(fileName);
        }

        public async Task<string> PutPDF(byte[] file, string fileName)
        {
            using (var stream = new MemoryStream(file))
            {
                var request = new PutObjectRequest
                {
                    BucketName = _aWSConfig.BucketName,
                    InputStream = stream,
                    ContentType = "application/pdf",
                    Key = fileName,
                    CannedACL = S3CannedACL.PublicRead
                };

                await _amazonS3.PutObjectAsync(request);
            }

            return this.GenerateURL(fileName);
        }

        //public async Task<string> GetXml(string key)
        //{
        //    string resultService = null;

        //    try
        //    {
        //        GetObjectResponse getResponse = await _amazonS3.GetObjectAsync(_aWSConfig.BucketNameLogXml, key);

        //        if (getResponse != null)
        //        {
        //            StreamReader reader = new StreamReader(getResponse.ResponseStream);

        //            resultService = reader.ReadToEnd();
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return resultService;
        //}
    }
}
