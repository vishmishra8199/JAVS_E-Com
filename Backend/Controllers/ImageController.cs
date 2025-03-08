using JWT_Token_Example.ImageServices;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    // GET: /<controller>/
    private S3Service? S3service;
    private readonly IAWSConfiguration appConfiguration;

    public ImageController(IAWSConfiguration appConfig)
    {

        this.appConfiguration = appConfig;
    }
    
    [HttpPost]
    [Route("Dummy")]
    public async Task<IActionResult> Dummy([FromBody]IFormFile file)
    {
        Console.WriteLine("dajdnakondomaodnandoanodnandka djkla kdnaklndlnakld");
        Console.WriteLine(file.FileName);
        try
        {
            if (file is null || file.Length <= 0)
                return BadRequest("File is empty");
        
        
            S3service = new S3Service(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, appConfiguration.Region, appConfiguration.BucketName);
            var result = S3service.UploadFile(file);
        
            return Ok("File uploaded");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok();
    }


    [HttpPost]
    [Route("uploadImg")]
    public async Task<IActionResult> UploadDocumentToS3([FromBody]IFormFile file)
    {
        // Console.WriteLine(file.FileName);
        Console.WriteLine("dajdnakondomaodnandoanodnandka djkla kdnaklndlnakld");
        // try
        // {
        //     if (file is null || file.Length <= 0)
        //         return BadRequest("File is empty");
        //
        //
        //     S3service = new S3Service(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, appConfiguration.Region, appConfiguration.BucketName);
        //     var result = S3service.UploadFile(file);
        //
        //     return Ok("File uploaded");
        // }
        // catch (Exception ex)
        // {
        //     return BadRequest(ex.Message);
        // }
        return Ok();
    }


    [HttpGet("{folder}/{imageName}")]
    public IActionResult GetDocumentFromS3(string folder, string imageName)
    {
        try
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(imageName))
                return BadRequest("Folder and image name parameters are required");

            S3service = new S3Service(appConfiguration.AwsAccessKey, appConfiguration.AwsSecretAccessKey, appConfiguration.Region, appConfiguration.BucketName);

            // Combine the folder and image name to create the full S3 object key
            var objectKey = $"{folder}/{imageName}";

            var document = S3service.DownloadFileAsync(objectKey).Result;

            return File(document, "application/octet-stream", imageName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}