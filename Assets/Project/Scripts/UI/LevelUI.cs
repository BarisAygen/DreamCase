using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private RectTransform goalContainer;
    [SerializeField] private GoalItemUI goalItemPrefab;
    [SerializeField] private GoalIconLibrary iconLibrary;

    private void Start()
    {
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        LevelData data = LevelDataLoader.LoadLevelData(level);

        if (data != null)
        {
            movesText.text = $"{data.move_count}";
            RenderGoals(data);
        }
        else
        {
            movesText.text = "?";
            Debug.LogError($"Level {level} data not found.");
        }
    }

    private void RenderGoals(LevelData data)
    {
        foreach (Transform child in goalContainer)
        {
            Destroy(child.gameObject);
        }

        int count = data.goals.Count;
        float spacing = 55f;
        float size = GetGoalSize(count);

        for (int i = 0; i < count; i++)
        {
            var goal = data.goals[i];
            var item = Instantiate(goalItemPrefab, goalContainer);
            var icon = iconLibrary.GetIcon(goal.key);
            item.Setup(icon, goal.count);

            RectTransform rt = item.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(110f, 110f); 
            rt.localScale = Vector3.one * (size / 110f); 
            rt.anchoredPosition = GetGoalPosition(i, count, spacing);
        }
    }

    private float GetGoalSize(int count)
    {
        switch (count)
        {
            case 1: return 100f;
            case 2: return 80f;
            case 3: return 70f;
            case 4: return 55f;
            default: return 30f;
        }
    }

    private Vector2 GetGoalPosition(int index, int count, float spacing)
    {
        switch (count)
        {
            case 1:
                return Vector2.zero;

            case 2:
                return new Vector2((index == 0 ? -1 : 1) * spacing, 0);

            case 3:
                if (index == 0) return new Vector2(-spacing, spacing / 1.5f);
                if (index == 1) return new Vector2(spacing, spacing / 1.5f);
                return new Vector2(0, -spacing / 1.2f); 

            case 4:
                return new Vector2(
                    (index % 2 == 0 ? -1 : 1) * spacing,
                    (index < 2 ? spacing : -spacing)
                );

            default:
                int cols = Mathf.CeilToInt(Mathf.Sqrt(count));
                int row = index / cols;
                int col = index % cols;
                float x = (col - (cols - 1) / 2f) * spacing;
                float y = -((row - (cols - 1) / 2f) * spacing);
                return new Vector2(x, y);
        }
    }
}