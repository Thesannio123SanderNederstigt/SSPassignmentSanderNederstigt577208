using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Models;

namespace Functions;

public class ImageStatusClass
{
    [FunctionName(nameof(ImageStatus))]
    [OpenApiOperation(nameof(ImageStatus), tags: new[] { "images" })]
    [OpenApiParameter("imageId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The image identifier (guid)")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(ImageStatusResultModel), Description = "The current status of the image")]
    public static IActionResult ImageStatus(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images/{imageId}/status")] HttpRequest req, string imageId,
        [Table("imageStatus", "ImagePartition", "{imageId}", Connection = "connString")] ImageStatusTableModel status,
        ILogger log)
    {        
        log.LogInformation("C# HTTP trigger ImageStatus function processed an image status request.");

        return new OkObjectResult(new ImageStatusResultModel(status.Status));
    }
}
