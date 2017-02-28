using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Base
{
    public class TouchManager
    {
        private static readonly TouchManager instance = new TouchManager();
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

        private void OnBlur(Transform t)
        {
        }

        private void Remove(Transform t)
        {
            touchedForms.Remove(t);
            ChangeColor(t, Color.white);
        }

        public void TouchEnded()
        {
            foreach (Transform t in  touchedForms)
            {
                if (t.gameObject.GetComponent<CardAttr>().ready2go)
                {
                    t.localPosition += Vector3.down * 10;
                }
                else
                {
                    t.localPosition += Vector3.up * 10;
                }
                t.gameObject.GetComponent<CardAttr>().ready2go = !t.gameObject.GetComponent<CardAttr>().ready2go;
                ChangeColor(t, Color.white);
            }

            touching = false;
            touchedForms.Clear();
        }

        private void ChangeColor(Transform t, Color c)
        {
            t.gameObject.GetComponent<UISprite>().color = c;
            t.gameObject.GetComponent<UIButton>().defaultColor = c;
            t.gameObject.GetComponent<UIButton>().hover = c;
        }
    }
}