using UnityEngine;

public enum RocketType
{
    Horizontal,
    Vertical
}

public class Rocket : Item
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite hSprite, vSprite;

    private RocketType type;

    public void SetType(RocketType t)
    {
        type = t;
        sr.sprite = (type == RocketType.Horizontal) ? hSprite : vSprite;
    }

    public override void OnClicked()
    {
        if (type == RocketType.Horizontal)
            GridManager.Instance.ClearRow(GridY);
        else
            GridManager.Instance.ClearColumn(GridX);
    }
}