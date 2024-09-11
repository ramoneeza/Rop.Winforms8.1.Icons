using System.ComponentModel;

namespace Rop.Winforms8.DuotoneIcons.MaterialDesign;

internal partial class Dummy
{
}

public class MaterialDesignBank : Component, IBankIcon
{
    public IEmbeddedIcons Bank { get; }

    public MaterialDesignBank()
    {
        Bank = IconRepository.GetEmbeddedIcons<MaterialDesignIcons>();
    }
}