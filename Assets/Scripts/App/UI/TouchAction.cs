using App.Base;
using UnityEngine;

public class TouchAction : MonoBehaviour
{
    private void OnMouseEnter()
    {
        TouchManager.GetInstance().OnFocus(transform);
    }
}
