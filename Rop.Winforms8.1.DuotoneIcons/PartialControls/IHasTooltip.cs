namespace Rop.Winforms8.DuotoneIcons.PartialControls;

public interface IHasTooltip
{
    ToolTip? ToolTip { get; set; }
    bool ShowToolTip { get; set; }
    string GetToolTipText();
    void InitIHasToolTip();
    void DoShowTooltip(string s);
}