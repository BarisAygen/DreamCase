public abstract class Obstacle : Item
{
    protected int hitPoints;
    public abstract void TakeDamage(int amount);
}