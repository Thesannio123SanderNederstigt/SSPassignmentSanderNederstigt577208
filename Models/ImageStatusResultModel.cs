namespace Models;

public class ImageStatusResultModel
{
    public ImageStatusType Status { get; set; }

    public ImageStatusResultModel(ImageStatusType status)
    {
        Status = status;
    }
}
