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
        if (!_asset.isDamageable) return; 

        Health--;
        ParticleManager.Instance.PlayObstacleDamageParticle(transform.position, this._asset.key);
        if (Health <= 0)
        {
            DestroySelf();
        }
        else if (_asset.damagedSprite is not null && TryGetComponent<SpriteRenderer>(out var sr))
        {
            sr.sprite = _asset.damagedSprite;
        }
    }
}