using App.Base;
using UnityEngine;

public class TouchManager : MonoBehaviour {

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
	            TouchContext.GetInstance().touching = true;
	        }
	        else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
	        {
	            Debug.Log("touch end...");

	            foreach (GameObject gameObject in  TouchContext.GetInstance().TouchedList())
	            {

	                Debug.Log("touched objet : " + gameObject.tag + ", ready:" + gameObject.GetComponent<TouchAction>().ready2go);
	                if (gameObject.GetComponent<TouchAction>().ready2go)
	                {
	                    gameObject.transform.position += Vector3.down;
	                }
	                else
	                {
	                    gameObject.transform.position += Vector3.up;
	                }
	                gameObject.GetComponent<TouchAction>().ready2go = !gameObject.GetComponent<TouchAction>().ready2go;
	                gameObject.transform.Find("Front").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
	            }

	            TouchContext.GetInstance().touching = false;
	            TouchContext.GetInstance().Clear();
	        }
	    }
	}
}
