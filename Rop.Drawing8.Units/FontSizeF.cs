using System.ComponentModel;
using System.Drawing;

namespace Rop.Drawing8.Units;

public record struct FontSizeF(float Width, float Height,float Ascent)
{
    public static readonly FontSizeF Empty=default;
    public FontSizeF(FontSizeF size) : this(size.Width, size.Height, size.Ascent)
    {
    }
    public FontSizeF(SizeF size,float ascent):this(size.Width,size.Height,ascent)
    {
    }
    public static FontSizeF FromWAD(float width,float ascent,float descent)
    {
        if (ascent<0) ascent = 0;
        if (descent<0) descent = 0;
        return new FontSizeF(width, ascent + descent, ascent);
    }
    public SizeF ToSizeHeight() => new SizeF(this.Width, this.Height);
    public SizeF ToSizeAscent() => new SizeF(this.Width, this.Ascent);
    public readonly FontSizeF ToFontSizeUnit() => new FontSizeF(WidthUnit, 1,AscentUnit);
    public static FontSizeF operator +(FontSizeF sz1, FontSizeF sz2) => Add(sz1, sz2);
    public static FontSizeF operator *(float left, FontSizeF right) => Multiply(right, left);
    public static FontSizeF operator *(FontSizeF left, float right) => Multiply(left, right);
    public static FontSizeF operator /(FontSizeF left, float right) => new FontSizeF(left.Width / right, left.Height / right,left.Ascent/right);

    [Browsable(false)]
    public readonly bool IsEmpty => this.Equals(Empty);
    [Browsable(false)]
    public float Descent
    {
        readonly get => this.Height-this.Ascent;
        set => this.Height = this.Ascent + value;
    }
    public readonly float AscentUnit => this.Ascent / this.Height;
    public readonly float DescentUnit => Descent / this.Height;
    public readonly float WidthUnit => this.Width / this.Height;
    
    public static FontSizeF Mix(FontSizeF sz1, FontSizeF sz2)
    {
        var maxascent = Math.Max(sz1.Ascent, sz2.Ascent);
        var maxdescent=Math.Max(sz1.Descent, sz2.Descent);
        var maxwidth=Math.Max(sz1.Width, sz2.Width);
        return FontSizeF.FromWAD(maxwidth, maxascent, maxdescent);
    }
    public static FontSizeF Add(FontSizeF sz1, FontSizeF sz2)
    {
        var maxascent = Math.Max(sz1.Ascent, sz2.Ascent);
        var maxdescent=Math.Max(sz1.Descent, sz2.Descent);
        return FontSizeF.FromWAD(sz1.Width+sz2.Width, maxascent, maxdescent);
    }
    public static FontSizeF Mix(params FontSizeF[] sizes)
    {
        if (sizes.Length == 0) return Empty;
        var maxascent = sizes.Max(s=>s.Ascent);
        var maxdescent=sizes.Max(s=>s.Descent);
        var maxwidth=sizes.Max(s => s.Width);
        return FontSizeF.FromWAD(maxwidth, maxascent, maxdescent);
    }
    public static FontSizeF Add(params FontSizeF[] sizes)
    {
        if (sizes.Length == 0) return Empty;
        var maxascent = sizes.Max(s => s.Ascent);
        var maxdescent=sizes.Max(s => s.Descent);
        var totwidth=sizes.Sum(s => s.Width);
        return FontSizeF.FromWAD(totwidth, maxascent, maxdescent);
    }
    public static FontSizeF Mix(IEnumerable<FontSizeF> sizes)=>Mix(sizes.ToArray());
    public static FontSizeF Add(IEnumerable<FontSizeF> sizes)=>Add(sizes.ToArray());
    public readonly override string ToString()
    {
        return $"{{Width={this.Width}, Height={this.Height}, Ascent={Ascent}}}";
    }
    public static FontSizeF Multiply(FontSizeF size, float multiplier) => new FontSizeF(size.Width * multiplier, size.Height * multiplier,size.Ascent *multiplier);
}