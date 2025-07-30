using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance { get; private set; }

    private Dictionary<string, int> _goalCounts = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize(List<LevelGoal> goals)
    {
        _goalCounts = goals.ToDictionary(g => g.key, g => g.count);

        LevelUI.Instance.RenderGoals(goals);
    }

    public void DecreaseGoal(string key)
    {
        if (!_goalCounts.ContainsKey(key)) return;

        _goalCounts[key] = Mathf.Max(0, _goalCounts[key] - 1);
        LevelUI.Instance.UpdateGoalUI(key, _goalCounts[key]);
    }

    public bool AreAllGoalsCompleted()
    {
        return _goalCounts.Values.All(count => count <= 0);
    }
}