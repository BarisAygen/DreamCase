using UnityEngine;

public class Cube : Item
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool _isHinted;
    private Sprite normalSprite;
    private Sprite hintSprite;

    public override void Initialize(ItemAsset asset, int x, int y)
    {
        base.Initialize(asset, x, y);
        SetSprites(asset.mainSprite, asset.hintedSprite); // ✅ Hemen burada başlat
    }

    public void SetSprites(Sprite normal, Sprite hint)
    {
        normalSprite = normal;
        hintSprite = hint;
        spriteRenderer.sprite = normalSprite;
    }

    public void SetHintState(bool isHint)
    {
        _isHinted = isHint;
        spriteRenderer.sprite = isHint ? hintSprite : normalSprite;
    }

    public override void OnClicked()
    {
        GridManager.Instance.OnItemClicked(this);
    }
}