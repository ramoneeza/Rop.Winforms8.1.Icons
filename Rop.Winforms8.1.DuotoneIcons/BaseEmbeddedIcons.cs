using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rop.Winforms8.DuotoneIcons;

/// <summary>
/// Base class for embedded icons
/// </summary>
public abstract class BaseEmbeddedIcons : IEmbeddedIcons
{
    private readonly FrozenDictionary<string, DuoToneIcon> _charSet;
    public IReadOnlyList<string> Codes { get; }
    public IReadOnlyList<string> Aliases { get; }
    public string FontName { get; }
    public Size BaseSize { get; }
    public int Count => _charSet.Count;
    public DuoToneIcon? GetIcon(string name) => _charSet.GetValueOrDefault(name);
    public float DrawIcon(Graphics gr,string code,DuoToneColor iconcolor, float x, float y, float height)
    {
        return  GetIcon(code)?.DrawIcon(gr,iconcolor,x,y,height)??0;
    }
    public void DrawIconFit(Graphics gr,string code,DuoToneColor iconcolor, float x, float y, float size)
    {
        GetIcon(code)?.DrawIconFit(gr,iconcolor,x,y,size);
    }
    public float DrawIconBaseLine(Graphics gr, string code, DuoToneColor iconcolor, float x, float baseline, float height)
    {
       return GetIcon(code)?.DrawIconBaseline(gr, iconcolor, x,baseline,height)??0;
    }
    private record BankData
    {
        public string Name { get; init; } = "";
        public string Initials { get; init; } = "";
        public Size BaseSize { get; init; } = new Size(0, 0);
        public IconData[] Icons { get; init; } = [];
    }

    private record IconData
    {
        public string Code { get; init; } = "";
        public int Width { get; init; }
        public int Height { get; init; }
        public int BaseLine { get; init; } = 0;
        public byte[] Data { get; init; } = Array.Empty<byte>();
        public string[] Alias { get; init; } = Array.Empty<string>();
    }
    
    
    protected BaseEmbeddedIcons(string resourcename)
    {
        var jsonbankdata = GetZipResource(this.GetType(),resourcename);
        if (jsonbankdata == null) throw new ArgumentException($"Resource {resourcename} not found");
        // decodifica jsoncodes que es un string json que representa un array de baseicon
        var bankData = JsonSerializer.Deserialize<BankData>(jsonbankdata);
        if (bankData == null) throw new ArgumentException($"Resource {resourcename} not deserialized");
        FontName = bankData.Name;
        var charSet = new Dictionary<string, DuoToneIcon>(StringComparer.OrdinalIgnoreCase);
        var alias = new List<string>();
        var codes = new List<string>();
        BaseSize = bankData.BaseSize;
        foreach (var resourceIcon in bankData.Icons)
        {
            var sz = new Size(resourceIcon.Width, resourceIcon.Height);
            var icon = FactoryDuoToneIcon(resourceIcon.Code,sz,resourceIcon.BaseLine,resourceIcon.Data);
            charSet[resourceIcon.Code] = icon;
            alias.AddRange(resourceIcon.Alias);
            codes.Add(resourceIcon.Code);
            foreach (var a in resourceIcon.Alias)
            {
                charSet[a] = icon;
            }
        }
        _charSet = charSet.ToFrozenDictionary();
        Codes = codes.OrderBy(i=>i).ToList();
        Aliases = alias.OrderBy(a=>a).ToList();
    }
    protected DuoToneIcon FactoryDuoToneIcon(string code,Size size,int baseline,byte[] data)
    {
        return new DuoToneIcon(code,size,baseline,data);
    }

    private static Stream? _getResourceStream(Type t, string name)
    {
        var assembly = t.Assembly;
        var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(str => str.EndsWith(name, StringComparison.OrdinalIgnoreCase));
        return resourceName == null ? null : assembly.GetManifestResourceStream(resourceName);
    }

    public static string? GetZipResource(Type t, string name)
    {
        using var compressedstream = _getResourceStream(t, name);
        if (compressedstream is null) return null;
        using var gzipStream = new GZipStream(compressedstream, CompressionMode.Decompress);
        using var reader = new StreamReader(gzipStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }
    public static Bitmap? GetResourceBitmap(Type t, string name)
    {
        using var stream = _getResourceStream(t, name);
        if (stream is null) return null;
        return new Bitmap(stream);
    }

}