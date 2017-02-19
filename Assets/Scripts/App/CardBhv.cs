using UnityEngine;

namespace App
{
    public class CardBhv : MonoBehaviour
    {
        void Start()
        {
        }


        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter(Collider e)
        {
            Debug.Log("collider tag is : " + e.gameObject.tag);
            if (e.gameObject.tag.Equals("card11"))
            {
                transform.Rotate(new Vector3(80, 80, 80), 160f);
                var go = GameObject.FindGameObjectWithTag("card11");
                go.transform.Rotate(new Vector3(90, 0, 0));
                Destroy(gameObject);
            }
        }
    }
}