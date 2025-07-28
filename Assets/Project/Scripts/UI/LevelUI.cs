using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private RectTransform goalContainer;
    [SerializeField] private GoalItemUI goalItemPrefab;
    [SerializeField] private GoalIconLibrary iconLibrary;

    [Header("Layout Settings")]
    [SerializeField] private float spacing = 55f;            
    [SerializeField] private float[] sizeByCount = { 110f, 90f, 80f, 60f };                            

    private void Awake()
    {
        Instance = this;
    }

    /// Called by LevelInitializer once LevelData is ready.
    public void SetupLevel(LevelData data)
    {
        movesText.text = data.move_count.ToString();
        RenderGoals(data.goals);
    }

    /// Updates the move counter from GridManager.
    public void UpdateMoveCount(int count)
    {
        movesText.text = count.ToString();
    }

    private void RenderGoals(List<LevelGoal> goals)
    {
        // 🔥 Önce tüm eski hedefleri temizle
        foreach (Transform child in goalContainer)
        {
            Destroy(child.gameObject);
        }

        int count = goals.Count;
        float size = count <= sizeByCount.Length ? sizeByCount[count - 1] : sizeByCount[^1];

        for (int i = 0; i < count; i++)
        {
            var goal = goals[i];
            var item = Instantiate(goalItemPrefab, goalContainer);
            item.gameObject.SetActive(true);

            var icon = iconLibrary.GetIcon(goal.key);
            item.Setup(icon, goal.count);

            var rt = item.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(110f, 110f); 
            rt.localScale = Vector3.one * (size / 110f); 
            rt.anchoredPosition = GetGoalPosition(i, count);
        }
    }

    private Vector2 GetGoalPosition(int index, int count)
    {
        switch (count)
        {
            case 1:  return Vector2.zero;
            case 2:  return new Vector2((index == 0 ? -1 : 1) * spacing, 0);
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
                int row  = index / cols;
                int col  = index % cols;
                float x  = (col - (cols - 1) / 2f) * spacing;
                float y  = -((row - (cols - 1) / 2f) * spacing);
                return new Vector2(x, y);
        }
    }
}