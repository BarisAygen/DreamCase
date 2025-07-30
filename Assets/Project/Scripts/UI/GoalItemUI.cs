using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalItemUI : MonoBehaviour
{
    [Header("Variables")]
    [Tooltip("Put prefab itself")] [SerializeField] private Image iconImage;
    [Tooltip("Put prefab's text")] [SerializeField] public TextMeshProUGUI countText;
    [Tooltip("Put goal check image")] [SerializeField] public Image doneCheck;

    private string _key;
    public string Key => _key;

    public void Setup(string key, Sprite icon, int count)
    {
        _key = key;
        iconImage.sprite = icon;
        countText.text = count.ToString();
        doneCheck.gameObject.SetActive(count <= 0);
        countText.gameObject.SetActive(count > 0);
    }
}