using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button btnHelp;

    void Start()
    {
       if (!Data.scenariosPlayed.GetLatestScenarioDate().Date.Equals(System.DateTime.UtcNow.ToLocalTime().Date))
        {
            btnHelp = GameObject.Find("btnHelpBen").GetComponent<Button>();
            btnHelp.interactable = true;
            TMP_Text btnText = btnHelp.GetComponentInChildren<TMP_Text>();
            btnText.text = "Hilf Ben!";
        }
    }

    public void GoToScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}