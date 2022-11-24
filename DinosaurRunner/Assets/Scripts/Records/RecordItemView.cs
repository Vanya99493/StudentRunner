using UnityEngine;
using UnityEngine.UI;

public class RecordItemView : MonoBehaviour
{
    public void SetValues(string playerName, int playerScore)
    {
        transform.GetChild(0).GetComponent<Text>().text = playerName;
        transform.GetChild(1).GetComponent<Text>().text = playerScore.ToString();
    }
}