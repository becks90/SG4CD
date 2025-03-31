using System.Collections.Generic;
using System;
using UnityEngine;

public class AdvisedPerson
{
    public ScenariosPlayed scenariosPlayed { get; private set; }
    public ScenarioPlans scenariosPlan { get; set; }
    public Diary diary { get; private set; }
    public User user { get; private set; }

    private int dataLoadedCount = 0;  // Zähler zum prüfen ob alle Daten geladen wurden

    private Action<AdvisedPerson> callback;

    public class UserEntry
    {
        public int id;
        public DateTime entrydate;
        public string type;
        public string entry;
        public int feeling;
        public string erCategory;
        public string answerCategory;
        public string answer;

        public UserEntry(int id, string entrydate, string entry, int feeling)
        {
            this.id = id;
            this.entrydate = DateTime.Parse(entrydate);
            this.type = "Tagebuch";
            this.entry = entry;
            this.feeling = feeling;
        }

        public UserEntry(int id, string entrydate, string name, string category, string answercategory, string answer)
        {
            this.id = id;
            this.entrydate = DateTime.Parse(entrydate);
            this.type = "Szenario";
            this.entry = name;
            this.erCategory = category;
            this.answerCategory = answercategory;
            this.answer = answer;
        }
    }

    // Konstruktor
    public AdvisedPerson(User user, Action<AdvisedPerson> callback)
    {
        this.user = user;
        this.callback = callback;

        // Initialisiere Szenarioplan und Szenarien mit leeren Listen, falls keine Daten vorhanden sind
        this.scenariosPlan = new ScenarioPlans();
        this.scenariosPlan.scenarioplan = new List<ScenarioPlan>();

        this.scenariosPlayed = new ScenariosPlayed();
        this.scenariosPlayed.scenariosPlayed = new List<ScenarioPlayed>();

        this.diary = new Diary();
        this.diary.entries = new List<DiaryEntry>();

        // Läd das Tagebuch des Benutzers
        DataManager.Instance.GetUserDiary(user.id, diaryData =>
        {
            if (diaryData != null)
            {
                this.diary = diaryData; // Tagebuchdaten zuweisen
            }
            else
            {
                Debug.LogError("Keine Einträge gefunden");
                this.diary = new Diary { entries = new List<DiaryEntry>() }; // Leere Liste zuweisen
            }

            // Überprüfe, ob alle Daten geladen wurden
            CheckIfLoaded();
        });

        // Läd die Szenarien, die vom Benutzer gespielt wurden
        DataManager.Instance.GetPlayedScenarios(user.id, scenariosData =>
        {
            if (scenariosData != null && scenariosData.scenariosPlayed != null && scenariosData.scenariosPlayed.Count > 0)
            {
                Debug.Log($"Es wurden {scenariosData.scenariosPlayed.Count} gespielte Szenarien abgerufen.");
                this.scenariosPlayed = scenariosData; // Szenarien zuweisen
            }
            else
            {
                Debug.LogError("Keine absolvierten Szenarien gefunden");
                this.scenariosPlayed = new ScenariosPlayed { scenariosPlayed = new List<ScenarioPlayed>() }; // Leere Liste zuweisen
            }

            // Überprüfe, ob alle Daten geladen wurden
            CheckIfLoaded();
        });

        // Läd den Szenarioplan des Benutzers
        DataManager.Instance.GetScenarioPlan(user.id, scenarioPlansData =>
        {
            if (scenarioPlansData != null && scenarioPlansData.scenarioplan != null && scenarioPlansData.scenarioplan.Count > 0)
            {
                Debug.Log($"Es wurden {scenarioPlansData.scenarioplan.Count} geplantes Szenarien abgerufen.");
                this.scenariosPlan = scenarioPlansData; // Szenarioplan zuweisen
            }
            else
            {
                Debug.Log("Keine Szenariopläne gefunden");
                this.scenariosPlan = new ScenarioPlans { scenarioplan = new List<ScenarioPlan>() }; // Leere Liste zuweisen
            }

            // Überprüfe, ob alle Daten geladen wurden
            CheckIfLoaded();
        });
    }

    // Überprüft, ob alle Daten geladen wurden
    private void CheckIfLoaded()
    {
        // Erhöhe den Zähler
        dataLoadedCount++;

        // Wenn alle Daten geladen sind (3 in diesem Fall), rufe den Callback auf
        if (dataLoadedCount >= 3)
        {
            callback?.Invoke(this); // Callback mit dem geladenen AdvisedPerson-Objekt aufrufen
        }
    }
}