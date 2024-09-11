using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Winforms8.DuotoneIcons
{
    public static class ColorHelper
    {
        private static readonly Lazy<FrozenDictionary<int, Color>> _namedcolordic = new Lazy<FrozenDictionary<int, Color>>(_makeNamedColorDic);
        private static FrozenDictionary<int, Color> _makeNamedColorDic()
        {
            var res = new Dictionary<int, Color>();
            var props = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var prop in props)
            {
                if (prop.PropertyType != typeof(Color)) continue;
                var color = (Color)(prop.GetValue(null) ?? throw new InvalidOperationException("Color cast"));
                if (!res.ContainsKey(color.ToArgb()))
                {
                    res.Add(color.ToArgb(), color);
                }
            }
            props=typeof(SystemColors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var prop in props)
            {
                if (prop.PropertyType != typeof(Color)) continue;
                var color = (Color)(prop.GetValue(null) ?? throw new InvalidOperationException("Color cast"));
                if (!res.ContainsKey(color.ToArgb()))
                {
                    res.Add(color.ToArgb(), color);
                }
            }
            // devolver frozen dictionary desde res
            return res.ToFrozenDictionary();
        }

        public static Color SearchNamedColor(Color c)
        {
            return _namedcolordic.Value.GetValueOrDefault(c.ToArgb(), c);
        }
        public static Color SearchNamedColor(int r,int g,int b)
        {
            var c= Color.FromArgb(r, g, b);
            return SearchNamedColor(c);
        }
        public static Color SearchNamedColor(int a,int r,int g,int b)
        {
            if (a!=255) return Color.FromArgb(a, r, g, b);
            var c= Color.FromArgb(r, g, b);
            return SearchNamedColor(c);
        }
        public static Color ColorFromString(string s)
        {
            if (s == "Empty") return Color.Empty;
            if (s == "Transparent") return Color.Transparent;
            if (!s.StartsWith("#")) return Color.FromName(s);
            var c = s[1..];
            int a;
            int r;
            int g;
            int b;
            if (c.Length == 6)
            {
                a = 255;
                r = int.Parse(c[0..2], NumberStyles.HexNumber);
                g = int.Parse(c[2..4], NumberStyles.HexNumber);
                b = int.Parse(c[4..6], NumberStyles.HexNumber);
            }
            else
            {
                a = int.Parse(c[0..2], NumberStyles.HexNumber);
                r = int.Parse(c[2..4], NumberStyles.HexNumber);
                g = int.Parse(c[4..6], NumberStyles.HexNumber);
                b = int.Parse(c[6..8], NumberStyles.HexNumber);
            }
            return ColorHelper.SearchNamedColor(a,r,g,b);
        }
        public static string ColorToString(Color color)
        {
            if (color == Color.Empty) return "Empty";
            if (color == Color.Transparent) return "Transparent";
            if (color.IsNamedColor) return color.Name;
            if (color.A == 255)
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            else
                return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
