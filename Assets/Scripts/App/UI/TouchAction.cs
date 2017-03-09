using UnityEngine;

public class TouchAction : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (TouchManager.GetInstance().doDetect && GetComponent<CardAttr>().inHand)
        {
            TouchManager.GetInstance().OnFocus(transform);
        }
    }
}
