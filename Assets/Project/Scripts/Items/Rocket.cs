using UnityEngine;

public class Rocket : Item
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Initialize(ItemAsset asset, int x, int y)
    {
        base.Initialize(asset, x, y);
        if (spriteRenderer is not null)
            spriteRenderer.sprite = asset.mainSprite;
    }
}