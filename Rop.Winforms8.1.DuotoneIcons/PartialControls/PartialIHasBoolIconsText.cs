using Rop.IncludeFrom.Annotations;

namespace Rop.Winforms8.DuotoneIcons.PartialControls;
internal partial class Dummy{}
[DummyPartial]
[AlsoInclude(typeof(PartialIHasBoolIcons),"GetText")]
[AlsoInclude(typeof(PartialIHasText),"PartialIHasIcon")]
internal partial class PartialIHasBoolIconsText:Control,IHasBoolIconsText
{
    private string _defaultIconText = "";

    public virtual string DefaultIconText
    {
        get => _defaultIconText;
        set
        {
            _defaultIconText= value;
            LaunchTextChanged();
        }
    }
    private string _textIconDisabled = "";
    public virtual string TextIconDisabled
    {
        get => _textIconDisabled;
        set
        {
            _textIconDisabled = value;
            LaunchTextChanged();
        }
    }
    private string _textIconOff = "";
    public virtual string TextIconOff
    {
        get => _textIconOff;
        set
        {
            _textIconOff= value;
            LaunchTextChanged();
        }
    }
    private string _textIconOn = "";
    public virtual string TextIconOn
    {
        get => _textIconOn;
        set
        {
            _textIconOn= value;
            LaunchTextChanged();
        }
    }
    public virtual string GetText()
    {
        if (Disabled) return !string.IsNullOrEmpty(TextIconDisabled) ? TextIconDisabled : DefaultIconText;
        var r = SelectedIcon ? TextIconOn : TextIconOff;
        return !string.IsNullOrEmpty(r) ? r : DefaultIconText;
    }
}