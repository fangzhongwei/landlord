using System.Collections.Generic;
using App.Base;
using UnityEngine;

public class TouchDetect : MonoBehaviour {

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
	void Update () {

	    if (Input.touchCount > 0)
	    {
	        if (Input.touches[0].phase == TouchPhase.Began)
	        {
	            Debug.Log("touch begin...");
	            TouchManager.GetInstance().touching = true;
	        }
	        else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
	        {
	            Debug.Log("touch end...");
	            TouchManager.GetInstance().TouchEnded();
	        }
	    }
	}
}
