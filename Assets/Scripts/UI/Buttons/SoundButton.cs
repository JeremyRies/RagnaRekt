using Sound;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundButton : Button {
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SfxSound.SfxSoundInstance.PlayClip(ClipIdentifier.UIButtonSelect);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        SfxSound.SfxSoundInstance.PlayClip(ClipIdentifier.UIButtonSelect);
    }
}
