using System.Net;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace JWT_Token_Example.ImageServices;

public class S3Service
{
    private readonly string _bucketName;
            private readonly IAmazonS3 _awsS3Client;

            public S3Service()
            {
                
            }
    
            public S3Service(string awsAccessKeyId, string awsSecretAccessKey, string region, string bucketName)
            {
                _bucketName = bucketName;
                _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.GetBySystemName(region));
            }


            public async Task<bool> UploadFile(IFormFile file)
            {
                try
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        file.CopyTo(newMemoryStream);

                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = "Product/"+file.FileName,
                            BucketName = _bucketName,
                            ContentType = file.ContentType
                        };

                        var fileTransferUtility = new TransferUtility(_awsS3Client);

                        await fileTransferUtility.UploadAsync(uploadRequest);

                        var imageURL = "https://vendor-buck.s3.ap-south-1.amazonaws.com/hello/" + file.FileName;
                        Console.WriteLine(imageURL);
                        return true;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }


            public async Task<byte[]> DownloadFileAsync(string file)
            {
                MemoryStream ms = null;

                try
                {
                    GetObjectRequest getObjectRequest = new GetObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = file
                    };

                    using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
                    {
                        if (response.HttpStatusCode == HttpStatusCode.OK)
                        {
                            using (ms = new MemoryStream())
                            {
                                await response.ResponseStream.CopyToAsync(ms);
                            }
                        }
                    }

                    if (ms is null || ms.ToArray().Length < 1)
                        throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

                    Console.WriteLine(ms.ToArray());
                    return ms.ToArray();
                }
                catch (Exception)
                {
                    throw;
                }

            }
}