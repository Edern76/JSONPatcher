using System.Reflection;

namespace JSONPatchParser.Utils;

public static class AccessUtils
{
    public static T? AccessNonPublicProperty<T>(object obj, string propertyName)
    {
        return (T?)(obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
            ?.GetValue(obj));
    }
}