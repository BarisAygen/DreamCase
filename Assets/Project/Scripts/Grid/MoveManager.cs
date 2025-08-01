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

    public bool ConsumeMove()
    {
        MoveCount--;
        LevelUI.Instance.UpdateMoveCount(MoveCount);
        return true;
    }
}