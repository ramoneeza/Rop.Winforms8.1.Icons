using Rop.Drawing8.Units;
using Rop.Winforms8.DuotoneIcons.PartialControls;

namespace Rop.Winforms8.DuotoneIcons;

public static class DuoToneIconHelper
{
    public static void DrawStringWithIcons(this Graphics gr, PointF offset, IMeasuredIcon measured, bool test = false)
    {
        if (!measured.Args.HasText)
        {
            _DrawSoloIcon(gr,offset, measured, test);
        }
        else
        {
            _DrawStringWithIcons(gr,offset, measured,test);
        }
    }
    private static void _DrawStringWithIcons(Graphics gr,PointF offset, IMeasuredIcon measured,bool test=false)
    {
        //test = true;
        var args = measured.Args;
        var maxh=measured.Bounds.Height;
        var oldtr = gr.TextRenderingHint;
        gr.TextRenderingHint = args.TextRenderingHint;
        var br = new SolidBrush(args.ForeColor);
        var iconcolor =args.FinalIconColor;
        var bounds = measured.OffsetBounds(offset.X, offset.Y);
        var iconbounds = measured.OffsetBoundsIcon(offset.X, offset.Y);
        var textbounds= measured.OffsetBoundsText(offset.X, offset.Y);
        if (measured.Icon is not null)
        {
            var r = iconbounds;
            if (test) gr.FillRectangle(new SolidBrush(Color.Chartreuse), r);
            measured.Icon.DrawIcon(gr, iconcolor, r);
        }
        if (measured.Text!="")
        {
            var r = textbounds;
            if (test) gr.FillRectangle(new SolidBrush(Color.BlueViolet), r);
                gr.DrawString(measured.Text, args.Font, br, r.X, r.Y, StringFormat.GenericTypographic);
        }
        gr.TextRenderingHint = oldtr;
    }

    private static void _DrawSoloIcon(Graphics gr, PointF offset, IMeasuredIcon measured, bool test = false)
    {
        //test = true;
        var args = measured.Args;
        var oldtr = gr.TextRenderingHint;
        var bounds = measured.Bounds;
        var r = bounds.WithOffset(offset);
        if (test) gr.FillRectangle(new SolidBrush(Color.Chartreuse), r.ToRectangleF());
        measured.Icon?.DrawIcon(gr,args.FinalIconColor, r.X, r.Y, r.Size);
    }
    public static PointF AlignOffset(this ContentAlignment alignment,RectangleF outerbounds, RectangleF textbounds)
    {
        var res= textbounds.Location;
        switch (alignment)
        {
            case ContentAlignment.TopCenter:
            case ContentAlignment.TopLeft:
            case ContentAlignment.TopRight:
                res = new PointF(textbounds.X,outerbounds.Top);
                break;
            case ContentAlignment.BottomCenter:
            case ContentAlignment.BottomLeft:
            case ContentAlignment.BottomRight:
                res = new PointF(textbounds.X, outerbounds.Bottom - textbounds.Height);
                break;
            case ContentAlignment.MiddleCenter:
            case ContentAlignment.MiddleLeft:
            case ContentAlignment.MiddleRight:
                res = new PointF(textbounds.X, outerbounds.Y + (outerbounds.Height - textbounds.Height) / 2);
                break;
        }
        switch (alignment)
        {
            case ContentAlignment.TopLeft:
            case ContentAlignment.MiddleLeft:
            case ContentAlignment.BottomLeft:
                res = new PointF(outerbounds.Left,res.Y);
                break;
            case ContentAlignment.TopRight:
            case ContentAlignment.MiddleRight:
            case ContentAlignment.BottomRight:
                res = new PointF(outerbounds.Right - textbounds.Width, res.Y);
                break;
            case ContentAlignment.TopCenter:
            case ContentAlignment.MiddleCenter:
            case ContentAlignment.BottomCenter:
                res = new PointF(outerbounds.X + (outerbounds.Width - textbounds.Width) / 2, res.Y);
                break;
        }

        return res;
    }
    public static PointF AlignOffset(this Control c, ContentAlignment alignment, RectangleF textbounds)
    {
        var controlbounds = new RectangleF(c.Padding.Left, c.Padding.Top, c.Width - c.Padding.Horizontal, c.Height - c.Padding.Vertical);
        return alignment.AlignOffset(controlbounds, textbounds);
    }
    public static PointF AlignOffset(this Control c, ContentAlignment alignment, FontRectangleF textbounds)
    {
        return c.AlignOffset(alignment, textbounds.ToRectangleF());
    }
}