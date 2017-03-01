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
	            FindCard(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, -10.0f));
	        }
	        else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
	        {
	            Debug.Log("touch end...");
	            TouchManager.GetInstance().TouchEnded();
	        }
	    }
	}

    private void FindCard (Vector3 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("got one:" + hit.transform.tag);
            Debug.DrawLine(touchPosition, hit.transform.position);
            TouchManager.GetInstance().OnFocus(hit.transform);
        }
    }
}
