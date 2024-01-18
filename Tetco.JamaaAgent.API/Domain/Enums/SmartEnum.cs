using System.Reflection;

namespace Domain.Enums;

public abstract record SmartEnum<TEnum>(string key,int value)
    where TEnum : SmartEnum<TEnum>
{
    public static Dictionary<string, TEnum> GetAll()
    {
        var enumType = typeof(TEnum);
        var fieldTypes = enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

        return fieldTypes.Where(x => x.value != 0).ToDictionary(x => x.key);
    }
    public static TEnum GetByKey(string key)
    => GetAll()[key];

    public static IEnumerable<string> GetKeys()
        => GetAll().Select(x=>x.Key);
    public static string CommaSeperatedkeys()
    {
        return $"{string.Join(",",GetKeys())}";
    }

    public static implicit operator int (SmartEnum<TEnum> e)
        => e.value;

    public static implicit operator string(SmartEnum<TEnum> e)
        => e.key;

    public static implicit operator SmartEnum<TEnum>(string key)
        => GetAll()[key];
}
