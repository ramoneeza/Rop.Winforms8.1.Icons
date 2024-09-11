using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rop.Winforms8.DuotoneIcons;

public static class IconRepository
{
    private static readonly ConcurrentDictionary<string,IEmbeddedIcons> _loadedIconsByName = new (StringComparer.OrdinalIgnoreCase);
    private static readonly ConcurrentDictionary<RuntimeTypeHandle,IEmbeddedIcons> _loadedIconsByType = new ();
    public static IEmbeddedIcons? DefaultBank { get; private set; }
    public static IEmbeddedIcons GetEmbeddedIcons<T>() where T : IEmbeddedIcons
    {
        var t = typeof(T);
        return GetEmbeddedIcons(t);
    }
    public static IEmbeddedIcons GetEmbeddedIcons(Type t)
    {
        if (_loadedIconsByType.TryGetValue(t.TypeHandle, out var bank)) return bank;
        bank = (IEmbeddedIcons)(Activator.CreateInstance(t)??throw new NullReferenceException());
        _loadedIconsByType[t.TypeHandle] = bank;
        _loadedIconsByName[bank.FontName] = bank;
        if (DefaultBank == null || DefaultBank.FontName!="MaterialDesign") 
            DefaultBank = bank;
        return bank;
    }
    public static IEmbeddedIcons? GetEmbeddedIcons(string name)
    {
        _loadedIconsByName.TryGetValue(name, out var bank);
        return bank;
    }
    public static bool IsLoaded(string name)
    {
        return _loadedIconsByName.ContainsKey(name);
    }

    public static string[] GetRegisteredIcons()
    {
        return _loadedIconsByName.Keys.ToArray();
    }
    static IconRepository()
    {
        var asm=LoadIconsAssemblies();
        
        var basetype = typeof(BaseEmbeddedIcons);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var name = assembly.GetName().Name??"";
            if (!name.Contains(".DuotoneIcons.")) continue;
            var derivedTypes = from type in assembly.GetTypes()
                where type.IsSubclassOf(basetype) && !type.IsAbstract
                select type;

            foreach (var t in derivedTypes)
            {
                GetEmbeddedIcons(t);
            }
        }
    }

    private static List<Assembly> LoadIconsAssemblies()
    {
        var currentAssembly= Assembly.GetEntryAssembly();
        if (currentAssembly == null) return [];
        var referencedAssemblies=currentAssembly.GetReferencedAssemblies();
        var res = new List<Assembly>();
        foreach (var assemblyName in referencedAssemblies)
        {
            if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName == assemblyName.FullName))
            {
                var name2 = assemblyName.Name;
                continue;
            }
            var name= assemblyName.Name??"";
            if (!name.Contains(".DuotoneIcons.")) continue;
            try
            {
                res.Add(Assembly.Load(assemblyName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load assembly {assemblyName.Name}: {ex.Message}");
            }
        }

        return res;
    }
}
