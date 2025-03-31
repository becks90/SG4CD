using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using static AddScenario;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Verhindert die Zerstörung bei Szenenwechsel
        }
        else
        {
            Destroy(gameObject); // Zerstöre die Instanz, wenn schon eine existiert
        }
    }

    // Hilfsmethode zur Berechnung von Level und Punkten
    private (int, int) GetPointsAndLevel(int points)
    {
        var currentPoints = Data.user.userpoints;
        var currentLevel = Data.user.userlevel;

        if (currentPoints + points < Data.level.points)
        {
            currentPoints = Data.user.userpoints + points;
        }
        else
        // Levelup
        {
            currentPoints = (Data.user.userpoints + points) % (Data.level.points);
            currentLevel = Data.user.userlevel + 1;
        }
        return (currentPoints, currentLevel);
    }

    public void AddPoints(int points, Action<int, int> callback)
    {
        StartCoroutine(AddPointsCoroutine(points, callback));
    }

    private IEnumerator AddPointsCoroutine(int points, Action<int, int> callback)
    {
        var values = GetPointsAndLevel(points);

        // Bereite das POST-Formular vor
        WWWForm updatePoints = new WWWForm();
        updatePoints.AddField("userId", Data.user.id);
        updatePoints.AddField("points", values.Item1);
        updatePoints.AddField("level", values.Item2);

        // Sende die Anfrage an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "updatepoints.php", updatePoints))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // Erfolgreich
                Data.user.userpoints = values.Item1;
                Data.user.userlevel = values.Item2;

                // Neue Level-Daten
                Debug.Log(www.downloadHandler.text);
                Data.level = JsonMapper.ToObject<Level>(www.downloadHandler.text);

                // Callback mit neuen Werten aufrufen (Punkte, Level)
                callback?.Invoke(values.Item1, values.Item2);
            }
            else
            {
                // Fehlerfall: Callback trotzdem aufrufen, als Fehlermeldung mit -1
                callback?.Invoke(-1, -1);
            }
        }
    }

    public void Updatestreak(Action<int, int> callback)
    {
        StartCoroutine(UpdatestreakCoroutine(callback));
    }

    private IEnumerator UpdatestreakCoroutine(Action<int, int> callback)
    {
        // streak + 1, oder 1 wenn gerissen
        var newstreak = Data.user.GetLastLogin().AddDays(1).Date.Equals(DateTime.UtcNow.ToLocalTime().Date) ? Data.user.streak + 1 : 1;
        var addPoints = newstreak % 8;

        var values = GetPointsAndLevel(addPoints);

        // Bereite das POST-Formular vor
        WWWForm updatestreak = new WWWForm();
        updatestreak.AddField("userId", Data.user.id);
        updatestreak.AddField("streak", newstreak);
        updatestreak.AddField("recordstreak", Math.Max(Data.user.recordstreak, newstreak));
        updatestreak.AddField("level", values.Item2);
        updatestreak.AddField("points", values.Item1);
        updatestreak.AddField("lastlogin", DateTime.UtcNow.ToLocalTime().ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"));

        // Sende die Anfrage an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "updatestreak.php", updatestreak))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(www.downloadHandler.text);
                // Das JSON kann direkt in das Level-Objekt geparst werden
                Data.level = JsonMapper.ToObject<Level>(www.downloadHandler.text);

                // Erfolgreich
                Data.user.userpoints = values.Item1;
                Data.user.userlevel = values.Item2;
                Data.user.lastlogin = DateTime.UtcNow.ToLocalTime().ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                Data.user.streak = newstreak;
                Data.user.recordstreak = Math.Max(Data.user.recordstreak, newstreak);

                // Callback mit neuen Werten
                callback?.Invoke(values.Item1, values.Item2);
            }
            else
            {
                // Fehlerfall: Callback trotzdem aufrufen, als Fehlermeldung mit -1
                Debug.LogError("Fehler beim Hinzufügen der Punkte: " + www.error);
                callback?.Invoke(-1, -1);
            }
        }
    }

    public void GetUser(int id, Action<User> callback)
    {
        StartCoroutine(GetUserCoroutine(id, callback));
    }

    private IEnumerator GetUserCoroutine(int id, Action<User> callback)
    {
        // LoginDaten übermitteln
        WWWForm getUser = new WWWForm();
        getUser.AddField("userId", id);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getuser.php", getUser))
        {
            yield return www.SendWebRequest();

            // Fehler bei der Serververbindung
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Abrufen fehlgeschlagen: Keine Verbindung zum Server!");
            }
            else
            {
                // Serverantwort als JSON parsen
                Debug.Log("Abruf erfolgreich!");
                Debug.Log(www.downloadHandler.text);

                // Versuche, die Antwort als JSON zu deserialisieren
                try
                {
                    var response = JsonMapper.ToObject<Dictionary<string, object>>(www.downloadHandler.text);

                    if (response.ContainsKey("status") && response["status"].ToString() == "success")
                    {
                        // Callback mit deserialisiertem User aufrufen
                        User user = JsonMapper.ToObject<User>(www.downloadHandler.text);
                        callback?.Invoke(user); // Callback mit dem User-Objekt aufrufen
                    }
                    else
                    {
                        // Fehler, wenn der Status nicht "success" ist
                        string errorMessage = response.ContainsKey("message") ? response["message"].ToString() : "Unbekannter Fehler";
                        Debug.LogError("Login fehlgeschlagen: " + errorMessage);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Fehler beim Parsen der Serverantwort: " + e.Message);
                }
            }
        }
    }

    public void GetUserDiary(int id, Action<Diary> callback)
    {
        StartCoroutine(GetUserDiaryCoroutine(id, callback));
    }

    private IEnumerator GetUserDiaryCoroutine(int id, Action<Diary> callback)
    {
        WWWForm getDiary = new WWWForm();
        getDiary.AddField("userId", id);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getdiary.php", getDiary))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Tagebuch abrufen fehlgeschlagen: Keine Verbindung zum Server!");
            }
            else
            {
                Debug.Log("Tagebuch abgerufen!");
                Debug.Log(www.downloadHandler.text);
                Diary diary = JsonMapper.ToObject<Diary>(www.downloadHandler.text);
                if (diary == null)
                {
                    diary.entries = new List<DiaryEntry>();
                }
                callback?.Invoke(diary);
            }
        }
    }

    public void GetPlayedScenarios(int id, Action<ScenariosPlayed> callback)
    {
        StartCoroutine(GetPlayedScenariosCoroutine(id, callback));
    }

    private IEnumerator GetPlayedScenariosCoroutine(int userId, Action<ScenariosPlayed> callback)
    {
        WWWForm getPlayedScenarios = new WWWForm();
        getPlayedScenarios.AddField("userId", userId);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getplayedscenarios.php", getPlayedScenarios))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Fehler beim Abrufen der Szenarien: " + www.error);
            }
            else
            {
                ScenariosPlayed scenariosPlayed = JsonMapper.ToObject<ScenariosPlayed>(www.downloadHandler.text);

                if (scenariosPlayed != null && scenariosPlayed.scenariosPlayed != null)
                {
                    callback?.Invoke(scenariosPlayed);
                }
                else
                {
                    callback?.Invoke(new ScenariosPlayed { scenariosPlayed = new List<ScenarioPlayed>() });
                }
            }
        }
    }

    public void GetLevelData(int level, Action<Level> callback)
    {
        StartCoroutine(GetLevelDataCoroutine(level, callback));
    }

    private IEnumerator GetLevelDataCoroutine(int level, Action<Level> callback)
    {
        WWWForm getLevel = new WWWForm();
        getLevel.AddField("userlevel", level);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getlevel.php", getLevel))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Levelabruf fehlgeschlagen!");
            }
            else
            {
                Debug.Log("Level abgerufen!");
                Debug.Log(www.downloadHandler.text);
                Level lvl = JsonMapper.ToObject<Level>(www.downloadHandler.text);
                callback?.Invoke(lvl);
            }
        }
    }

    public void GetAdvisedUsers(int id, Action<List<User>> callback)
    {
        StartCoroutine(GetAdvisedUsersCoroutine(id, callback));
    }

    private IEnumerator GetAdvisedUsersCoroutine(int id, Action<List<User>> callback)
    {
        WWWForm getAdvisedUsers = new WWWForm();
        getAdvisedUsers.AddField("id", id);

        // Sende die Anfrage an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getadvisedusers.php", getAdvisedUsers))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Abruf der User fehlgeschlagen! Fehler: " + www.error);
            }
            else
            {
                // Erfolgreiche Antwort erhalten
                Debug.Log("User abgerufen!");
                Debug.Log(www.downloadHandler.text); // Debug-Ausgabe der Antwort

                // Die Antwort direkt als Liste von Benutzern deserialisieren
                List<User> users = JsonMapper.ToObject<List<User>>(www.downloadHandler.text);

                // Den Callback mit der Liste der Benutzer (ggf leer) aufrufen
                callback?.Invoke(users ?? new List<User>());
            }
        }
    }


    public void GetScenarioPlan(int id, Action<ScenarioPlans> callback)
    {
        StartCoroutine(GetScenarioPlanCoroutine(id, callback));
    }

    private IEnumerator GetScenarioPlanCoroutine(int userId, Action<ScenarioPlans> callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", userId);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getscenarioplan.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Abruf des Szenarioplans fehlgeschlagen: " + www.error);
            }
            else
            {
                ScenarioPlans response = JsonMapper.ToObject<ScenarioPlans>(www.downloadHandler.text);

                if (response?.scenarioplan != null)
                {
                    callback?.Invoke(response);
                }
                else
                {
                    // Leere Liste zurückgeben, wenn keine Szenarien vorhanden
                    callback?.Invoke(new ScenarioPlans());
                }
            }
        }
    }

    public void GetScenarios(Action<ScenarioPlanMenu.ScenarioResponse> callback)
    {
        StartCoroutine(GetScenariosCoroutine(callback));
    }

    private IEnumerator GetScenariosCoroutine(Action<ScenarioPlanMenu.ScenarioResponse> callback)
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "getscenarios.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Abruf des Szenarioplans fehlgeschlagen: " + www.error);
            }
            else
            {
                ScenarioPlanMenu.ScenarioResponse response = JsonMapper.ToObject<ScenarioPlanMenu.ScenarioResponse>(www.downloadHandler.text);

                if (response?.scenarios != null)
                {
                    callback?.Invoke(response);
                }
                else
                {
                    Debug.LogError("Szenarien konnten nicht abgerufen werden: " + www.error);
                }
            }
        }
    }

    internal void DeleteScenarioPlan(int id, Action<bool> callback)
    {
        StartCoroutine(DeleteScenarioPlanCoroutine(id, callback));
    }

    private IEnumerator DeleteScenarioPlanCoroutine(int id, Action<bool> callback)
    {
        // Erstellen des WWWForm
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        // Senden der Anfrage an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "deletescenarioplan.php", form))
        {
            yield return www.SendWebRequest();

            // Überprüfen, ob die Anfrage erfolgreich war
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Löschen des Szenarioplans fehlgeschlagen: " + www.error);
                callback?.Invoke(false); // Fehler: Callback mit `false`
            }
            else
            {
                // Wenn die Anfrage erfolgreich war, den JSON-Response prüfen
                string jsonResponse = www.downloadHandler.text;

                // Überprüfe, ob die Antwort einen Erfolg enthält
                if (!string.IsNullOrEmpty(jsonResponse) && jsonResponse.Contains("\"status\":\"success\""))
                {
                    // Erfolgreich gelöscht, Callback mit `true`
                    callback?.Invoke(true);
                }
                else
                {
                    // Fehler bei der Server-Antwort (z.B. Status "error")
                    Debug.LogError("Fehler bei der Löschung des Szenarioplans: " + jsonResponse);
                    callback?.Invoke(false); // Fehler: Callback mit `false`
                }
            }
        }
    }

    public void AddScenarioPlan(int? scenario, int? category, int orderNumber, Action<bool> success)
    {
        StartCoroutine(AddScenarioPlanCoroutine(scenario, category, orderNumber, success));
    }

    private IEnumerator AddScenarioPlanCoroutine(int? scenario, int? category, int orderNumber, Action<bool> callback)
    {
        // Erstellen des WWWForm
        WWWForm form = new WWWForm();
        form.AddField("userId", Data.advisedPerson.user.id);
        form.AddField("scenarioId", scenario.ToString());
        form.AddField("categoryId", category.ToString());
        form.AddField("orderNumber", orderNumber.ToString());

        // Senden der Anfrage an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "addscenarioplan.php", form))
        {
            yield return www.SendWebRequest();

            // Überprüfen, ob die Anfrage erfolgreich war
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Hinzufügen des Szenarioplans fehlgeschlagen: " + www.error);
                callback?.Invoke(false); // Fehler: Callback mit `false`
            }
            else
            {
                // Wenn die Anfrage erfolgreich war, den JSON-Response prüfen
                string jsonResponse = www.downloadHandler.text;

                // Überprüfe, ob die Antwort einen Erfolg enthält
                if (!string.IsNullOrEmpty(jsonResponse) && jsonResponse.Contains("\"status\":\"success\""))
                {
                    // Erfolgreich gelöscht, Callback mit `true`
                    callback?.Invoke(true);
                }
                else
                {
                    // Fehler bei der Server-Antwort (z.B. Status "error")
                    Debug.LogError("Fehler beim hinzufügen des Szenarioplans: " + jsonResponse);
                    callback?.Invoke(false); // Fehler: Callback mit `false`
                }
            }
        }
    }

    public void SetAdvisor(int id, int advisorId, Action<bool> success)
    {
        StartCoroutine(SetAdvisorCoroutine(id, advisorId, success));
    }

    private IEnumerator SetAdvisorCoroutine(int id, int advisorId, Action<bool> callback)
    {
        // Erstellen des WWWForm
        WWWForm form = new WWWForm();
        form.AddField("userId", id.ToString());
        form.AddField("advisorId", advisorId.ToString());

        // Senden der Anfrage an den Server
        using (UnityWebRequest www = UnityWebRequest.Post(Server.ip + "addadvisor.php", form))
        {
            yield return www.SendWebRequest();

            // Überprüfen, ob die Anfrage erfolgreich war
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Hinzufügen des Betreuers fehlgeschlagen: " + www.error);
                callback?.Invoke(false); // Fehler: Callback mit `false`
            }
            else
            {
                // Wenn die Anfrage erfolgreich war, den JSON-Response prüfen
                string jsonResponse = www.downloadHandler.text;

                // Überprüfe, ob die Antwort einen Erfolg enthält
                if (!string.IsNullOrEmpty(jsonResponse) && jsonResponse.Contains("\"status\":\"success\""))
                {
                    // Erfolgreich gelöscht, Callback mit `true`
                    callback?.Invoke(true);
                }
                else
                {
                    // Fehler bei der Server-Antwort (z.B. Status "error")
                    Debug.LogError("Fehler beim hinzufügen des Betreuers: " + jsonResponse);
                    callback?.Invoke(false);
                }
            }
        }
    }

    public void GetScenario(int userId, Action<Scenario> scenario)
    {
        StartCoroutine(GetScenarioCoroutine(userId, scenario));
    }

    private IEnumerator GetScenarioCoroutine(int userId, Action<Scenario> callback)
    {

        WWWForm getScenario = new WWWForm();
        getScenario.AddField("userId", userId);

        // Senden der Anfrage an den Server
        using var www = UnityWebRequest.Post(Server.ip + "getscenario.php", getScenario);
        yield return www.SendWebRequest();

        // Überprüfen, ob die Anfrage erfolgreich war
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            callback?.Invoke(new Scenario());
        }
        else
        {
            Debug.Log("Szenario abgerufen!");
            Scenario scenario = JsonMapper.ToObject<ScenarioResponse>(www.downloadHandler.text).scenario;
            callback?.Invoke(scenario);
        }
    }

    public void SetScenario(Scenario scenario, Answer answer, Action<bool> success)
    {
        StartCoroutine(SetScenarioCoroutine(scenario, answer, success));
    }

    private IEnumerator SetScenarioCoroutine(Scenario scenario, Answer answer, Action<bool> callback)
    {

        // Bereite die zu sendenden Daten vor
        WWWForm completeScenario = new WWWForm();
        completeScenario.AddField("userId", Data.user.id.ToString());
        completeScenario.AddField("scenarioId", scenario.scenarioId.ToString());
        completeScenario.AddField("answerId", answer.answerId.ToString());
        completeScenario.AddField("answercategory", answer.answercategory.ToString());
        completeScenario.AddField("categoryId", scenario.categoryId.ToString());

        // Senden der Anfrage an den Server
        using var www = UnityWebRequest.Post(Server.ip + "setscenario.php", completeScenario);
        yield return www.SendWebRequest();

        // Überprüfen, ob die Anfrage erfolgreich war
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Fehler beim Senden der Anfrage: " + www.error);
            callback.Invoke(false);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            try
            {
                ScenariosPlayed scenarioPlayed = JsonMapper.ToObject<ScenariosPlayed>(www.downloadHandler.text);
                Debug.Log(scenarioPlayed.scenariosPlayed.First().date);
                Data.scenariosPlayed.scenariosPlayed.Add(scenarioPlayed.scenariosPlayed.First());
                callback.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }

    public void GetCategories(Action<AddScenario.Categories> callback)
    {
        StartCoroutine(GetCategoriesCoroutine(callback));
    }

    private IEnumerator GetCategoriesCoroutine(Action<AddScenario.Categories> callback)
    {
        WWWForm getCategories = new WWWForm();

        using var www = UnityWebRequest.Post(Server.ip + "getcategories.php", getCategories);
        yield return www.SendWebRequest();

        {
            Debug.Log(www.downloadHandler.text);
            try
            {
                CategoryResponse resp = JsonMapper.ToObject<CategoryResponse>(www.downloadHandler.text);

                if (resp.status == "success")
                {
                    // Erfolgreiche Antwort
                    Debug.Log("Anzahl der erCategories: " + resp.categories.erCategories.Count);
                    callback.Invoke(resp.categories);
                }
                else
                {
                    // Fehlerbehandlung
                    Debug.LogError("Fehler: " + resp.message);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Fehler bei der Deserialisierung: " + ex.Message);
            }
        }
    }

    public void SendScenario(Scenario scenario, Action<bool> success)
    {
        StartCoroutine(SendScenarioCoroutine(scenario, success));
    }

    private IEnumerator SendScenarioCoroutine(Scenario scenario, Action<bool> success)
    {
        // Bereite die zu sendenden Daten vor
        WWWForm completeScenario = new WWWForm();
        completeScenario.AddField("userId", Data.user.id);
        completeScenario.AddField("name", scenario.Name);
        completeScenario.AddField("question", scenario.question);
        completeScenario.AddField("categoryId", scenario.categoryId);
        completeScenario.AddField("a1", scenario.answers[0].answer);
        completeScenario.AddField("a2", scenario.answers[1].answer);
        completeScenario.AddField("a3", scenario.answers[2].answer);
        completeScenario.AddField("a4", scenario.answers[3].answer);
        completeScenario.AddField("a5", scenario.answers[4].answer);
        completeScenario.AddField("a6", scenario.answers[5].answer);

        completeScenario.AddField("r1", scenario.answers[0].response);
        completeScenario.AddField("r2", scenario.answers[1].response);
        completeScenario.AddField("r3", scenario.answers[2].response);
        completeScenario.AddField("r4", scenario.answers[3].response);
        completeScenario.AddField("r5", scenario.answers[4].response);
        completeScenario.AddField("r6", scenario.answers[5].response);

        completeScenario.AddField("c1", scenario.answers[0].answercategory);
        completeScenario.AddField("c2", scenario.answers[1].answercategory);
        completeScenario.AddField("c3", scenario.answers[2].answercategory);
        completeScenario.AddField("c4", scenario.answers[3].answercategory);
        completeScenario.AddField("c5", scenario.answers[4].answercategory);
        completeScenario.AddField("c6", scenario.answers[5].answercategory);
        Debug.Log("Gesendete Daten:");
        Debug.Log("userId: " + Data.user.id);
        Debug.Log("name: " + scenario.Name);
        Debug.Log("question: " + scenario.question);
        Debug.Log("categoryId: " + scenario.categoryId);

        Debug.Log("a1: " + scenario.answers[0].answer);
        Debug.Log("a2: " + scenario.answers[1].answer);
        Debug.Log("a3: " + scenario.answers[2].answer);
        Debug.Log("a4: " + scenario.answers[3].answer);
        Debug.Log("a5: " + scenario.answers[4].answer);
        Debug.Log("a6: " + scenario.answers[5].answer);

        Debug.Log("r1: " + scenario.answers[0].response);
        Debug.Log("r2: " + scenario.answers[1].response);
        Debug.Log("r3: " + scenario.answers[2].response);
        Debug.Log("r4: " + scenario.answers[3].response);
        Debug.Log("r5: " + scenario.answers[4].response);
        Debug.Log("r6: " + scenario.answers[5].response);

        Debug.Log("c1: " + scenario.answers[0].answercategory);
        Debug.Log("c2: " + scenario.answers[1].answercategory);
        Debug.Log("c3: " + scenario.answers[2].answercategory);
        Debug.Log("c4: " + scenario.answers[3].answercategory);
        Debug.Log("c5: " + scenario.answers[4].answercategory);
        Debug.Log("c6: " + scenario.answers[5].answercategory);

        // Senden der Anfrage an den Server
        using var www = UnityWebRequest.Post(Server.ip + "addscenario.php", completeScenario);
        yield return www.SendWebRequest();

        // Überprüfen, ob die Anfrage erfolgreich war
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Fehler beim Senden der Anfrage: " + www.error);
            success.Invoke(false);
        }
        else
        {
            success.Invoke(true);
            Debug.Log("Scenario erfolgreich eingetragen");
        }
    }
}