using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalItemUI : MonoBehaviour
{
    [Header("Variables")]
    [Tooltip("Put prefab itself")] [SerializeField] private Image iconImage;
    [Tooltip("Put prefab's text")] [SerializeField] private TextMeshProUGUI countText;

    public void Setup(Sprite icon, int count)
    {
        iconImage.sprite = icon;
        countText.text = $"{count}";
    }
}