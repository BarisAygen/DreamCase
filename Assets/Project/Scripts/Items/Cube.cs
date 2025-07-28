using UnityEngine;

public class Cube : Item
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Sprite normalSprite;
    private Sprite hintedSprite;
    private bool isHinted;

    public void SetSprites(Sprite normal, Sprite hint)
    {
        normalSprite = normal;
        hintedSprite = hint;
        spriteRenderer.sprite = normalSprite;
    }

    public void SetHintState(bool hint)
    {
        isHinted = hint;
        spriteRenderer.sprite = hint ? hintedSprite : normalSprite;
    }

    public override void OnClicked()
    {
        GridManager.Instance.OnCubeClicked(this);
    }
}