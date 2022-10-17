using Models;
using Newtonsoft.Json;

namespace Services;

public class DictionaryService : IDictionaryService
{
    private readonly HttpClient httpClient;

    public DictionaryService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient();
    }

    public async Task<string> GetDictionaryText(string colorNameWord)
    {
        //retrieve a text related to the color name word and process the result as json
        HttpResponseMessage response = await httpClient.GetAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{colorNameWord}");
        string json = await response.Content.ReadAsStringAsync();
        DictionaryApi[]? result = JsonConvert.DeserializeObject<DictionaryApi[]>(json);

        //select a random meaning along with a random definition from the list of definitions within the meaning and return either the definition or an example if available
        switch (result)
        {
            case null: throw new Exception("an invalid or non-existent json result was retrieved from the dictionary api");

            case DictionaryApi[] res:
                    DictionaryApi entry = res[Random.Shared.Next(0, res.Length)];
                    DictionaryMeaning meaning = entry.Meanings[Random.Shared.Next(0, entry.Meanings.Length)];
                    DictionaryDefinition definition = meaning.Definitions[Random.Shared.Next(0, meaning.Definitions.Length)];
                    return definition.Example ?? definition.Definition;

            default: throw new Exception("something went wrong and some unreachable code was reached (check the retrieved api entry)");
        };
    }
}
