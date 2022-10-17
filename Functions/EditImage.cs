using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using SixLabors.ImageSharp.PixelFormats;

namespace Functions;

public class EditImageClass
{
    private readonly IImageService imageService;
    private readonly IColorService colorService;
    private readonly IDictionaryService dictionaryService;

    public EditImageClass(IImageService imageService, IColorService colorService, IDictionaryService dictionaryService)
    {
        this.imageService = imageService;
        this.colorService = colorService;
        this.dictionaryService = dictionaryService;
    }

    [FunctionName(nameof(EditImage))]
    public async Task EditImage(
        [QueueTrigger("imagesqueue", Connection = "connString")] string imageName,
        [Blob("imagescontainer", Connection = "connString")] BlobContainerClient blobs,
        [Table("imageStatus", Connection = "connString")] TableClient table,
        ILogger log)
    {
        log.LogInformation("C# Queue trigger editImage function initiated.");

        // retrieve the blob client of the image and create memory stream
        BlobClient blob = blobs.GetBlobClient(imageName);
        MemoryStream stream = new();

        // download the image from blob storage into memorystream
        await blob.DownloadToAsync(stream);

        // create the byte array of the image from the stream and retrieve the dominant color, and the color name and dictionary text from api calls in the service classes/layer
        byte[] imageBytes = stream.ToArray();
        Rgba32 dominantColor = imageService.GetDominantColor(imageBytes);
        string colorName = await colorService.GetColorName(dominantColor);
        string dictionaryText = await dictionaryService.GetDictionaryText(colorName);

        // add the retrieved text to the image
        byte[] editedBytes = imageService.AddTextToImage(imageBytes, (dictionaryText, (0, 0), 20, "#000000"));

        // create a new memory stream from the edited data of the image
        MemoryStream outputStream = new(editedBytes);

        // upload the edited image data in the stream to azure blob storage
        await blob.UploadAsync(outputStream, true);

        // (send a call to) update the image status in the azure table storage table entry to completed (Done in this case)
        await table.UpdateEntityAsync(ImageStatusTableModel.Done("ImagePartition", imageName), ETag.All);
    }
}
