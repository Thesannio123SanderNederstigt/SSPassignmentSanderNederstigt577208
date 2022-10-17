namespace Models;

// This is a json object returned from https://api.dictionaryapi.dev/api/v2/entries/en
// This is a cut down version of a lot more info returned from this api endpoint
public struct DictionaryApi
{
    public DictionaryMeaning[] Meanings { get; set; }
}

public struct DictionaryMeaning
{
    public DictionaryDefinition[] Definitions { get; set; }
}

public struct DictionaryDefinition
{
    public string Definition { get; set; }
    public string? Example { get; set; }
}
