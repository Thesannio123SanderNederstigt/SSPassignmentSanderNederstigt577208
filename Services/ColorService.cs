using Models;
using Newtonsoft.Json;
using SixLabors.ImageSharp.PixelFormats;

namespace Services;

public class ColorService : IColorService
{
    private readonly HttpClient httpClient;

    public ColorService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> GetColorName(Rgba32 color)
    {
        //call to thecolorapi and retrieve a name from the given Rgba32 dominant image pixels color
        HttpResponseMessage response = await httpClient.GetAsync($"https://www.thecolorapi.com/id?rgb={color.R},{color.G},{color.B}&format=json");
        string json = await response.Content.ReadAsStringAsync();
        ColorApi? result = JsonConvert.DeserializeObject<ColorApi>(json);

        return result switch
        {
            null => throw new Exception("an invalid or non-existent json result was retrieved from the color api"),
            ColorApi res => res.Name.Value.Split(' ')[Random.Shared.Next(0, res.Name.Value.Split().Length)],
        };
        //note: test this last line with only one word being given back from the color api (see if splitting that into an array still works or not)
    }
}
