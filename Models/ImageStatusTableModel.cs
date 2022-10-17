using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models;
public class ImageStatusTableModel : ITableEntity
{
    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public ImageStatusType Status { get; set; }

    public static ImageStatusTableModel Processing(string partitionKey, string rowKey)
    {
        return new ImageStatusTableModel()
        {
            PartitionKey = partitionKey,
            RowKey = rowKey,
            Timestamp = DateTimeOffset.UtcNow,
            Status = ImageStatusType.InProgress,

        };
    }

    public static ImageStatusTableModel Done(string partitionKey, string rowKey)
    {
        return new ImageStatusTableModel()
        {
            PartitionKey = partitionKey,
            RowKey = rowKey,
            Status = ImageStatusType.Done,
        };
    }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum ImageStatusType
{
    InProgress,
    Done
}
