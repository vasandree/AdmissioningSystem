using DictionaryService.Domain.Enums;

namespace DictionaryService.Domain;

public static class StringsDictionary
{
    public static readonly Dictionary<FormOfEducation, string> FormOfEducationStrings = new Dictionary<FormOfEducation, string>
    {
        { FormOfEducation.Очная, "Очная" },
        { FormOfEducation.ОчноЗаочная, "Очно-заочная" }
    };

    public static readonly Dictionary<Language, string> LanguageStrings = new Dictionary<Language, string>()
    {
        { Language.English, "English"},
        { Language.Русский , "Русский"}
    };
}