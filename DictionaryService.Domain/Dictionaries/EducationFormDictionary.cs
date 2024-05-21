using DictionaryService.Domain.Enums;

namespace DictionaryService.Domain.Dictionaries;

public class EducationFormDictionary
{
    private  readonly Dictionary<FormOfEducation, string> _dictionary = new Dictionary<FormOfEducation, string>
    {
        { FormOfEducation.Очная, "Очная" },
        { FormOfEducation.ОчноЗаочная, "Очно-заочная" }
    };
    
    public string GetFormOfEducation(FormOfEducation enumForm)
    {
        if (_dictionary.TryGetValue(enumForm, out var form))
        {
            return form;
        }

        throw new ArgumentException("Invalid language");
    }
}