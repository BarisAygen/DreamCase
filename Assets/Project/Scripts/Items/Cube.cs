using UnityEngine;

public class Cube : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite horizontalRocketSprite;
    public Sprite verticalRocketSprite;

    private RocketDirection rocketDirection = RocketDirection.None;
    private SpriteRenderer sr;

    private void Awake() => sr = GetComponent<SpriteRenderer>();

    public void SetRocketState(RocketDirection direction)
    {
        rocketDirection = direction;

        if (direction == RocketDirection.Horizontal)
            sr.sprite = horizontalRocketSprite;
        else if (direction == RocketDirection.Vertical)
            sr.sprite = verticalRocketSprite;
        else
            sr.sprite = defaultSprite;
    }

    public bool IsRocket() => rocketDirection != RocketDirection.None;
    public RocketDirection GetDirection() => rocketDirection;
}
