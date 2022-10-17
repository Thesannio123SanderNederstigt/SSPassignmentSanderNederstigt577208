using SixLabors.ImageSharp.PixelFormats;

namespace Services;

public interface IImageService
{
    public Rgba32 GetDominantColor(byte[] imageBytes);

    public byte[] AddTextToImage(byte[] imageBytes, params (string text, (float x, float y) position, int fontSize, string colorHex)[] texts);
}
