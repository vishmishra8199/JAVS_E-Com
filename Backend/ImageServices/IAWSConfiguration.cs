namespace JWT_Token_Example.ImageServices;

public interface IAWSConfiguration
{
    string AwsAccessKey { get; set; }
    string AwsSecretAccessKey { get; set; }
    string BucketName { get; set; }
    string Region { get; set; }
}