using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class Registration : MonoBehaviour
{
    public TMP_InputField inpName, inpEmail, inpPassword;
    public TextMeshProUGUI lblName, lblEmail, lblPassword, lblHeadline;
    public Button btnRegister;

    void Start()
    {
        SetFontsize();
    }

    private void Update()
    {
        SetFontsize();
    }

    private void SetFontsize()
    {
        lblName.fontSize = lblPassword.fontSize = lblHeadline.fontSize * 0.9f;
        inpName.textComponent.fontSize = lblHeadline.fontSize * 0.9f;
        inpName.placeholder.GetComponent<TextMeshProUGUI>().fontSize = lblHeadline.fontSize * 0.9f;
        inpPassword.textComponent.fontSize = lblHeadline.fontSize * 0.9f;
        inpPassword.placeholder.GetComponent<TextMeshProUGUI>().fontSize = lblHeadline.fontSize * 0.9f;
        btnRegister.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = lblHeadline.fontSize * 0.9f;
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        // Setze Daten für Registrierung
        WWWForm registrationData = new WWWForm();
        registrationData.AddField("Name", inpName.text);
        registrationData.AddField("password", inpPassword.text);
        registrationData.AddField("email", inpEmail.text);
        registrationData.AddField("registrationDate", System.DateTime.UtcNow.ToLocalTime().ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"));

        // Sende Registrierung an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "register.php", registrationData))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Registrierung fehlgeschlagen, keine Verbindung zum Server! Error: " + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                // Einfacher Vergleich der Serverantwort
                if (www.downloadHandler.text.Contains("Benutzer erfolgreich angelegt"))
                {
                    Debug.Log("Registrierung erfolgreich!");

                    // Wenn erfolgreich, lade die Startszene
                    SceneManager.LoadScene(0);
                }
                else
                {
                    Debug.LogError("Registrierung fehlgeschlagen: " + www.downloadHandler.text);
                }
            }
        }
    }
    
    // Button aktivieren wenn alle eingaben valide, wird von den ValueChanged Events aufgerufen
    public void enableRegisterButton()
    {
        btnRegister.interactable = inpName.text.Length >= 8 && inpPassword.text.Length >= 8 && IsValidEmail();
    }

    // Überprüfung auf valide email-Adresse
    private bool IsValidEmail()
    {
        // Regex check
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(inpEmail.text);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}