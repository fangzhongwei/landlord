using App.Base;
using UnityEngine;

public class TouchAction : MonoBehaviour {

    public bool ready2go { get; set; }
    public int idx { get; set; }

    void Awake ()
    {
        UIEventListener.Get(gameObject).onClick = ObjOnClick;
    }

    void ObjOnClick(GameObject go)
    {
        //
    }

    // Update is called once per frame
    void Update () {
        //绘制射线为红色。方便调试
        //Debug.DrawLine(Target.transform.position, transform.position, Color.red);
        //Debug.DrawRay();
    }

    private void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter:" + tag);
        TouchManager.GetInstance().OnFocus(transform);
    }

    private void OnMouseOver()
    {
        Debug.Log("OnMouseOver:" + tag);
        //TouchManager.GetInstance().OnFocus(transform);
    }

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag:" + tag);
        //TouchManager.GetInstance().OnFocus(transform);
    }

    private void OnMouseUp()
    {
        Debug.Log("OnMouseUp:" + tag);
    }

    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit:" + tag);
       TouchManager.GetInstance().OnBlur(transform);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("OnMouseUpAsButton:" + tag);
    }

}
