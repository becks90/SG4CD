using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Dates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AdvisedPerson;

public class AdvisedPersonData : MonoBehaviour
{
    private List<UserEntry> entries = new List<UserEntry>();
    private List<UserEntry> filteredEntries = new List<UserEntry>();
    private List<string> erCategories;
    private List<string> answerCategories;

    public GameObject ctnFilter;
    public DatePicker dpStart;  
    public DatePicker dpEnd;    
    public Toggle chkDiary;     
    public Toggle chkScenario;  
    public Toggle chkGood, chkNeutral, chkBad;
    public Toggle togglePrefab;       

    public GameObject ctnEntryPrefab;
    public Transform parentTransform;

    public GameObject pnlScenario, pnlDiary;
    public TextMeshProUGUI txtDate, txtScenario, txtCategory, txtAnswer, txtAnswerCategory;
    public TextMeshProUGUI txtDiaryDate, txtDiaryEntry;

    void Start()
    {
        FillEntries(); // Fülle entries mit Daten

        // Button und Image in Header deaktivieren
        ctnEntryPrefab.transform.Find("imgFeeling").GetComponent<Image>().gameObject.SetActive(false);
        ctnEntryPrefab.transform.Find("btnEntry").GetComponent<Button>().gameObject.SetActive(false);

        // Initialisiere Liste mit den Einträgen des aktiven Person
        filteredEntries = new List<UserEntry>(entries);

        // Hole benötigte/vorhandene Kategorie-Namen
        erCategories = Data.advisedPerson.scenariosPlayed.scenariosPlayed.Select(s => s.category_name).Distinct().ToList();
        answerCategories = Data.advisedPerson.scenariosPlayed.scenariosPlayed.Select(s => s.answercategory_name).Distinct().ToList();

        // Erstelle Toggles
        GenerateCategoryToggles();
        GenerateAnswerCategoryToggles();

        // Reagiert auf Änderungen am Szenario-Toggle
        chkScenario.onValueChanged.AddListener((isOn) =>
        {
            SetCategoryTogglesInteractivity(isOn);  
            SetAnswerCategoryTogglesInteractivity(isOn);
        });
    }

