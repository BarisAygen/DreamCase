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
    
    public override bool TryActivate()
    {
        var group = GameManager.Instance.MatchService.FindConnectedGroup(GridManager.Instance.Grid, GridX, GridY, Key);
        if (group.Count < 2) return false;

        GridManager.Instance.DamageObstaclesAroundGroup(group);

        GameManager.Instance.MatchService.RemoveGroup(GridManager.Instance.Grid, group);

        if (group.Count >= 4)
        {
            string rocketKey = UnityEngine.Random.Range(0, 2) == 0 ? "hro" : "vro";
            var rocket = GameManager.Instance.TileSpawner.Spawn(rocketKey, GridX, GridY, transform.position, GridManager.Instance.GridParent);
            rocket.SetGridPosition(GridX, GridY);
            GridManager.Instance.Grid[GridX, GridY] = rocket;
        }

        return true;
    }
}