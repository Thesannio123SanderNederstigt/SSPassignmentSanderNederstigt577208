using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;

namespace Functions
{
    public static class UploadImageClass
    {
        [FunctionName(nameof(UploadImage))]
        [OpenApiOperation(operationId: nameof(UploadImage), tags: new[] { "images" })]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(ImageFormRequestBody), Required = true, Description = "A signle png image to upload as data")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ImageFormResponseOk), Description = "The OK response along with the id of the image.")]
        [OpenApiResponseWithBody(HttpStatusCode.BadRequest, "application/json", typeof(ImageFormResponseError), Description = "The response containing an error message returned from a bad request ")]
        public static async Task<IActionResult> UploadImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "images/upload")] HttpRequest req,
            [Blob("imagescontainer", Connection = "connString")] BlobContainerClient blobs,
            [Queue("imagesqueue", Connection = "connString")] QueueClient queue,
            [Table("imageStatus", Connection = "connString")] TableClient table,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger uploadImage function processed a request.");

            // check the required form data "uploadedFile" key and check the content of the uploaded images as value
            IFormFile file = req.Form.Files["uploadedFile"];

            if (file.ContentType != "image/png" || file == null)
            {
                return new BadRequestObjectResult( new ImageFormResponseError { Message = "please upload a png type image file" });
            }

            // generate a globally unique identifier (guid) as a new image id
            string imageId = Guid.NewGuid().ToString();

            // create the blob client
            BlobClient blob = blobs.GetBlobClient(imageId);

            // upload image to a blob in the container
            await blob.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders { ContentType = file.ContentType });

            // write the image status to table storage using the specific image status table model class method for writing the in progress status implementing the ITableEntity interface
            await table.AddEntityAsync(ImageStatusTableModel.Processing("ImagePartition", imageId));

            // send a message with the image id to the queue (which should be picked up by and employ the queuetrigger in the editImage class to start editing the uploaded image)
            await queue.SendMessageAsync(imageId);

            // return succesfull result message along with the created image id
            return new OkObjectResult(new ImageFormResponseOk { ImageId = imageId });
        }
    }
}

