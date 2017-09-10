using System.Collections;
using System.Collections.Generic;
using Sound;
using UI.Menu;
using UniRx;
using UnityEngine;
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
        base.OnSubmit(eventData);
        DisplayPlayerId(true);
    }

    private void DisplayPlayerId(bool value)
    {
        var currentPlayer = CharacterPickingHandler.GetCounter();
        _playerIds[currentPlayer].gameObject.SetActive(value);
    }
}
