using System;
using UnityEngine;

public static class GameEventManager
{
    public static event Action<Item> OnItemClicked;
    public static event Action<Item> OnItemDestroyed;
    public static event Action<bool> OnGameOver;
    public static event Action OnAllActionsComplete;

    public static void ItemClicked(Item item) => OnItemClicked?.Invoke(item);
    public static void ItemDestroyed(Item item) => OnItemDestroyed?.Invoke(item);

    public static void GameOver(bool win) => OnGameOver?.Invoke(win);
    public static void AllActionsComplete() => OnAllActionsComplete?.Invoke();
}