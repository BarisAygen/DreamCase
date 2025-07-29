using System;
using UnityEngine;

public static class GameEventManager
{
    public static event Action<Item> OnItemClicked;
    public static event Action<Item> OnItemDestroyed;
    public static event Action OnMoveUsed;

    public static void ItemClicked(Item item) => OnItemClicked?.Invoke(item);
    public static void ItemDestroyed(Item item) => OnItemDestroyed?.Invoke(item);
    public static void MoveUsed() => OnMoveUsed?.Invoke();
}