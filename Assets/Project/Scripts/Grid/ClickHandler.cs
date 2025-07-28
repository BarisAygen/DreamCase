using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Cube cube = hit.collider.GetComponent<Cube>();
                if (cube != null)
                    GridManager.Instance.OnCubeClicked(cube);
            }
        }
    }
}