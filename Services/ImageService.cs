using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace Services;

public class ImageService : IImageService
{
    public Rgba32 GetDominantColor(byte[] imageBytes)
    {
        using Image<Rgba32> image = Image.Load<Rgba32>(imageBytes);

        //size the image down to try and increase method performance
        image.Mutate(i => i.Resize(new ResizeOptions { Sampler = KnownResamplers.NearestNeighbor, Size = new Size(100, 0) }));

        int red = 0;
        int green = 0;
        int blue = 0;
        int pixelTotal = 0;

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                Rgba32 pixel = image[x, y];

                red += Convert.ToInt32(pixel.R);
                green += Convert.ToInt32(pixel.G);
                blue += Convert.ToInt32(pixel.B);

                pixelTotal++;
            }
        }

        red /= pixelTotal;
        green /= pixelTotal;
        blue /= pixelTotal;

        //return Rgba32 color
        return new((byte)red, (byte)green, (byte)blue, 255);
    }

    public byte[] AddTextToImage(byte[] imageBytes, params (string text, (float x, float y) position, int fontSize, string colorHex)[] texts)
    {
        MemoryStream memoryStream = new MemoryStream();

        Image image = Image.Load(imageBytes);

        image.Clone(img =>
        {
            foreach (var (text, (x, y), fontSize, colorHex) in texts)
            {
                Font font = SystemFonts.CreateFont("Verdana", fontSize);
                Rgba32 color = Rgba32.ParseHex(colorHex);

                img.DrawText(text, font, color, new PointF(x, y));
            }
        })
        .SaveAsPng(memoryStream);

        return memoryStream.ToArray();
    }
}