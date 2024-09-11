using System.ComponentModel;
using System.Drawing;

namespace Rop.Drawing8.Units;

/// <summary>
/// A struct that represents a rectangle whit an ascent, descent and baseline.
/// </summary>
public record struct FontRectangleF(float X,float Y,float Width,float Height,float Ascent)
{
    /// <summary>
    /// A struct that represents an empty FontRectangleF.
    /// </summary>
    public static readonly FontRectangleF Empty = default;
    /// <summary>
    /// Gets the rectangle corresponding to this FontRectangleF's ascent.
    /// </summary>
    [Browsable(false)]
    public readonly RectangleF AscentBounds => new RectangleF(X, Y, Width, Ascent);
    /// <summary>
    /// Gets or sets the baseline value.
    /// </summary>
    [Browsable(false)]
    public float BaseLine
    {
        readonly get => this.Y + this.Ascent;
        set => this.Y = value - this.Ascent;
    }
    /// <summary>
    /// Gets or sets the location of the baseline.
    /// </summary>
    [Browsable(false)]
    public PointF BaseLineLocation
    {
        readonly get => new PointF(this.X, this.BaseLine);
        set
        {
            this.X = value.X;
            this.BaseLine = value.Y;
        }
    }
    /// <summary>
    /// Gets or sets the location of the bottom of the rectangle.
    /// </summary>
    [Browsable(false)]
    public PointF BotomLocation
    {
        readonly get => new PointF(this.X, this.Y + this.Height);
        set
        {
            this.X = value.X;
            this.Y = value.Y - this.Height;
        }
    }
    /// <summary>
    /// Gets or sets the bottom value.
    /// </summary>
    [Browsable(false)]
    public float Bottom
    {
        readonly get => this.Y + this.Height;
        set => Y = value - this.Height;
    }
    /// <summary>
    /// Gets or sets the rectangle value containing both the ascent and descent values.
    /// </summary>
    [Browsable(false)]
    public RectangleF Bounds
    {
        readonly get => new RectangleF(X, Y, Width, Height);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Width = value.Width;
            this.Height = value.Height;
        }
    }
    /// <summary>
    /// Gets or sets the X value of the center point.
    /// </summary>
    [Browsable(false)]
    public float CenterX
    {
        readonly get => this.X + this.Width / 2;
        set => this.X = value - this.Width / 2;
    }
    /// <summary>
    /// Gets or sets the Y value of the center point.
    /// </summary>
    [Browsable(false)]
    public float CenterY
    {
        readonly get => this.Y + this.Height / 2;
        set => this.Y = value - this.Height / 2;
    }
    /// <summary>
    /// Gets or sets the descent value.
    /// </summary>
    [Browsable(false)]
    public float Descent
    {
        readonly get => this.Height - this.Ascent;
        set => this.Height = Ascent + value;
    }
    /// <summary>
    /// Gets the rectangle corresponding to this FontRectangleF's descent.
    /// </summary>
    [Browsable(false)]
    public readonly RectangleF DescentBounds => new RectangleF(X, BaseLine, Width, Descent);
    /// <summary>
    /// Gets a value indicating whether this FontRectangleF is empty.
    /// </summary>
    [Browsable(false)]
    public readonly bool IsEmpty => this.Equals(Empty);
    /// <summary>
    /// Gets the left value.
    /// </summary>
    [Browsable(false)]
    public float Left
    {
        readonly get => this.X;
        set => this.X = value;
    }

    /// <summary>
    /// Gets or sets the right value.
    /// </summary>
    [Browsable(false)]
    public float Right
    {
        readonly get => this.X + this.Width;
        set => X = value - this.Width;
    }
    /// <summary>
    /// Gets or sets the FontSizeF of this FontRectangleF.
    /// </summary>
    [Browsable(false)]
    public FontSizeF Size
    {
        readonly get => new FontSizeF(this.Width, this.Height, this.Ascent);
        set
        {
            this.Width = value.Width;
            this.Height = value.Height;
            this.Ascent = value.Ascent;
        }
    }
    /// <summary>
    /// Gets the top value.
    /// </summary>
    [Browsable(false)]
    public float Top
    {
        readonly get => this.Y;
        set => this.Y = value;
    }

    /// <summary>
    /// Gets the topleft location.
    /// </summary>
    [Browsable(false)]
    public PointF TopLeftLocation
    {
        readonly get => new PointF(this.X, this.Y);
        set
        {
            this.X = value.X;
            this.Y = value.Y;
        }
    }
    /// <summary>
    /// Gets the topright location.
    /// </summary>
    [Browsable(false)]
    public PointF TopRightLocation
    {
        readonly get => new(Right, Top);
        set
        {
            this.Right = value.X;
            this.Y = value.Y;
        }
    }

    /// <summary>
    /// Initializes a new instance of the FontRectangleF struct.
    /// </summary>
    /// <param name="location">The coordinates for the starting location of the rectangle.</param>
    /// <param name="size">The FontSizeF of the rectangle.</param>
    public FontRectangleF(PointF location, FontSizeF size) : this(location.X, location.Y, size.Width, size.Height,
        size.Ascent)
    {
    }

    /// <summary>
    /// Implicitly converts a FontRectangleF object to a RectangleF object.
    /// </summary>
    /// <param name="r">The FontRectangleF object to convert.</param>
    /// <returns>A RectangleF object that is equivalent to the FontRectangleF passed.</returns>
    public static explicit operator RectangleF(FontRectangleF r) => new RectangleF(r.X, r.Y, r.Width, r.Height);
    
    /// <summary>
    /// Creates a new instance of the FontRectangleF struct using the specified base line.
    /// </summary>
    /// <param name="baseline">The baseline of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    /// <returns>A new instance of the FontRectangleF struct.</returns>
    public static FontRectangleF FromBaseLine(PointF baseline, FontSizeF size) => FromBaseLine(baseline.X, baseline.Y, size.Width, size.Height, size.Ascent);
    
    /// <summary>
    /// Creates a new instance of the FontRectangleF struct using the specified coordinates and size.
    /// </summary>
    /// <param name="x">The x component of the starting coordinates for the rectangle.</param>
    /// <param name="baseline">The y component of the base line coordinates for the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="ascent">The ascent of the rectangle.</param>
    /// <returns>A new instance of the FontRectangleF struct.</returns>
    public static FontRectangleF FromBaseLine(float x, float baseline, float width, float height, float ascent) => new FontRectangleF(x, baseline - ascent, width, height, ascent);
    
    /// <summary>
    /// Creates a new instance of the FontRectangleF struct using the specified coordinates, ascent and descent values.
    /// </summary>
    /// <param name="x">The x component of the starting coordinates for the rectangle.</param>
    /// <param name="baseline">The y component of the base line coordinates for the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="ascent">The ascent of the rectangle.</param>
    /// <param name="descent">The descent of the rectangle.</param>
    /// <returns>A new instance of the FontRectangleF struct.</returns>
    public static FontRectangleF FromBaseLineAscentDescent(float x, float baseline, float width, float ascent, float descent) => FromBaseLine(x, baseline, width, ascent + descent, ascent);
    public static FontRectangleF UnionByAscent(FontRectangleF a, FontRectangleF b)
    {
        float baseline = (a.Ascent > b.Ascent) ? a.BaseLine : b.BaseLine;
        return UnionByBaseLine(a, b, baseline);
    }
    
    /// <summary>
    /// Creates a new instance of the FontRectangleF struct using the specified FontRectangleFs and setting the baseline Value accordingly.
    /// </summary>
    /// <param name="rectangles">The FontRectangleFs to include in the union.</param>
    /// <returns>A new instance of the FontRectangleF struct.</returns>
    public static FontRectangleF UnionByAscent(IEnumerable<FontRectangleF> rectangles)
    {
        var all = rectangles.ToArray();
        var rbaseline = all.MaxBy(a => a.Ascent).BaseLine;
        return UnionByBaseLine(all, rbaseline);
    }
    /// <summary>
    /// Creates a new instance of the FontRectangleF struct, calling the BaseLine method to set the baseline value.
    /// </summary>
    /// <param name="a">A FontRectangleF to include.</param>
    /// <param name="b">A FontRectangleF to include.</param>
    /// <param name="baseline">The baseline value to use.</param>
    /// <returns>A new instance of the FontRectangleF struct.</returns>
    public static FontRectangleF UnionByBaseLine(FontRectangleF a, FontRectangleF b, float baseline)
    {
        a.BaseLine = baseline;
        b.BaseLine = baseline;
        float x = Math.Min(a.X, b.X);
        float right = Math.Max(a.Right, b.Right);
        float y = Math.Min(a.Y, b.Y);
        float bottom = Math.Max(a.Bottom, b.Bottom);
        return FontRectangleF.FromBaseLine(x, baseline, right - x, bottom - y, baseline - y);
    }
    /// <summary>
    /// Creates a new instance of the FontRectangleF struct, calling the BaseLine method to set the baseline value.
    /// </summary>
    /// <param name="rectangles">The FontRectangleFs to include in the union.</param>
    /// <param name="baseline">The baseline value to use.</param>
    /// <returns>A new instance of the FontRectangleF struct.</returns>
    public static FontRectangleF UnionByBaseLine(IEnumerable<FontRectangleF> rectangles, float baseline)
    {
        var all = rectangles.Select(r => r with {BaseLine=baseline}).ToArray();
        float x = all.Min(r => r.X);
        float right = all.Max(r => r.Right);
        float y = all.Min(r => r.Y);
        float bottom = all.Max(r => r.Bottom);
        return FontRectangleF.FromBaseLine(x, baseline, right - x, bottom - y, baseline - y);
    }
    /// <summary>
    /// Determines if the specified point is contained within this FontRectangleF.
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    /// <returns>True if the specified point is contained, otherwise false</returns>
    public readonly bool Contains(float x, float y) => Bounds.Contains(x, y);
    /// <summary>
    /// Determines if the specified point is contained within this FontRectangleF.
    /// </summary>
    /// <param name="pt">Point to compare</param>
    /// <returns>True if the specidied point is contained, otherwise false</returns>
    public readonly bool Contains(PointF pt) => this.Contains(pt.X, pt.Y);
    /// <summary>
    /// Determines if the specified FontRectangleF is contained within this FontRectangleF.
    /// </summary>
    /// <param name="rect">FontRectangleF to compare</param>
    /// <returns>True if the specified FontRectangleF is contained, otherwise false</returns>
    public readonly bool Contains(RectangleF rect) => Bounds.Contains(rect);
    
    /// <summary>
    /// Offsets the FontRectangleF by the specified amount.
    /// </summary>
    /// <param name="pos">Amount to Offset</param>
    public void Offset(PointF pos) => this.Offset(pos.X, pos.Y);
   /// <summary>
   /// Offsets the FontRectangleF by the specified amount.
   /// </summary>
   /// <param name="x">Offset X</param>
   /// <param name="y">Offset Y</param>
    public void Offset(float x, float y)
    {
        this.X += x;
        this.Y += y;
    }
   /// <summary>
   /// Converts the FontRectangleF to a RectangleF.
   /// </summary>
   /// <returns>A new RectangleF instance.</returns>
    public RectangleF ToRectangleF()
    {
        return new RectangleF(X, Y, Width, Height);
    }
   /// <summary>
   /// Returns a FontRectangleF that is offset by the specified amount.
   /// </summary>
   /// <param name="dx">Delta x value</param>
   /// <param name="dy">Delta y value</param>
   /// <returns>FontRectengleF with offset</returns>
   public FontRectangleF WithOffset(float dx, float dy)
   {
       var a = this;
       a.X += dx;
       a.Y += dy;
       return a;
   }
   public FontRectangleF WithOffset(PointF delta) => WithOffset(delta.X, delta.Y);
}