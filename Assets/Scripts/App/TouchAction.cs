using App.Base;
using UnityEngine;

public class TouchAction : MonoBehaviour {

    public bool ready2go { get; set; }

    // Update is called once per frame
    void Update () {

        //绘制射线为红色。方便调试
        //Debug.DrawLine(Target.transform.position, transform.position, Color.red);
        //Debug.DrawRay();
    }

    private void OnMouseEnter()
    {
        TouchContext.GetInstance().Add(gameObject);

        //gameObject.GetComponent<Light>().enabled = true;
        //GetComponent<Renderer>().material.color = Color.red;
        //shader
        Debug.Log("OnMouseEnter:" + tag);
        //GameObject.FindGameObjectWithTag(tag + "Face").GetComponent<SpriteRenderer>().color = Color.red;
        transform.Find("Front").gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnMouseOver()
    {
        Debug.Log("OnMouseOver22:" + tag);
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag:" + tag);
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp:" + tag);
    }

    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit:" + tag);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("OnMouseUpAsButton:" + tag);
    }
}
