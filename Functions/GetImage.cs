using System;
using System.Net;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Functions;

public static class GetImageClass
{
    [FunctionName(nameof(GetImage))]
    [OpenApiOperation(nameof(GetImage), tags: new[] { "images" })]
    [OpenApiParameter("imageId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The image identifier (guid)")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Redirect)]
    public static IActionResult GetImage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images/{imageId}")] HttpRequest req, string imageId,
        [Blob(blobPath: "imagescontainer/{imageId}", Connection = "connString")] BlobClient blob,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger GetImage function processed an image retrieval request.");

        // create a blob Shared Access Signature so the image can be requested for about an hour
        BlobSasBuilder builder = new()
        {
            StartsOn = DateTime.UtcNow.AddMinutes(-5),
            ExpiresOn = DateTime.UtcNow.AddHours(1),
            BlobContainerName = blob.BlobContainerName,
            BlobName = blob.Name,
            ContentType = "image/png",
        };

        // set the permissions for the access to read only
        builder.SetPermissions(BlobAccountSasPermissions.Read);

        // create a credential to get access to the storage account shared blob file(s)
        StorageSharedKeyCredential sasKey = new(
            Environment.GetEnvironmentVariable("StorageAccountName", EnvironmentVariableTarget.Process),
            Environment.GetEnvironmentVariable("StorageAccountKey", EnvironmentVariableTarget.Process)
            );

        // create and apply the blob sas query params using the credentials and build an accessable url to get access to the image
        BlobSasQueryParameters sas = builder.ToSasQueryParameters(sasKey);
        string url = $"{blob.Uri}?{sas}";

        // return the new redirection url to the image file in blob storage
        return new RedirectResult(url);
    }
}
