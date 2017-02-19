using UnityEngine;

namespace App.Helper
{
    public class ScreenHelper
    {
        public static Rect GUIRectWithObject(GameObject go)
        {
            //Bounds bounds = go.GetComponent<Collider>().bounds;
            Bounds bounds = go.GetComponent<Renderer>().bounds;

            // Get mesh origin and farthest extent (this works best with simple convex meshes)
            Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
            Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

            // Create rect in screen space and return - does not account for camera perspective
            return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
        }

        public static Vector2 ScreenRect()
        {
            return new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        }
    }
}