
using App.Base;

public class TweenCardMove : UITweener
{
    protected override void OnUpdate(float factor, bool isFinished)
    {
        if (GetComponent<CardAttr>().inHand && TouchManager.GetInstance().touching)
        {
            OnFocus();
        }
    }

    public void OnFocus()
    {
        TouchManager.GetInstance().OnFocus(transform);
    }
}
