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
        if (spriteRenderer != null)
            spriteRenderer.sprite = asset.mainSprite;
    }

    public override void OnClicked()
    {
        GridManager.Instance.OnItemClicked(this);
    }
    
    public bool IsVertical => _asset.isVerticalRocket;
}