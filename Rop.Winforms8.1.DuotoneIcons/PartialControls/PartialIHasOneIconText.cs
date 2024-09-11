using System.ComponentModel;
using Rop.IncludeFrom.Annotations;

namespace Rop.Winforms8.DuotoneIcons.PartialControls;
internal partial class Dummy{}
[DummyPartial]
[AlsoInclude(typeof(PartialIHasOneIcon))]
[AlsoInclude(typeof(PartialIHasText),"PartialIHasIcon")]
internal partial class PartialIHasOneIconText:Control,IHasOneIconText
{
    public virtual string GetText() => Text;
}