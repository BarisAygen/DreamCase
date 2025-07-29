using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainReactionManager : MonoBehaviour
{
    public static ChainReactionManager Instance { get; private set; }

    private Queue<IChainReactionItem> queue = new();
    private bool isRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
    }

    public void StartChainWith(IChainReactionItem item)
    {
        if (isRunning) return;
        StartCoroutine(RunChain(item));
    }

    public void Enqueue(IChainReactionItem item)
    {
        queue.Enqueue(item);
    }

    private IEnumerator RunChain(IChainReactionItem rootItem)
    {
        isRunning = true;
        ActionTracker.Instance.StartAction();

        Enqueue(rootItem);

        while (queue.Count > 0)
        {
            IChainReactionItem current = queue.Dequeue();
            yield return current.ExecuteEffectSequence();
        }

        isRunning = false;
        ActionTracker.Instance.EndAction();
    }
}