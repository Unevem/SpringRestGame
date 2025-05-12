using UnityEngine;

public class BuildCursor : MonoBehaviour
{

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}