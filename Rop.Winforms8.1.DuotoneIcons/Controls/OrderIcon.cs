using System.ComponentModel;
using System.Drawing.Text;
using Rop.IncludeFrom.Annotations;
using Rop.Winforms8.DuotoneIcons.PartialControls;

namespace Rop.Winforms8.DuotoneIcons.Controls
{
    internal partial class Dummy{}
    
    [IncludeFrom(typeof(PartialIHasText),"PartialIHasBank","GetIcon")]
    [IncludeFrom(typeof(PartialIShowHidden))]
    public partial class OrderIcon : Label,IHasText
    {
        public event EventHandler? SortOrderChanged;
        public int ColumnIndex { get; set; }
        public bool Selectable { get; set; } = true;
        public bool Selected
        {
            get => SortOrder != SortOrder.None;
            set
            {
                if (!Selectable)
                {
                    SortOrder = SortOrder.None;
                    return;
                }

                if (Selected == value) return;
                SortOrder = (value) ? SortOrder.Ascending : SortOrder.None;
            }
        }
        private DuoToneColor _iconColorSelected = DuoToneColor.OneTone(Color.Black);
        public DuoToneColor IconColorSelected
        {
            get => _iconColorSelected;
            set
            {
                _iconColorSelected = value;
                Invalidate();
            }
        }
        private DuoToneColor _iconColorUnSelected = DuoToneColor.OneTone(Color.Silver);
        public DuoToneColor IconColorUnSelected
        {
            get => _iconColorUnSelected;
            set
            {
                _iconColorUnSelected = value;
                Invalidate();
            }
        }
        private string _iconUnSelected = "";
        public string IconUnselected
        {
            get => _iconUnSelected;
            set
            {
                _iconUnSelected = value;
                LaunchTextChanged();
            }
        }
        private string _iconAscending = "";
        public string IconAscending
        {
            get => _iconAscending;
            set
            {
                _iconAscending = value;
                LaunchTextChanged();
            }
        }
        private string _iconDescending = "";
        public string IconDescending
        {
            get => _iconDescending;
            set
            {
                _iconDescending = value;
                LaunchTextChanged();
            }
        }
        private SortOrder _sortOrder = SortOrder.None;
        public SortOrder SortOrder
        {
            get => _sortOrder;
            set
            {
                _sortOrder = value;
                LaunchTextChanged();
                LaunchSortOrderChanged();
            }
        }
        public bool Ascending
        {
            get => _sortOrder == SortOrder.Ascending;
            set => SortOrder = value ? SortOrder.Ascending : SortOrder.Descending;
        }

        private float _offsetIcon;

        public float OffsetIcon
        {
            get => _offsetIcon;
            set
            {
                _offsetIcon = value;
                LaunchFontChanged();
            }
        }

        private int _minAscent;

        public int MinAscent
        {
            get => _minAscent;
            set
            {
                _minAscent = value;
                LaunchFontChanged();
            }
        }
        private int _minHeight;
        public int MinHeight
        {
            get => _minHeight;
            set
            {
                 _minHeight = value;
                LaunchFontChanged();
            }
        }

        private int _iconScale;

        public int IconScale
        {
            get => _iconScale;
            set
            {
                _iconScale = value;
                LaunchFontChanged();
            }
        }

        private DuoToneColor _disabledColor;

        public DuoToneColor DisabledColor
        {
            get => _disabledColor;
            set
            {
                _disabledColor = value;
                Invalidate();
            }
        }

        public bool Disabled=>!Enabled;
        public string ToolTipText { get; set; } = "";
        public string GetToolTipText() => ToolTipText;

        public void LaunchSortOrderChanged()
        {
            SortOrderChanged?.Invoke(this,EventArgs.Empty);
        }
        public event EventHandler<PaintEventArgs>? PostPaint;
        public OrderIcon() : base()
        {
           InitIHasToolTip();
           InitShowHidden();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor),e.ClipRectangle);
            IconTextPaint(e.Graphics);
            PostPaint?.Invoke(this, e);
        }
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (!AutoSize) return base.GetPreferredSize(proposedSize);
            return GetPreferedSize();
        }
        public virtual void IconTextPaint(Graphics gr)
        {
            var args = IconArgs.Factory(this);
            var measure = MeasuredIconString.Factory(gr, args);
            var offset = this.AlignOffset(TextAlign, measure.Bounds);
            gr.DrawStringWithIcons(offset, measure);
        }
        public virtual string GetText()=>Text;
        public DuoToneIcon? GetIcon() =>(Parent as IHasBank)?.Icons?.GetIcon(GetIconCode());
        public string GetIconCode()
        {
            switch (SortOrder)
            {
                case SortOrder.Ascending:
                    return IconAscending;
                case SortOrder.Descending:
                    return IconDescending;
                default:
                    return IconUnselected;
            }
        }
        public DuoToneColor GetIconColor()
        {
            switch (SortOrder)
            {
                case SortOrder.Ascending:
                case SortOrder.Descending:
                    return IconColorSelected;
                default:
                    return IconColorUnSelected;
            }
        }
        public virtual bool DisableAndThereIsDisabledColor() => Disabled && DisabledColor != DuoToneColor.Empty;
        public virtual IBankIcon? BankIcon => (Parent as IHasBank)?.BankIcon;
        public virtual IEmbeddedIcons? Icons=>(Parent as IHasBank)?.Icons;
    }
}
