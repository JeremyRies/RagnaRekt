using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppStarter : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    LevelController.GetLevelController().LoadMenuScene();
	}
}
