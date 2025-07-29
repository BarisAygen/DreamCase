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
        if (MoveCount <= 0) return false;

        MoveCount--;
        LevelUI.Instance.UpdateMoveCount(MoveCount);

        return true;
    }
}