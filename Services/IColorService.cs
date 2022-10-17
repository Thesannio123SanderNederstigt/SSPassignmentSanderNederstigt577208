using SixLabors.ImageSharp.PixelFormats;

namespace Services;

public interface IColorService
{
    public Task<string> GetColorName(Rgba32 color);
}
