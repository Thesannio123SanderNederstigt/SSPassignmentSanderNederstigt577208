namespace Services;

public interface IDictionaryService
{
    public Task<string> GetDictionaryText(string colorNameWord);
}
