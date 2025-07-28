public class Box : Obstacle
{
    public override void Initialize(string key, int x, int y)
    {
        base.Initialize(key, x, y);
        hitPoints = 1;
    }

    public override void TakeDamage(int amount)
    {
        hitPoints -= amount;
        if (hitPoints <= 0)
            GridManager.Instance.ClearItemAt(GridX, GridY);
    }
}