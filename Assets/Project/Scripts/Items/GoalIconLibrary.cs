using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoalIconLibrary", menuName = "ScriptableObjects/GoalIconLibrary", order = 1)]
public class GoalIconLibrary : ScriptableObject
{
    [System.Serializable]
    public class GoalIconEntry
    {
        public string key;
        public Sprite icon;
    }

    [SerializeField] private List<GoalIconEntry> iconEntries;

    private Dictionary<string, Sprite> _iconMap;

    private void OnEnable()
    {
        _iconMap = new Dictionary<string, Sprite>();
        foreach (var entry in iconEntries)
        {
            if (!string.IsNullOrEmpty(entry.key) && entry.icon != null)
            {
                _iconMap[entry.key] = entry.icon;
            }
        }
    }

    public Sprite GetIcon(string key)
    {
        return _iconMap != null && _iconMap.TryGetValue(key, out Sprite sprite) ? sprite : null;
    }
}