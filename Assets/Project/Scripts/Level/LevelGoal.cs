[System.Serializable]
public class LevelGoal
{
    public string key;      
    public int count;

    public LevelGoal(string key, int count)
    {
        this.key = key;
        this.count = count;
    }
}