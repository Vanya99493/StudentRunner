using System;

[Serializable]
public class RecordItem : IComparable<RecordItem>
{
    public string PlayerName;
    public int PlayerScore;

    public RecordItem(string playerName, int playerScore)
    {
        PlayerName = playerName;
        PlayerScore = playerScore;
    }

    public int CompareTo(RecordItem obj)
    {
        if (obj.PlayerScore == PlayerScore)
        {
            return obj.PlayerName.CompareTo(PlayerName);
        }
        else
        {
            return obj.PlayerScore.CompareTo(PlayerScore);
        }
    }
}