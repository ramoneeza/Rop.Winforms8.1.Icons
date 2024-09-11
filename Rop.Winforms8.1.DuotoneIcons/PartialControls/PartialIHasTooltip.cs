﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.IncludeFrom.Annotations;

namespace Rop.Winforms8.DuotoneIcons.PartialControls;

internal partial class Dummy{}
[DummyPartial]
internal class PartialIHasTooltip:Control, IHasTooltip
{
    public ToolTip? ToolTip { get; set; }
    public bool ShowToolTip { get; set; }
        
    [ExcludeThis]
    public virtual string GetToolTipText() => "";

    public void InitIHasToolTip()
    {
        ToolTip = new ToolTip();
        MouseEnter += (_,_) => _mouseEnter();
        MouseLeave += (_, _) => _mouseLeave();
    }
    public void DoShowTooltip(string s)
    {
        if (ToolTip == null) return;
        var ss = string.IsNullOrEmpty(s) ? null : s;
        ToolTip?.SetToolTip(this, ss);
    }
    void _mouseEnter()
    {
        var s =ShowToolTip?GetToolTipText():"";
        DoShowTooltip(s);
    }
    protected virtual void _mouseLeave()
    {
        DoShowTooltip("");
    }
}