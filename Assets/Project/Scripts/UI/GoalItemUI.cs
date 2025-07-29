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
    private int _count;
    public bool IsDone { get; private set; }
    public string Key => _key;

    public void Setup(string key, Sprite icon, int count)
    {
        _key = key;
        _count = count;
        iconImage.sprite = icon;
        countText.text = $"{count}";
    }

    public void DecreaseCount()
    {
        _count = Mathf.Max(0, _count - 1);
        countText.text = $"{_count}";

        if (_count == 0)
            IsDone = true;
    }
}