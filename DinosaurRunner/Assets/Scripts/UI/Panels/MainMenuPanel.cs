using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Text _playerNameText;

    public void ChangePlayerName(string newPlayerName)
    {
        _playerNameText.text = newPlayerName;
    }
}