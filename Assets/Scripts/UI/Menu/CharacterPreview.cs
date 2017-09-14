using UnityEngine;
using UnityEngine.UI;

public class CharacterPreview : MonoBehaviour
{

    public Image[] Previews;
    public Sprite[] CharacterSprites;

    public void EnablePreview(int i, bool value)
    {
        Previews[i].gameObject.SetActive(value);
    }

    public void SetCharacter(int player, int character)
    {
        Previews[player].sprite = CharacterSprites[character-1];
    }

    public void ResetAll()
    {
        foreach (var preview in Previews)
        {
            preview.gameObject.SetActive(false);
        }
    }
}
