using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public string Key { get; private set; }
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public void Initialize(string key, int x, int y, Sprite sprite)
    {
        Key = key;
        GridX = x;
        GridY = y;
        spriteRenderer.sprite = sprite;
    }
}