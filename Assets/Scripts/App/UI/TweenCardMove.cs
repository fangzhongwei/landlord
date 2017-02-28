
using App.Base;
using UnityEngine;

public class TweenCardMove : UITweener
{
    protected override void OnUpdate(float factor, bool isFinished)
    {
        Debug.Log(string.Format("OnUpdate called, factor:{0}, isFinished:{1}", factor, isFinished));
//        Vector3 from = transform.position;
//        Vector3 to = from + Vector3.right;
//        Vector3 value = from * (1f - factor) + to * factor;
//        transform.localPosition += Vector3.up * 100;
        if (TouchManager.GetInstance().touching)
        {
            OnFocus();
        }
    }

    public void OnFocus()
    {
        TouchManager.GetInstance().OnFocus(transform);
    }
}