    public void ApplyFilters()
    {
        FillEntries();
        Debug.Log($"Anzahl der Einträge vor Filterung: {filteredEntries.Count}");

        DateTime? startDate = dpStart.SelectedDate.HasValue ? dpStart.SelectedDate : (DateTime?)null;
        DateTime? endDate = dpEnd.SelectedDate.HasValue ? dpEnd.SelectedDate : (DateTime?)null;

        // Fülle die Einträge mit den geladenen Daten
        var tempFilteredEntries = new List<UserEntry>();

        // Filtere nach "Tagebuch" (nur Tagebuch-Einträge, wenn chkDiary aktiviert ist)
        if (chkDiary.isOn)
        {
            // Wenn Tagebuch aktiviert ist, füge diese hinzu
            tempFilteredEntries.AddRange(entries.Where(entry => entry.type == "Tagebuch"));
        }

        // Filtere nach "Szenario" (nur Szenario-Einträge, wenn chkScenario aktiviert ist)
        if (chkScenario.isOn)
        {
            var scenarioEntries = entries.Where(entry => entry.type == "Szenario").ToList();

            // Filtere nach den erCategory- und answerCategory-Toggles
            List<string> selectedCategories = new();
            List<string> selectedAnswerCategories = new();

            // Aktiviert Filter für aktive Toggles
            foreach (Toggle toggle in ctnFilter.GetComponentsInChildren<Toggle>())
            {
                if (toggle.isOn && toggle.GetComponentInChildren<Text>().text != null)
                {
                    string toggleText = toggle.GetComponentInChildren<Text>().text.Trim();

                    if (erCategories.Contains(toggleText))
                    {
                        selectedCategories.Add(toggleText);
                    }
                    else if (answerCategories.Contains(toggleText))
                    {
                        selectedAnswerCategories.Add(toggleText);
                    }
                }
            }

            Debug.Log($"Aktive Kategorien: {string.Join(", ", selectedCategories)}");
            Debug.Log($"Aktive Antwortkategorien: {string.Join(", ", selectedAnswerCategories)}");

            // Wenn Szenario aktiv ist, filtere nach den ER-Kategorien und Antwortkategorien
            scenarioEntries = scenarioEntries.Where(entry =>
            {
                bool erCategoryMatch = selectedCategories.Contains(entry.erCategory?.Trim());
                bool answerCategoryMatch = selectedAnswerCategories.Contains(entry.answerCategory?.Trim());

                Debug.Log($"Überprüfe Szenario: entry.erCategory = '{entry.erCategory}', entry.answerCategory = '{entry.answerCategory}' | erCategory-Filter: {erCategoryMatch}, answerCategory-Filter: {answerCategoryMatch}, Match: {erCategoryMatch && answerCategoryMatch}");

                return erCategoryMatch && answerCategoryMatch;
            }).ToList();

            Debug.Log($"Gefilterte Szenario-Einträge: {scenarioEntries.Count}");

            // Füge diese hinzu
            tempFilteredEntries.AddRange(scenarioEntries);
        }

        // Filterung nach Gut, Schlecht und Neutral
        tempFilteredEntries = tempFilteredEntries.Where(entry =>
        {
            bool matchesGood = chkGood.isOn && (entry.feeling > 0 && entry.feeling < 3 || entry.answerCategory == "Akzeptanz" ||
                                                entry.answerCategory == "Problemlösung" ||
                                                entry.answerCategory == "Kognitive Umbewertung");

            bool matchesNeutral = chkNeutral.isOn && (entry.feeling == 3);

            bool matchesBad = chkBad.isOn && (entry.feeling > 3 || entry.answerCategory == "Vermeidung" ||
                                              entry.answerCategory == "Rumination" ||
                                              entry.answerCategory == "Unterdrückung des Gefühlsausdrucks");

            // bleibt in der Liste, wenn mindestens ein aktiver Filter entspricht
            return (matchesGood || matchesNeutral || matchesBad);
        }).ToList();


        // Filtere nach Zeitraum (falls angegeben)
        if (startDate.HasValue)
        {
            tempFilteredEntries = tempFilteredEntries.Where(entry => entry.entrydate >= startDate.Value).ToList();
        }
        if (endDate.HasValue)
        {
            tempFilteredEntries = tempFilteredEntries.Where(entry => entry.entrydate <= endDate.Value).ToList();
        }

        // Debug: Anzahl der gefilterten Einträge nach Anwendung der Filter
        Debug.Log($"Anzahl der Einträge nach Filterung: {tempFilteredEntries.Count}");

        // Zeige die gefilterten Ergebnisse
        if (tempFilteredEntries.Count == 0)
        {
            Debug.Log("Keine Einträge gefunden, die den Filterkriterien entsprechen.");
        }
        else
        {
            tempFilteredEntries = tempFilteredEntries.OrderBy(e => e.entrydate).ToList();
            ShowEntries(tempFilteredEntries);
            Debug.Log($"Anzahl der gefilterten Einträge: {tempFilteredEntries.Count}");
        }
    }

    // (De)Aktiviert die Toggles

    public void SetCategoryTogglesInteractivity(bool active)
    {
        foreach (Toggle toggle in ctnFilter.GetComponentsInChildren<Toggle>())
        {
            if (erCategories.Contains(toggle.GetComponentInChildren<Text>().text))
            {
                toggle.interactable = active;
            }
        }
    }

    // (De)Aktiviert die Toggles
    public void SetAnswerCategoryTogglesInteractivity(bool active)
    {
        foreach (Toggle toggle in ctnFilter.GetComponentsInChildren<Toggle>())
        {
            if (answerCategories.Contains(toggle.GetComponentInChildren<Text>().text))
            {
                toggle.interactable = active;
            }
        }
    }

    // Toggles für die ER-Kategorien
    void GenerateCategoryToggles()
    {
        foreach (var category in erCategories)
        {
            Toggle newToggle = Instantiate(togglePrefab, ctnFilter.transform);
            newToggle.GetComponentInChildren<Text>().text = category;  // Setze den Text des Toggles auf den Namen der Kategorie
            newToggle.isOn = false;  // Optional: Standardmäßig deaktiviert
        }
    }

    // Toggles für die Antwortkategorien
    void GenerateAnswerCategoryToggles()
    {
        foreach (var answerCategory in answerCategories)
        {
            Toggle newToggle = Instantiate(togglePrefab, ctnFilter.transform);
            newToggle.GetComponentInChildren<Text>().text = answerCategory;  // Setze den Text des Toggles auf den Namen der Antwortkategorie
            newToggle.isOn = false;  // Optional: Standardmäßig deaktiviert
        }
    }

