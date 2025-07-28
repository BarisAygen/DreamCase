using UnityEngine;

public class Vase : Obstacle
{
    private bool damagedThisTurn;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite intactSprite;
    [SerializeField] private Sprite damagedSprite;

    public override void Initialize(string key, int x, int y)
    {
        base.Initialize(key, x, y);
        hitPoints = 2;
        damagedThisTurn = false;

        // Başlangıçta sağlam sprite'ı göster
        if (spriteRenderer != null && intactSprite != null)
            spriteRenderer.sprite = intactSprite;
    }

    public override void TakeDamage(int amount)
    {
        if (damagedThisTurn) return;
        damagedThisTurn = true;
        hitPoints--;

        // 1 can kaldıysa sprite değiştir
        if (hitPoints == 1 && damagedSprite != null && spriteRenderer != null)
            spriteRenderer.sprite = damagedSprite;

        if (hitPoints <= 0)
            GridManager.Instance.ClearItemAt(GridX, GridY);
    }

    public void ResetTurnDamage() => damagedThisTurn = false;
}