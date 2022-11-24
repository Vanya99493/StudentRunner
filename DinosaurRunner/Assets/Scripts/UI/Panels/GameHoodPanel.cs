using UnityEngine;
using UnityEngine.UI;

public class GameHoodPanel : MonoBehaviour
{
    [SerializeField] private MobileControllerPanel _mobileControllerPanel;
    [SerializeField] private Text _scoreText;

    private int _score;

    public void ControlPlatform(bool isMobile)
    {
        if (!isMobile)
        {
            _mobileControllerPanel.gameObject.SetActive(false);
        }
    }

    public void UpdateScore(int newScore)
    {
        _score = newScore;
        _scoreText.text = _score.ToString();
    }

    public int GetScore()
    {
        return _score;
    }
}