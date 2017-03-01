
using System;
using App.Base;
using UnityEngine;

public class TweenCardMove : UITweener
{
    protected override void OnUpdate(float factor, bool isFinished)
    {
//        Debug.Log(string.Format("OnUpdate called, tag:{0}, inHand:{1}, factor:{2}, isFinished:{3}",tag, GetComponent<CardAttr>().inHand,  factor, isFinished));
//        if (GetComponent<CardAttr>().inHand && TouchManager.GetInstance().touching)
//        {
//            Debug.Log(string.Format("Hover OnFocus called, tag:{0}, inHand:{1}", tag, GetComponent<CardAttr>().inHand));
//            OnFocus();
//        }
    }

    public void OnFocus()
    {
        Debug.Log(string.Format("OnFocus:{0}", tag));
        TouchManager.GetInstance().OnFocus(transform);
    }

    private void OnMouseEnter()
    {
        Debug.Log(string.Format("OnMouseEnter:{0}", tag));
    }
}
