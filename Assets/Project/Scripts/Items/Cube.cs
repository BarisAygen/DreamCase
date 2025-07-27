using UnityEngine;

public class Cube : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite rocketSprite;
    private SpriteRenderer sr;

    private void Awake() => sr = GetComponent<SpriteRenderer>();

    public void SetRocketState()
    {

        if (false)
        {
            sr.sprite = rocketSprite;
        }
        else
            sr.sprite = defaultSprite;
    }
}