    private void FillEntries()
    {
        entries = new List<UserEntry>();

        // Füge Tagebucheinträge hinzu
        entries.AddRange(Data.advisedPerson.diary.entries.Select(diaryentry =>
            new UserEntry(diaryentry.id, diaryentry.entrydate, diaryentry.entry, diaryentry.feeling)));

        // Füge Szenarioeinträge hinzu
        entries.AddRange(Data.advisedPerson.scenariosPlayed.scenariosPlayed.Select(scenarios =>
            new UserEntry(scenarios.id, scenarios.date, scenarios.name, scenarios.category_name, scenarios.answercategory_name, scenarios.answer)));

        // Debug: Überprüfen, ob Einträge nach dem Laden korrekt sind
        Debug.Log($"Anzahl der geladenen Einträge: {entries.Count}");
    }

    public void ShowEntries(List<UserEntry> selectedEntries)
    {
        parentTransform.gameObject.SetActive(true); // Sicherstellen, dass der Eltern-Container aktiv ist

        // Zerstöre alle alten Instanzen im Content-Container, außer dem Prefab
        foreach (Transform child in parentTransform)
        {
            if (child.gameObject != ctnEntryPrefab)
            {
                Destroy(child.gameObject);
            }
        }

        // Image und Button des Prefab zur generierung aktivieren
        ctnEntryPrefab.transform.Find("imgFeeling").GetComponent<Image>().gameObject.SetActive(true);
        ctnEntryPrefab.transform.Find("btnEntry").GetComponent<Button>().gameObject.SetActive(true);

        // Für jedes gefilterte Element eine Zeile erstellen
        foreach (UserEntry entry in selectedEntries)
        {
            // Instanziiere das Prefab
            GameObject newEntry = Instantiate(ctnEntryPrefab, parentTransform);
            Debug.Log("Instanziiere neues Entry: " + newEntry.name);
            newEntry.SetActive(true);

            // Komponenten holen
            TMP_Text dateText = newEntry.transform.Find("txtDate").GetComponent<TMP_Text>();
            TMP_Text typeText = newEntry.transform.Find("txtType").GetComponent<TMP_Text>();
            TMP_Text entryText = newEntry.transform.Find("txtEntry").GetComponent<TMP_Text>();
            Image feelingImage = newEntry.transform.Find("imgFeeling").GetComponent<Image>();
            Button button = newEntry.transform.Find("btnEntry").GetComponent<Button>();

            // Setze UI-Werte
            dateText.gameObject.SetActive(true);
            dateText.text = entry.entrydate.ToString("dd.MM.yyyy");
            typeText.gameObject.SetActive(true);
            typeText.text = entry.type;
            entryText.gameObject.SetActive(true);
            entryText.text = entry.entry;
            feelingImage.gameObject.SetActive(true);
            feelingImage.sprite = GetFeelingSprite(entry.feeling, entry.type, entry.answerCategory);

            // Button Listener
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => OnButtonClick(entry));
        }
        // Button und Image in Header-Prefab deaktivieren
        ctnEntryPrefab.transform.Find("imgFeeling").GetComponent<Image>().gameObject.SetActive(false);
        ctnEntryPrefab.transform.Find("btnEntry").GetComponent<Button>().gameObject.SetActive(false);
    }

    // Gibt den entsprechenden Sprite (Bild, Emoticon) zurück
    Sprite GetFeelingSprite(int feeling, string type, string answer)
    {
        if (string.Equals("Tagebuch", type))
        {
            switch (feeling)
            {
                case 1: return Resources.Load<Sprite>("Feeling_1");
                case 2: return Resources.Load<Sprite>("Feeling_2");
                case 3: return Resources.Load<Sprite>("Feeling_3");
                case 4: return Resources.Load<Sprite>("Feeling_4");
                case 5: return Resources.Load<Sprite>("Feeling_5");
                default: return null;
            }
        }
        else
        {
            switch (type)
            {
                case "Akzeptanz" :
                case "Problemlösung":
                case "Kognitive Umbewertung":
                    return Resources.Load<Sprite>("Feeling_1");
                default: 
                    return Resources.Load<Sprite>("Feeling_5");
            }
        }
    }

    void OnButtonClick(UserEntry entry)
    {
        if (string.Equals(entry.type, "Szenario"))
        {
            pnlScenario.SetActive(true);
            txtDate.SetText(entry.entrydate.ToString());
            txtScenario.SetText(entry.entry);
            txtCategory.SetText(entry.erCategory);
            txtAnswer.SetText(entry.answer);
            txtAnswerCategory.SetText(entry.answerCategory);
        }
        else {
            pnlDiary.SetActive(true);
            txtDiaryDate.SetText(entry.entrydate.ToString());
            txtDiaryEntry.SetText(entry.entry);
        }
    }

    public void ClosePanel()
    {
        pnlScenario.SetActive(false);
        pnlDiary.SetActive(false);
    }

    public void GoBack(int i)
    {
        SceneManager.LoadScene(i);
    }
}