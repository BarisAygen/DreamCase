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
        bool triggeredCombo = false;

        Vector2Int[] dirs = new[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in dirs)
        {
            int nx = GridX + dir.x;
            int ny = GridY + dir.y;
            if (GridManager.Instance.IsInsideGrid(nx, ny))
            {
                var neighbor = GridManager.Instance.Grid[nx, ny];
                if (neighbor is Rocket r && r != this)
                {
                    triggeredCombo = true;
                    break;
                }
            }
        }

        if (triggeredCombo)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int colX = GridX + dx;
                if (!GridManager.Instance.IsInsideGrid(colX, GridY)) continue;

                for (int y = 0; y < GridManager.Instance.Height; y++)
                {
                    if (y == GridY && dx == 0) continue; 
                    yield return TryDestroy(colX, y);
                }
            }

            for (int dy = -1; dy <= 1; dy++)
            {
                int rowY = GridY + dy;
                if (!GridManager.Instance.IsInsideGrid(GridX, rowY)) continue;

                for (int x = 0; x < GridManager.Instance.Width; x++)
                {
                    if (x >= GridX - 1 && x <= GridX + 1 && rowY == GridY) continue; 
                    yield return TryDestroy(x, rowY);
                }
            }
        }
        else
        {
            if (_asset.isVerticalRocket)
            {
                for (int offset = 1; offset < GridManager.Instance.Height; offset++)
                {
                    int upY = GridY + offset;
                    int downY = GridY - offset;

                    if (GridManager.Instance.IsInsideGrid(GridX, upY))
                        yield return TryDestroy(GridX, upY);

                    if (GridManager.Instance.IsInsideGrid(GridX, downY))
                        yield return TryDestroy(GridX, downY);

                    if (!GridManager.Instance.IsInsideGrid(GridX, upY) &&
                        !GridManager.Instance.IsInsideGrid(GridX, downY))
                        break;
                }
            }
            else
            {
                for (int offset = 1; offset < GridManager.Instance.Width; offset++)
                {
                    int rightX = GridX + offset;
                    int leftX = GridX - offset;

                    if (GridManager.Instance.IsInsideGrid(rightX, GridY))
                        yield return TryDestroy(rightX, GridY);

                    if (GridManager.Instance.IsInsideGrid(leftX, GridY))
                        yield return TryDestroy(leftX, GridY);

                    if (!GridManager.Instance.IsInsideGrid(rightX, GridY) &&
                        !GridManager.Instance.IsInsideGrid(leftX, GridY))
                        break;
                }
            }
        }

        yield return new WaitForSeconds(0.05f);
        DestroySelf();
    }

    private IEnumerator TryDestroy(int x, int y)
    {
        var item = GridManager.Instance.Grid[x, y];
        if (item is null || item == this) yield break;

        yield return new WaitForSeconds(0.015f);

        if (item is IChainReactionItem special)
            ChainReactionManager.Instance.Enqueue(special);
        else
            item.DestroySelf();
    }
}