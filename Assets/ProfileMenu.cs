using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Profile : MonoBehaviour

{
    public Button btnSave, btnSaveAdvisor;
    public TMP_InputField txtName, txtEmail, txtPassword, txtAdvisor;
    public TMP_Text txtLevel, txtPoints, txtDiaryEntries, txtLongeststreak, txtID, txtuserName, txtStreak, txtPlayedScenarios;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTexts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTexts()
    {
        txtLevel.text = Data.user.userlevel.ToString();
        txtPoints.text = Data.user.userpoints.ToString();
        txtDiaryEntries.text = Data.Diary.entries.Count.ToString();
        txtLongeststreak.text = Data.user.recordstreak.ToString();
        txtStreak.text = Data.user.streak.ToString();
        txtPlayedScenarios.text = Data.scenariosPlayed.scenariosPlayed.Count.ToString();

        txtID.text = Data.user.id.ToString();
        txtuserName.text = Data.user.username;
        txtName.text = Data.user.name;
        txtEmail.text = Data.user.email;
        btnSave.interactable = false;
    }

    // Enable button on valid inputs
    public void EnableBtnSave()
    {
        btnSave.interactable = string.IsNullOrEmpty(txtPassword.text) || txtPassword.text.Length >= 8 &&
            (!string.Equals(Data.user.name, txtName.text) || !string.Equals(Data.user.email, txtEmail.text) && IsValidEmail());
    }

    private bool IsValidEmail()
    {
        // Regex check
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(txtEmail.text);
    }

    public void Save()
    {
        StartCoroutine(UpdateUserdata());
    }

    IEnumerator UpdateUserdata()
    {
        // Package data for registration into WWWForm
        WWWForm updateUser = new WWWForm();
        updateUser.AddField("userId", Data.user.id);

        if (!string.IsNullOrEmpty(txtPassword.text) && txtPassword.text.Length >= 8)
        {
            updateUser.AddField("password", txtPassword.text);
            Debug.Log("PW set");
        }

        updateUser.AddField("email", txtEmail.text);
        Debug.Log("Neue Email: " + txtEmail.text);
        updateUser.AddField("name", txtName.text);
        Debug.Log("Neuer Name: " + txtName.text);


        // Sende die Daten an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "updateuser.php", updateUser))
        {
            yield return www.SendWebRequest();

            // Überprüfen, ob die Anfrage erfolgreich war
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Update fehlgeschlagen! Error: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);  // Optional, um genauere Fehler zu bekommen
                Debug.LogError("Server Response: " + www.downloadHandler.text); // Optional, für detaillierte Serverantwort
            }
            else
            {
                // Aktualisiere die Benutzerdaten im lokalen Data-Objekt
                Data.user.email = txtEmail.text;
                Data.user.name = txtName.text;

                Debug.Log(www.downloadHandler.text);
            }
        }
    }



    public void SetAdvisor()
    {
        DataManager.Instance.SetAdvisor(Data.user.id, int.Parse(txtAdvisor.text), success =>
        {
            if (success)
            {
                Data.user.advisorid = int.Parse(txtAdvisor.text);
            }
            else
            {
                Debug.LogError(success.ToString());
            }
        });
    }

    public void EnableSaveAdvisorButton()
    {
        btnSaveAdvisor.interactable = int.TryParse(txtAdvisor.text, out int parsedValue);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(5);
    }
}