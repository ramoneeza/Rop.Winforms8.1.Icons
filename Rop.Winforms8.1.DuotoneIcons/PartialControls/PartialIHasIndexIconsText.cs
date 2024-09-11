using System.ComponentModel;
using System.Drawing.Design;
using Rop.IncludeFrom.Annotations;

namespace Rop.Winforms8.DuotoneIcons.PartialControls;
internal partial class Dummy{}
[DummyPartial]
[IncludeFrom(typeof(PartialIHasIndexIcons),"GetText")]
[IncludeFrom(typeof(PartialIHasText),"PartialIHasIcon")]
internal partial class PartialIHasIndexIconsText:Control,IHasIndexIconsText
{
    public virtual string DefaultIconText
    {
        get => Text;
        set => Text = value;
    }

    private string _textIconDisabled="";

    public virtual string TextIconDisabled
    {
        get => _textIconDisabled;
        set => _setPropTch(ref _textIconDisabled, value);
    }

    [Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
    public string[] TextItems
    {
        get => _items.GetTexts();
        set
        {
            _items.SetText(value);
            LaunchTextChanged();
        }
    }
    public string GetText(int i)
    {
        return _items.GetTextOrDefault(i,DefaultIconText);
    }
    public void SetText(int i, string text)
    {
        _items.SetText(i, text);
        Invalidate();
    }
    public virtual string GetText()
    {
        if (Disabled && TextIconDisabled!="") return TextIconDisabled;
        return GetText(SelectedIcon);
    }
}