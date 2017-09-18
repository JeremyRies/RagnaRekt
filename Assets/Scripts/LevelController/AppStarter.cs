using UnityEngine;

public class AppStarter : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    LevelController.Instance.LoadMenuScene();
	}
}
