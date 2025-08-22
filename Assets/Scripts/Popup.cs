using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject popupPanel;  
    public TextMeshProUGUI popupMessageText;  
    public Button okButton;
    public Game game;
    void Start()
    {
        popupPanel.SetActive(false); 
        okButton.onClick.AddListener(OnOkClicked);  
    }

    public void ShowPopup(string message)
    {
        Debug.Log("Popup message: " + message); 
        popupMessageText.text = message;  
        popupPanel.SetActive(true);
    }

    public void hidePopup()
    {
        popupPanel.SetActive(false);
    }

    public void OnOkClicked()
    {
        popupPanel.SetActive(false);
        game.setPauseAtFalse();
    }
}