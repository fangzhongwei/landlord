using UnityEngine;

public class CardAttr : MonoBehaviour
{
    public bool ready2go { get; set; }
    public int idx { get; set; }

    void Start()
    {
        if ("card1".Equals(gameObject.name))
        {
            ready2go = false;
            idx = 0;
        }
        else if ("card2".Equals(gameObject.name))
        {
            ready2go = false;
            idx = 1;
        }
        else if ("card3".Equals(gameObject.name))
        {
            ready2go = false;
            idx = 2;
        }
    }

}