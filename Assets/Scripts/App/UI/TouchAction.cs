using App.Base;
using UnityEngine;

public class TouchAction : MonoBehaviour
{
    private void OnMouseEnter()
    {
        Debug.Log(string.Format("OnMouseEnter:{0}", tag));
        TouchManager.GetInstance().OnFocus(transform);
    }
}
