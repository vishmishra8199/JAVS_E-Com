namespace JWT_Token_Example.ImageServices;

public class AWSConfiguration : IAWSConfiguration
{
    public string AwsAccessKey { get; set; }
    public string AwsSecretAccessKey { get; set; }
    public string BucketName { get; set; }
    public string Region { get; set; }
}