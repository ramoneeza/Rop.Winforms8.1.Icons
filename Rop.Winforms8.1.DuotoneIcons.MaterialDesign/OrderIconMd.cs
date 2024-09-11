using System.ComponentModel;
using Rop.Winforms8.DuotoneIcons.Controls;

namespace Rop.Winforms8.DuotoneIcons.MaterialDesign;

internal partial class Dummy{}

public sealed class OrderIconMd : OrderIcon
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override IBankIcon? BankIcon => null;
    public override IEmbeddedIcons? Icons { get; }
    public OrderIconMd()
    {

        Icons = IconRepository.GetEmbeddedIcons<MaterialDesignIcons>();
        IconAscending = "ChevronDown";
        IconDescending = "ChevronUp";
        IconUnselected = "ChevronDown";
        IconColorSelected = DuoToneColor.OneTone(Color.Black);
        IconColorUnSelected = DuoToneColor.OneTone(Color.Silver);
    }
}