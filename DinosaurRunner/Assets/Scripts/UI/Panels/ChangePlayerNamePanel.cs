using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerNamePanel : MonoBehaviour
{
    [SerializeField] private InputField _playerNameInputField;

    public string PlayerName { get { return _playerNameInputField.text; } }
}