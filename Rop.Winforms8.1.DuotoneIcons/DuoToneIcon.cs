using System.Drawing.Drawing2D;
using Rop.Drawing8.Units;

namespace Rop.Winforms8.DuotoneIcons;

public class DuoToneIcon
{
    public string Name { get; }
    private readonly byte[] _data;
    public IReadOnlyCollection<byte> Data=> _data;
    public Size Size { get; }
    public int BaseLine { get; }
    public FontSizeF FontSizeUnit=>new FontSizeF(WidthUnit,1,AscentUnit);
    public FontSizeF FontSizeUnitExtended => new FontSizeF(WidthUnit, 1,1);
    public float WidthUnit { get; } 
    public float AscentUnit { get; } 
    public float DescentUnit=>1-AscentUnit;
    private Bitmap? _bitmap;
    public Bitmap GetBaseBitmap()
    {
        if (_bitmap == null)
        {
            _bitmap = Converter.ToBmp4(_data, Size);
        }
        return _bitmap;
    }
    public Bitmap GetBitmap(DuoToneColor color)
    {
        var bmp = GetBaseBitmap();
        var nb = (Bitmap)bmp.Clone();
        var p = nb.Palette;
        p.Entries[1] = color.Color1;
        p.Entries[2] = color.Color2;
        nb.Palette = p;
        return nb;
    }
    public FontSizeF MeasureIcon(float height)
    {
        return FontSizeUnit* height;
    }
    public FontRectangleF MeasureIconRect(PointF location, float height)
    {
        var m=MeasureIcon(height);
        return new FontRectangleF(location.X,location.Y,m.Width,m.Height,m.Ascent);
    }
    public FontRectangleF MeasureIconRectBaseline(PointF location, float height)
    {
        var m=MeasureIcon(height);
        return FontRectangleF.FromBaseLine(location,m);
    }
    public FontSizeF MeasureIconWithAscent(Graphics g, Font font,float scale)
    {
        var f = font.MeasureTextSizeWithAscent(g, "A");
        var m=MeasureIcon(f.Ascent*scale);
        return m;
    }
    
    public float DrawIcon(Graphics gr, DuoToneColor iconcolor, RectangleF rect)
    {
        using var bmp = GetBitmap(iconcolor);
        var a = gr.InterpolationMode;
        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
        gr.DrawImage(bmp,rect);
        gr.InterpolationMode = a;
        return rect.Width;
    }
    public float DrawIcon(Graphics gr,DuoToneColor iconcolor, float x, float y, float height)
    {
        var m = MeasureIcon(height);
        DrawIcon(gr,iconcolor, new RectangleF(x, y, m.Width,m.Height));
        return m.Width;
    }
    public float DrawIcon(Graphics gr,DuoToneColor iconcolor, float x, float y,FontSizeF iconsize)
    {
        DrawIcon(gr,iconcolor, new FontRectangleF(new PointF(x, y),iconsize));
        return iconsize.Width;
    }
    public float DrawIcon(Graphics gr,DuoToneColor iconcolor,FontRectangleF iconsize)
    {
        DrawIcon(gr,iconcolor, new RectangleF(iconsize.X,iconsize.Y,iconsize.Width,iconsize.Height));
        return iconsize.Width;
    }
    public float DrawIconBaseline(Graphics gr,DuoToneColor iconcolor, float x, float baseline, float height)
    {
        var m=MeasureIcon(height);
        var y= baseline - m.Ascent;
        DrawIcon(gr,iconcolor, new RectangleF(x, y, m.Width,m.Height));
        return m.Width;
    }
    public float DrawIconBaseline(Graphics gr,DuoToneColor iconcolor, float x, float baseline,FontSizeF iconsize)
    {
        var y= baseline - iconsize.Ascent;
        DrawIcon(gr,iconcolor, new RectangleF(x, y, iconsize.Width,iconsize.Height));
        return iconsize.Width;
    }
    public float DrawIconBaseline(Graphics gr,DuoToneColor iconcolor,float x, float baseline, IconSizeF iconSize)
    {
        return DrawIconBaseline(gr, iconcolor, x, baseline, iconSize.Size);
    }
    

    public float DrawIconFit(Graphics gr, DuoToneColor iconcolor, float x, float y, float size)
    {
        FontSizeF m;
        if (WidthUnit > 1)
        {
            var height = size / WidthUnit;
            m=FontSizeUnitExtended*height;
        }
        else
        {
            m=MeasureIcon(size);
        }
        y=y+(size-m.Ascent)/2;
        DrawIcon(gr, iconcolor,new RectangleF(x, y, m.Width,m.Height));
        return m.Width;
    }
    public DuoToneIcon(string name,Size size,int baseline,byte[] data)
    {
        //Container = container;
        Name = name;
        _data = data;
        Size = size;
        BaseLine = baseline;
        WidthUnit = (float)size.Width / (float)size.Height;
        AscentUnit = baseline/(float)size.Height;
        _bitmap = null;
    }
}