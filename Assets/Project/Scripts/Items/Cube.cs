using UnityEngine;

public class Cube : Item
{
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Sprite normalSprite;
    private Sprite hintSprite;

    public override void Initialize(ItemAsset asset, int x, int y)
    {
        base.Initialize(asset, x, y);
        SetSprites(asset.mainSprite, asset.hintedSprite); 
    }

    public void SetSprites(Sprite normal, Sprite hint)
    {
        normalSprite = normal;
        hintSprite = hint;
        spriteRenderer.sprite = normalSprite;
    }

    public void SetHintState(bool isHint)
    {
        spriteRenderer.sprite = isHint ? hintSprite : normalSprite;
    }
}