using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdvisorMenu : MonoBehaviour
{
    public TextMeshProUGUI txtSelectUser, txtUserId, txtUsername, txtName, txtEmail, txtRegistration, txtLastlogin;
    public TextMeshProUGUI txtStreak, txtRecordstreak, txtDiaryentries, txtPlayedscenarios;
    public Button btnData, btnScenarioPlan, btnScenarioStatistics;

    public TMP_Dropdown ddSelectUser;

    void Start()
    {
        if (Data.advisedPersons.Count > 0 && Data.advisedPerson != null)
        {
            SetScreen();
        }
    }



    void Update()
    {
    }


    private void SetScreen()
    {
        // Erstelle die Dropdown-Optionen
        List<string> options = Data.advisedPersons
            .Select(ap => $"{ap.user.id}, {ap.user.username}") // Id, username als angezeigte Option
            .ToList();

        // Setze die Dropdown-Optionen
        ddSelectUser.ClearOptions();
        ddSelectUser.AddOptions(options);

        // Setze den Dropdown-Index auf den ersten Eintrag, wenn Data.advisedPerson gesetzt ist
        ddSelectUser.value = 0;

        // Füge den Event-Listener hinzu
        ddSelectUser.onValueChanged.AddListener(OnDropdownValueChanged);
        SetTexts();
        btnData.interactable = true;
        btnScenarioPlan.interactable = true;
        btnScenarioStatistics.interactable = true;
        ddSelectUser.interactable = true;
    }

    private void SetTexts()
    {
        txtUserId.text = Data.advisedPerson.user.id.ToString();
        txtUsername.text = Data.advisedPerson.user.username;
        txtName.text = Data.advisedPerson.user.name;
        txtEmail.text = Data.advisedPerson.user.email;
        txtRegistration.text = Data.advisedPerson.user.registration;
        txtLastlogin.text = Data.advisedPerson.user.lastlogin;

        txtStreak.text = Data.user.streak.ToString();
        txtRecordstreak.text = Data.user.recordstreak.ToString();
        txtDiaryentries.text = Data.advisedPerson.diary.entries.Count.ToString();
        txtPlayedscenarios.text = Data.advisedPerson.scenariosPlayed.scenariosPlayed.Count.ToString();
    }

    public void LoadUsers()
    {
        DataManager.Instance.GetAdvisedUsers(Data.user.id, users =>
        {
            if (users != null && users.Count > 0)
            {
                List<AdvisedPerson> loadedPersons = new List<AdvisedPerson>();
                int totalUsers = users.Count;
                int loadedCount = 0;

                foreach (User user in users)
                {
                    // Erstelle eine neue AdvisedPerson mit einem Callback
                    new AdvisedPerson(user, advisedPerson =>
                    {
                        loadedPersons.Add(advisedPerson);
                        loadedCount++;

                        if (loadedCount == totalUsers)
                        {
                            // Füge die geladenen AdvisedPersons zur Data.advisedPersons-Liste hinzu
                            Data.advisedPersons.AddRange(loadedPersons);

                            // Setze Data.advisedPerson auf den ersten Benutzer aus der Liste
                            Data.advisedPerson = loadedPersons[0];

                            SetScreen();
                        }
                    });
                }
            }
            else
            {
                // Keine Benutzer gefunden, füge "Keine Benutzer gefunden" in das Dropdown ein
                List<string> noUserOption = new List<string> { "Keine Benutzer gefunden" };

                // Setze die Dropdown-Optionen
                ddSelectUser.ClearOptions();
                ddSelectUser.AddOptions(noUserOption);

                // Füge den Klick-Listener hinzu
                ddSelectUser.onValueChanged.AddListener(OnDropdownValueChanged);

                Debug.Log("Keine Benutzer gefunden.");
            }
        });
    }

    private void OnDropdownValueChanged(int index)
    {
        // Überprüfe, ob eine gültige Auswahl getroffen wurde
        if (index >= 0 && index < Data.advisedPersons.Count)
        {
            // Setze den entsprechenden User in Data.advisedPerson
            Data.advisedPerson = Data.advisedPersons[index];
            SetTexts();
        }
    }

    public void GoToMenu(int i)
    {
        SceneManager.LoadScene(i);
    }
}