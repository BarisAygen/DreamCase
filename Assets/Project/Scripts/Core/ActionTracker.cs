using UnityEngine;
using System;

public class ActionTracker : MonoBehaviour
{
    public static ActionTracker Instance { get; private set; }

    public static event Action OnAllActionsComplete;

    private int pendingActions = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartAction()
    {
        pendingActions++;
    }

    public void EndAction()
    {
        pendingActions = Mathf.Max(0, pendingActions - 1);
        if (pendingActions == 0)
            OnAllActionsComplete?.Invoke();
    }

    public bool HasPendingActions() => pendingActions > 0;
}