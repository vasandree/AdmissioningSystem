using DictionaryService.Domain.Enums;

namespace DictionaryService.Domain.Dictionaries;

public class LanguageDictionary
{
    private  readonly Dictionary<Language, string> _languageStrings = new Dictionary<Language, string>()
    {
        { Language.English, "English"},
        { Language.Русский , "Русский"}
    };
    
    public string GetLanguage(Language languageEnum)
    {
        if (_languageStrings.TryGetValue(languageEnum, out var language))
        {
            return language;
        }

        throw new ArgumentException("Invalid language");
    }
}