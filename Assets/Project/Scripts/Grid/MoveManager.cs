using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public static MoveManager Instance { get; private set; }
    public int MoveCount { get; private set; }

    private void Awake() => Instance = this;

    public void Initialize(int startingMoves)
    {
        MoveCount = startingMoves;
        LevelUI.Instance.UpdateMoveCount(MoveCount);
    }

    public bool TryConsumeMove()
    {
        MoveCount--;
        LevelUI.Instance.UpdateMoveCount(MoveCount);

        if (LevelUI.Instance.AreAllGoalsCompleted())
        {
            GameEventManager.GameOver(true);
        }
        else if (MoveCount <= 0)
        {
            GameEventManager.GameOver(false);
        }

        return true;
    }
}