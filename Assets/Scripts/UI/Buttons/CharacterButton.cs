using Sound;
using UI.Menu;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButton : Button
{
    public Image[] _playerIds; 
    public CharacterPickingHandler CharacterPickingHandler;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        SfxSound.SfxSoundInstance.PlayClip(ClipIdentifier.UIButtonSelect);
        DisplayPlayerId(true);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        DisplayPlayerId(false);

    }

    public override void OnSubmit(BaseEventData eventData)
    {
        DisplayPlayerId(false);
        SfxSound.SfxSoundInstance.PlayClip(ClipIdentifier.UIButtonSelect);
        base.OnSubmit(eventData);
        DisplayPlayerId(true);
    }

    public void ResetPlayerId()
    {
        foreach (var playerId in _playerIds)
        {
            playerId.gameObject.SetActive(false);
        }
    }

    private void DisplayPlayerId(bool value)
    {
        var currentPlayer = CharacterPickingHandler.GetCounter();
        _playerIds[currentPlayer].gameObject.SetActive(value);
    }
}
