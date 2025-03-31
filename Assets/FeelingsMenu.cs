using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class FeelingsMenu : MonoBehaviour 
{
    public Toggle toggle1, toggle2, toggle3, toggle4, toggle5;
    public TMP_InputField feedbackText;
    public TMP_Text lastEntryText, lastEntryHeadline;
    public Button sendButton;
    public ToggleGroup feelingToggles;

    private void Start()
    {
        if (Data.Diary != null && Data.Diary.entries.Count != 0)
        {
            lastEntryText.text = Data.Diary.getLastEntry().entry;
            lastEntryHeadline.text = "Eintrag vom " + Data.Diary.getLastEntry().entrydate.ToString();
        }
    }

    // Button aktivieren
    public void enableSendButton()
    {
        sendButton.interactable = true;
        feelingToggles.allowSwitchOff = false;
    }

    public void callSendFeedback()
    {
        StartCoroutine(sendFeedback());
    }

    IEnumerator sendFeedback()
    {
        // Daten packen
        WWWForm diaryEntry = new WWWForm();
        diaryEntry.AddField("userId", Data.user.id);
        diaryEntry.AddField("feeling", feelingToggles.GetFirstActiveToggle().name); // Aktives Toggle
        diaryEntry.AddField("entry", feedbackText.text);
        diaryEntry.AddField("date", System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss"));

        // An Server und DB senden
        using var www = UnityWebRequest.Post(Server.ip + "diaryentry.php", diaryEntry);
        yield return www.SendWebRequest();

        // connection error
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Eintrag fehlgeschlagen: Keine Verbindung zum Server!");
            Debug.Log(www.downloadHandler.text);

        }

        else
        {
            yield return www.downloadHandler.text;
            UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }
    }
}