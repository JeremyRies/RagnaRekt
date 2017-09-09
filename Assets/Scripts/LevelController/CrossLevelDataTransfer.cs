using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossLevelDataTransfer : MonoBehaviour
{

    private int[] _selectedCharacters;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
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
