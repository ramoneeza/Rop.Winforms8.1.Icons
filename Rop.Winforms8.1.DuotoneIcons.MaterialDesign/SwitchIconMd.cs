using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Rop.Winforms8.DuotoneIcons.Controls;

namespace Rop.Winforms8.DuotoneIcons.MaterialDesign;
 internal partial class Dummy{}
public class SwitchIconMd : IconBoolLabel
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IBankIcon? BankIcon => null;
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public SwitchIconMd():base()
    {
        Icons = IconRepository.GetEmbeddedIcons<MaterialDesignIcons>();
        IconOff = "ToggleSwitchOffLegacy";
        IconOn = "ToggleSwitchLegacy";
        IconScale = 150;
        DefaultIconColor = DuoToneColor.DefaultOneTone;
        DefaultIconText = "Switch";
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color SwitchColorOn
    {
        get => IconColorOn.Color1;
        set => IconColorOn = IconColorOn.WithColor1(value);
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color SwitchColorOff
    {
        get => IconColorOff.Color1;
        set => IconColorOff = IconColorOff.WithColor1(value);
    }
    [Browsable(false)]
    public override string Text
    {
        get => base.Text;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        set => base.Text = value??"";
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    }

    public bool AutoChange { get; set; }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (AutoChange && !Disabled)
        {
            Value = !Value;
        }
        base.OnMouseDown(e); 
    }
}