using System.Collections;
using UnityEngine;

public class Rocket : Item, IChainReactionItem
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

    public override bool TryActivate()
    {
        ChainReactionManager.Instance.StartChainWith(this);
        return true;
    }

    public IEnumerator ExecuteEffectSequence()
    {
        if (_asset.isVerticalRocket)
        {
            for (int y = 0; y < GridManager.Instance.Height; y++)
            {
                var item = GridManager.Instance.Grid[GridX, y];
                if (item is null || item == this) continue;

                yield return new WaitForSeconds(0.05f);

                if (item is Rocket r)
                    ChainReactionManager.Instance.Enqueue(r);
                else
                    item.DestroySelf();
            }
        }
        else
        {
            for (int x = 0; x < GridManager.Instance.Width; x++)
            {
                var item = GridManager.Instance.Grid[x, GridY];
                if (item is null || item == this) continue;

                yield return new WaitForSeconds(0.05f);

                if (item is IChainReactionItem specialItem)
                    ChainReactionManager.Instance.Enqueue(specialItem);
                else
                    item.DestroySelf();
            }
        }

        yield return new WaitForSeconds(0.1f);
        DestroySelf();
    }
}