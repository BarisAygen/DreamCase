using UnityEngine;

public class Obstacle : Item
{
    public int Health { get; private set; }

    public override void Initialize(ItemAsset asset, int x, int y)
    {
        base.Initialize(asset, x, y);
        Health = asset.maxHealth;
    }

    public void TakeDamage()
    {
        Health--;
        if (Health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (_asset.damagedSprite != null && TryGetComponent<SpriteRenderer>(out var sr))
            sr.sprite = _asset.damagedSprite;
    }

    public void SetHealth(int health)
    {
        Health = health;
    }
}