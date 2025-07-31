using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private Camera _cam;

    [SerializeField] private LayerMask itemLayer;

    private float tapCooldown = 0.1f;
    private float lastTapTime = 0f;

    private void Start() => _cam = Camera.main;

    private void Update()
    {
        if (Time.time - lastTapTime < tapCooldown || GridManager.Instance.isInputLocked) return;

        if (!GetTapPosition(out Vector2 screenPos)) return;

        lastTapTime = Time.time;

        Vector2 world = _cam.ScreenToWorldPoint(screenPos);

        if (Physics2D.Raycast(world, Vector2.zero, 0f, itemLayer).collider is Collider2D col &&
            col.TryGetComponent<Item>(out var item))
        {
            item.OnClicked(); 
        }
    }

    private bool GetTapPosition(out Vector2 pos)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            pos = Input.GetTouch(0).position;
            return true;
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
            return true;
        }
#endif
        pos = default;
        return false;
    }
}