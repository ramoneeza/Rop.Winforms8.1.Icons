﻿using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.IncludeFrom.Annotations;

namespace Rop.Winforms8.DuotoneIcons.PartialControls
{
    internal partial class Dummy{}
    
    [DummyPartial]
    [AlsoInclude(typeof(PartialIHasIcon))]
    internal partial class PartialIHasText:Control,IHasText
    {
        private bool _useIconColor;
        public virtual bool UseIconColor
        {
            get => _useIconColor;
            set=> _setPropInv(ref _useIconColor, value);
        }
        private float _offsetText;

        public virtual float OffsetText
        {
            get => _offsetText;
            set => _setPropFch(ref _offsetText, value);
        }
        
        private float _iconMarginLeft;
        public virtual float IconMarginLeft
        {
            get => _iconMarginLeft;
            set => _setPropTch(ref _iconMarginLeft, value);
        }
        private float _iconMarginRight;

        public virtual float IconMarginRight
        {
            get => _iconMarginRight;
            set=> _setPropTch(ref _iconMarginRight, value);
        }
        private bool _isSuffix;

        public virtual bool IsSuffix
        {
            get => _isSuffix;
            set => _setPropTch(ref _isSuffix, value);
        }
        private TextRenderingHint _textRenderingHint;
        public virtual TextRenderingHint TextRenderingHint
        {
            get => _textRenderingHint;
            set => _setPropInv(ref _textRenderingHint, value);
        }
        public virtual Color GetForeColor()
        {
            if (DisableAndThereIsDisabledColor()) return DisabledColor.Color1;
            if (UseIconColor) return (this as IHasIcon).GetIconColor().Color1;
            return Enabled?ForeColor:SystemColors.GrayText;
        }
    }
    // Abstract Partial
    internal partial class PartialIHasText
    {
        // Abstract part

        [ExcludeThis]
        public string GetIconCode() => "";

        [ExcludeThis]
        public DuoToneColor GetIconColor()=>DuoToneColor.Default;
        [ExcludeThis]
        public string GetToolTipText() => "";

        [ExcludeThis]
        public string GetText() => "";
    }
}
