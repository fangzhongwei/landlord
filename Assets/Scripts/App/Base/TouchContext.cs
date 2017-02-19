using Boo.Lang;
using UnityEngine;

namespace App.Base
{
    public class TouchContext
    {
        private static readonly TouchContext instance = new TouchContext();
        public bool touching { get; set; }
        private readonly List<GameObject> roundTouchedGos = new List<GameObject>();

        private TouchContext()
        {
        }

        public static TouchContext GetInstance()
        {
            return instance;
        }

        public void Add(GameObject go)
        {
            roundTouchedGos.Add(go);
        }

        public List<GameObject> TouchedList()
        {
            return roundTouchedGos;
        }

        public void Clear()
        {
            roundTouchedGos.Clear();
        }
    }
}