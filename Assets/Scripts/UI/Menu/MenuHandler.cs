using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public void OnQuit()
    {
#if DEBUG
        Debug.Break();
#else
        Application.Stop();
#endif
    }
}
