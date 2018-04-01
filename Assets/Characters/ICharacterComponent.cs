using System;
using System.Collections.Generic;
using System.Linq;

public interface ICharacterComponent
{
    bool Run();
    int Priority { get; set; }
}

static class InterfaceSearch
{
    public static IEnumerable<Type> GetTypesWithThisInterface<T>()
    {
        Type type = typeof(T);
        return
            AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
    }
}