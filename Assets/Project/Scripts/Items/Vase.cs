using UnityEngine;

public class Vase : MonoBehaviour
{
    public GameObject vase2Prefab; 

    private int health = 2;

    public void TakeDamage()
    {
        health--;

        if (health == 1)
        {
            GameObject vase2 = Instantiate(vase2Prefab, transform.position, Quaternion.identity, transform.parent);
            Destroy(gameObject);
        }
        else if (health <= 0)
        {
            // Destroy vase2
            Destroy(gameObject);
        }
    }
}
