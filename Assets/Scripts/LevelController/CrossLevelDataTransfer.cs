using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;

public class CrossLevelDataTransfer : MonoBehaviour
{
    public GameObject SoundManager;
    private int[] _selectedCharacters;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(SoundManager);
	}

    public void SaveSelectedCharacters(int[] characters)
    {
        _selectedCharacters = characters;
    }

    public int[] GetSelectedCharacters()
    {
        return _selectedCharacters;
    }
	
}
