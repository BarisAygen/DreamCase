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
        if (spriteRenderer != null)
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
        Vector2Int[] dirs = new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
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
            int halfSize = 1;
            int maxOffsetX = Mathf.Max(GridX, GridManager.Instance.Width - GridX - 1);
            int maxOffsetY = Mathf.Max(GridY, GridManager.Instance.Height - GridY - 1);
            int maxOffset = Mathf.Max(maxOffsetX, maxOffsetY);

            for (int offset = 1; offset <= maxOffset; offset++)
            {
                if (offset <= maxOffsetX)
                {
                    int xPos = GridX + offset;
                    int xNeg = GridX - offset;
                    for (int dy = -halfSize; dy <= halfSize; dy++)
                    {
                        int y = GridY + dy;
                        if (GridManager.Instance.IsInsideGrid(xPos, y))
                            StartCoroutine(TryDestroy(xPos, y));
                        if (GridManager.Instance.IsInsideGrid(xNeg, y))
                            StartCoroutine(TryDestroy(xNeg, y));
                    }
                }
                if (offset <= maxOffsetY)
                {
                    int yPos = GridY + offset;
                    int yNeg = GridY - offset;
                    for (int dx = -halfSize; dx <= halfSize; dx++)
                    {
                        int x = GridX + dx;
                        if (GridManager.Instance.IsInsideGrid(x, yPos))
                            StartCoroutine(TryDestroy(x, yPos));
                        if (GridManager.Instance.IsInsideGrid(x, yNeg))
                            StartCoroutine(TryDestroy(x, yNeg));
                    }
                }
                yield return new WaitForSeconds(0.05f);
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
                    if (!GridManager.Instance.IsInsideGrid(GridX, upY) && !GridManager.Instance.IsInsideGrid(GridX, downY))
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
                    if (!GridManager.Instance.IsInsideGrid(rightX, GridY) && !GridManager.Instance.IsInsideGrid(leftX, GridY))
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
        if (item == null || item == this)
            yield break;
        yield return new WaitForSeconds(0.015f);
        if (item is IChainReactionItem special)
            ChainReactionManager.Instance.Enqueue(special);
        else
            item.DestroySelf();
    }
}