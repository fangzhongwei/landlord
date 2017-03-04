using App.Base;
using UnityEngine;

public class TouchAction : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (GetComponent<CardAttr>().inHand)
        {
            TouchManager.GetInstance().OnFocus(transform);
        }
    }
}
