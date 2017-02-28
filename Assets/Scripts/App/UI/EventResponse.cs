using UnityEngine;

public class EventResponse : MonoBehaviour
{
    public void ResponseHoverIn()
    {
        Debug.Log(gameObject.tag + ", hover.");
        GameObject.FindGameObjectWithTag("message").GetComponent<UILabel>().text = gameObject.tag + ", hover.";
    }
    public void ResponseClick()
    {
        Debug.Log(gameObject.tag + ", click.");
        GameObject.FindGameObjectWithTag("message").GetComponent<UILabel>().text = gameObject.tag + ", click.";
    }
}