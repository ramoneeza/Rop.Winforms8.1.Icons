﻿using Rop.IncludeFrom.Annotations;
using Rop.Winforms8.DuotoneIcons.PartialControls;

namespace Rop.Winforms8.DuotoneIcons.Controls
{
    internal partial class Dummy{}
    [IncludeFrom(typeof(PartialIHasIndexIconsText))]
    public partial class IconIndexButton: Button, IHasIndexIcons, IHasText
    {
        private bool _painting = false;
        public override string Text
        {
            get => (!_painting)?base.Text:"";
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
            set => base.Text = value??"";
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _painting = true;
            base.OnPaint(e);
            _painting = false;
            IconPaint(e.Graphics);
        }

        public IconIndexButton()
        {
            base.Text = "";
            InitShowHidden();
            InitIHasToolTip();
        }
       
    }
}
