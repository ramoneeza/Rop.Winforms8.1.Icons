using System.Drawing;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Rop.Drawing8.Units
{
    public record RectangleCollection
    {
        private struct LeftSize
        {
            public float Left;
            public FontSizeF Size;
            public void Deconstruct(out float left, out FontSizeF size)
            {
                left = Left;
                size = Size;
            }
        }
        private readonly LeftSize[] _relativeSizes;
        public PointF Location { get; init; }

        public float Ascent { get; private set; } = 0;
        public float Descent { get; private set; } = 0;
        public float Width { get; private set; } = 0;
        public float MaxHeight { get; private set; } = 0;
        public float Height => Ascent + Descent;
        public FontSizeF SizeAscent => new(Width, Height, Ascent);
        public FontRectangleF RelativeBounds => new(0, 0, Width, Height, Ascent);
        public FontRectangleF AbsoluteBounds => RelativeBounds.WithOffset(Location);
        
        public RectangleCollection(PointF location,params FontSizeF[] sizes)
        {
            _relativeSizes = new LeftSize[sizes.Length];
            var last = 0f;
            Ascent = 0;
            Descent = 0;
            Width = 0;
            MaxHeight = 0;
            var i = 0;
            foreach (var s in sizes)
            {
                _relativeSizes[i++] = new LeftSize(){Left = last,Size = s};
                last += s.Width;
                if (s.Ascent > Ascent) Ascent = s.Ascent;
                if (s.Descent > Descent) Descent = s.Descent;
                Width += s.Width;
                if (s.Height > MaxHeight) MaxHeight = s.Height;
            }
            Location = location;
        }
        public RectangleCollection(params FontSizeF[] rectangles):this(PointF.Empty, rectangles)
        {
        }

        public FontRectangleF GetRelativeRectangle(int index)
        {
            return _getRelativeRectangle(_relativeSizes[index]);
        }
        private FontRectangleF _getRelativeRectangle(LeftSize lf)
        {
            var (left, sz) = lf;
            return FontRectangleF.FromBaseLine(left,Ascent,sz.Width,sz.Height,sz.Ascent);
        }
        private FontRectangleF _getAbsoluteRectangle(LeftSize lf)
        {
            var r = _getRelativeRectangle(lf);
            return r.WithOffset(Location.X,Location.Y);
        }
        public FontRectangleF GetAbsoluteRectangle(int index)=> _getAbsoluteRectangle(_relativeSizes[index]);
        public IEnumerable<FontRectangleF> GetRelativeRectangles()=> _relativeSizes.Select(_getRelativeRectangle);
        public IEnumerable<FontRectangleF> GetAbsoluteRectangles=> _relativeSizes.Select(_getAbsoluteRectangle);
    }

}
