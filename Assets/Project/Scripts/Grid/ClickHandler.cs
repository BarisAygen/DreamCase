using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private Camera _cam;

    private void Start() => _cam = Camera.main;

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Vector2 world = _cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 screen = Input.mousePosition;
        if (Physics2D.Raycast(world, Vector2.zero).collider is Collider2D col &&
            col.TryGetComponent<Item>(out var item))
        {
            item.OnClicked();
        }
    }
}