using System.Collections;
using System.Collections.Generic;
using LitJson;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField inpName, inpPassword;
    public TextMeshProUGUI lblName, lblPassword, lblHeadline;
    public UnityEngine.UI.Button btnLogin, btnMenu;

    void Start()
    {
    }

    private void Update()
    {
    }

    // Aufruf der Login-Methode
    public void callLogin()
    {
        StartCoroutine(Log_in());
    }

    // Log-in Coroutine
    IEnumerator Log_in()
    {
        // LoginDaten übermitteln
        WWWForm loginData = new WWWForm();
        loginData.AddField("name", inpName.text);
        loginData.AddField("password", inpPassword.text);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "login.php", loginData))
        {
            yield return www.SendWebRequest();

            // Fehler bei der Serververbindung
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Login fehlgeschlagen: Keine Verbindung zum Server!");
            }
            else
            {
                // Serverantwort als JSON parsen
                Debug.Log("Login erfolgreich!");
                string jsonResponse = www.downloadHandler.text;
                Debug.Log("Server Response: " + jsonResponse);

                // Versuche, die Antwort als JSON zu deserialisieren
             
                    // Die Antwort direkt in ein User-Objekt deserialisieren
                    var response = JsonMapper.ToObject<Dictionary<string, object>>(jsonResponse);

                if (response.ContainsKey("status") && response["status"].ToString() == "success")
                {
                    // Das User-Objekt direkt deserialisieren
                    Data.user = JsonMapper.ToObject<User>(jsonResponse);
                    if (Data.user != null)
                    {
                        Debug.Log("Benutzer ID: " + Data.user.id);
                        if (Data.user.usertype == 1)
                            SceneManager.LoadScene(8); // Betreuermodus -> Lade zu betreuende Personen
                        else
                            GetDiary();
                    }
                    else
                    {
                        Debug.LogError("Fehler: Das User-Objekt konnte nicht deserialisiert werden.");
                    }
                }
                else
                {
                    // Fehler, wenn der Status nicht "success" ist
                    string errorMessage = response.ContainsKey("message") ? response["message"].ToString() : "Unbekannter Fehler";
                    Debug.LogError("Login fehlgeschlagen: " + errorMessage);
                }
            }
        }
    }

    // Tagebucheinträge holen
    private void GetDiary()
    {
        DataManager.Instance.GetUserDiary(Data.user.id, diary =>
        {
            if (diary != null)
            {
                Data.Diary = diary;
                GetLevel();
            }
            else
            {
                Debug.LogError("Benutzertagebuch konnte nicht abgerufen werden.");

            }
        });
    }

    // Aktuelles Level holen
    private void GetLevel()
    {
        DataManager.Instance.GetLevelData(Data.user.userlevel, level =>
        {
            if (level != null)
            {
                Data.level = level;
                GetPlayedScenarios();
            }
            else
            {
                Debug.LogError("Level konnte nicht abgerufen werden.");

            }
        });
    }

    // Absolvierte Scenarien aus der Datenbank
    private void GetPlayedScenarios()
    {
        DataManager.Instance.GetPlayedScenarios(Data.user.id, playedScenarios =>
        {
            if (playedScenarios != null)
            {
                Data.scenariosPlayed = playedScenarios;
                SceneManager.LoadScene(3); // Spielmodus -> streakMenu
            }
            else
            {
                Debug.LogError("Szenarien konnten nicht abgerufen werden.");
            }
        });
    }

    // Login Button aktivieren, wird von ValueChanged Listenern aufgerufen
    public void enableLoginButton()
    {
        btnLogin.interactable = inpName.text.Length >= 8 && inpPassword.text.Length >= 8;
    }

    // Wechsel zum Menü
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}