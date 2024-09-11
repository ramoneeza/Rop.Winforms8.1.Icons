using Rop.Drawing8.Units;

namespace Rop.Winforms8.DuotoneIcons;

public static class FontHelper
{

    public static int PointsToPixels(float points, float dpi) => (int)(points * (dpi / 72f));
    public static int PointsToPixels(this Graphics g, float points) => PointsToPixels(points, g.DpiY);

    public static float GetAscentPoints(this Font f)
    {
        var a = GetAscentUnit(f);
        return a * f.SizeInPoints;
    }

    public static float GetAscentUnit(this Font f)
    {
        float heightEm = f.FontFamily.GetEmHeight(f.Style);
        float ascent = f.FontFamily.GetCellAscent(f.Style);
        //float descent=f.FontFamily.GetCellDescent(f.Style);
        //float line = f.FontFamily.GetLineSpacing(f.Style);
        return ascent / heightEm;
    }

    public static int GetAscentPixels(this Font f, float dpi)
    {
        var a = GetAscentUnit(f);
        return (int)(a * PointsToPixels(f.SizeInPoints, dpi));
    }

    public static int GetAscentPixels(this Font f, Graphics gr)
    {
        return GetAscentPixels(f, gr.DpiY);

    }

    public static FontSizeF MeasureTextSizeWithAscent(this Font font, Graphics gr, string text)
    {
        var sz = gr.MeasureString(text, font,PointF.Empty, StringFormat.GenericTypographic);
        var a = font.GetAscentPixels(gr);
        return new FontSizeF(sz, a);
    }

    public static FontRectangleF MeasureStringWithBaseLine(this Font font, Graphics gr, PointF baseline, string text)
    {
        var sz = MeasureTextSizeWithAscent(font, gr, text);
        return FontRectangleF.FromBaseLine(baseline.X, baseline.Y, sz.Width, sz.Height, sz.Ascent);
    }
}