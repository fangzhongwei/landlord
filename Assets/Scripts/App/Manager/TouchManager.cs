using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager
{
    private static readonly TouchManager instance = new TouchManager();
    public bool doDetect { get; set; }
    public bool touching { get; set; }
    private readonly List<Transform> touchedForms = new List<Transform>();

    private TouchManager()
    {
    }

    public static TouchManager GetInstance()
    {
        return instance;
    }

    public void OnFocus(Transform t)
    {
        GameObject go = t.gameObject;
        int idx = go.GetComponent<CardAttr>().idx;
        var count = touchedForms.Count;
        if (count == 0 || count == 1)
        {
            Active(t);
        }
        else
        {
            int lastIdx = touchedForms[touchedForms.Count - 1].GetComponent<CardAttr>().idx;
            if (Math.Abs(idx - lastIdx) == 1)
            {
                if (touchedForms[touchedForms.Count - 2].gameObject.tag.Equals(go.tag))
                {
                    Remove(touchedForms[touchedForms.Count - 1]);
                }
                else
                {
                    Active(t);
                }
            }
        }
    }

    private void Active(Transform t)
    {
        if (touchedForms.Count == 0 || !touchedForms[touchedForms.Count - 1].gameObject.tag.Equals(t.gameObject.tag))
        {
            touchedForms.Add(t);
            ChangeColor(t, Color.blue);
        }
    }

    private void Remove(Transform t)
    {
        touchedForms.Remove(t);
        ChangeColor(t, Color.white);
    }

    public void TouchEnded()
    {
        int point;
        bool ready2Go;
        foreach (Transform t in  touchedForms)
        {
            point = t.gameObject.GetComponent<CardAttr>().point;
            ready2Go = t.gameObject.GetComponent<CardAttr>().ready2go;
            if (ready2Go)
            {
                t.localPosition += Vector3.down * 0.5f;
                PlayManager.GetInstance().RemoveReady2GoPoint(point);
            }
            else
            {
                t.localPosition += Vector3.up * 0.5f;
                PlayManager.GetInstance().AddReady2GoPoint(point);
            }
            t.gameObject.GetComponent<CardAttr>().ready2go = !ready2Go;
            ChangeColor(t, Color.white);
        }

        touching = false;
        touchedForms.Clear();
    }

    private void ChangeColor(Transform t, Color c)
    {
        t.FindChild("front").GetComponent<MeshRenderer>().material.color = c;
    }
}
