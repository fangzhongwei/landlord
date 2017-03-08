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
	            TouchManager.GetInstance().touching = true;
	            FindCard(new Vector3(Input.touches[0].position.x, Input.touches[0].position.y, -10.0f));
	        }
	        else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
	        {
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
            Debug.DrawLine(touchPosition, hit.transform.position);
            if (hit.transform.gameObject.GetComponent<CardAttr>() != null && hit.transform.gameObject.GetComponent<CardAttr>().inHand)
            {
                TouchManager.GetInstance().OnFocus(hit.transform);
            }
        }
    }
}
