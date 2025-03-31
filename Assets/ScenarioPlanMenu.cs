using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioPlanMenu : MonoBehaviour
{
    public TMP_Dropdown ddScenarios, ddErCategories;
    public ScenarioResponse scenarios;

    public Transform parentTransform;
    public GameObject ctnPrefab;
    public Button btnAddScenarioPlan;
    public Toggle chkScenario, chkCategory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ctnPrefab.transform.Find("btnDelete").GetComponent<Button>().gameObject.SetActive(false);
        FillDropDowns();
    }

    // Erzeugt Klone des Prefabs für jeden SzenarioPlan des advised Person
    public void ShowEntries()
    {
        parentTransform.gameObject.SetActive(true); // Sicherstellen, dass der Eltern-Container aktiv ist
        ctnPrefab.transform.Find("btnDelete").GetComponent<Button>().gameObject.SetActive(true);

        // Zerstöre alle alten Instanzen im Content-Container, außer dem Prefab
        foreach (Transform child in parentTransform)
        {
            if (child.gameObject != ctnPrefab)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (ScenarioPlan scenario in Data.advisedPerson.scenariosPlan.scenarioplan)
        {
            // Instanziiere das Prefab
            GameObject newEntry = Instantiate(ctnPrefab, parentTransform);
            Debug.Log("Instanziiere neues Entry: " + newEntry.name);
            newEntry.SetActive(true);

            // Komponenten holen
            TMP_Text orderText = newEntry.transform.Find("txtOrder").GetComponent<TMP_Text>();
            TMP_Text scenarioText = newEntry.transform.Find("txtScenario").GetComponent<TMP_Text>();
            Button button = newEntry.transform.Find("btnDelete").GetComponent<Button>();

            // Setze UI-Werte
            orderText.gameObject.SetActive(true);
            orderText.text = scenario.questionOrder.ToString();
            scenarioText.gameObject.SetActive(true);

            // Nächstes Szenario (höhere Prio) oder Kategorie falls null
            if (scenario.nextScenarioId != null)
            {
                scenarioText.text = $"Szenario: {GetScenarioName((int)scenario.nextScenarioId)}";
            }
            else if (scenario.nextCategoryId != null)
            {
                scenarioText.text = $"Kategorie: {GetCategoryName((int)scenario.nextCategoryId)}";
            }

            // Click Listener
            button.gameObject.SetActive(true);
            button.onClick.AddListener(() => OnButtonClick(scenario));
        }
        ctnPrefab.transform.Find("btnDelete").GetComponent<Button>().gameObject.SetActive(false);
    }

    private void OnButtonClick(ScenarioPlan scenario)
    {
        // Löscht den SzenarioPlanEintrag
        DataManager.Instance.DeleteScenarioPlan(scenario.id, success =>
        {
            if (success)
            {
                Data.advisedPerson.scenariosPlan.scenarioplan.Remove(scenario);
                ShowEntries();
            }
            else
            {
                Debug.LogError("Scenarioplan konnte nicht gelöscht werden");
            }
        });
    }

    private void FillDropDowns()
    {
        // Läd die Szenarien
        DataManager.Instance.GetScenarios(scenarios =>
        {
            if (scenarios != null)
            {
                this.scenarios = scenarios;
                // Füllt die DropDowns
                ddScenarios.ClearOptions(); 
                ddScenarios.AddOptions(this.scenarios.scenarios.Select(s => s.name).ToList());
                ddErCategories.ClearOptions();
                ddErCategories.AddOptions(this.scenarios.scenarios.Select(s => s.categoryName).Distinct().ToList());
                ShowEntries();
            }
            else
            {
                Debug.LogError("Keine Szenarien gefunden");
            }
        });
    }

    // Fügt einen Plan mit dem ausgewähltem Szenario-Id oder Kategorie-ID hinzu
    public void AddScenarioPlan()
    {
        int? scenario = chkScenario.isOn ? GetScenarioId(ddScenarios.options[ddScenarios.value].text) : null;
        int? category = chkCategory.isOn ? GetCategoryId(ddErCategories.options[ddErCategories.value].text) : null;
        int orderNumber = 1;
        if (Data.advisedPerson.scenariosPlan.scenarioplan.Any())
        {
            orderNumber = Data.advisedPerson.scenariosPlan.scenarioplan.Max(sp => sp.questionOrder) + 1;
        }
        
        DataManager.Instance.AddScenarioPlan(scenario, category, orderNumber, success =>
        {
            if (success)
            {
                // ScenarioPlan neu abrufen
                DataManager.Instance.GetScenarioPlan(Data.advisedPerson.user.id, scenarioPlansData =>
                    {
                        if (scenarioPlansData != null && scenarioPlansData.scenarioplan != null && scenarioPlansData.scenarioplan.Count > 0)
                        {
                            Debug.Log($"Es wurden {scenarioPlansData.scenarioplan.Count} geplantes Szenarien abgerufen.");
                            Data.advisedPerson.scenariosPlan = scenarioPlansData; // Szenarioplan zuweisen

                            // Und anzeigen
                            ShowEntries();
                        }
                        else
                        {
                            Debug.LogError("Keine Szenariopläne gefunden");
                        }
                    });
            }
            else
            {
                Debug.LogError("Szenarioplan konnte nicht hinzugefügt werden!");
            }
        }
        );
    }

    public class Scenario
    {
        public int id { get; set; }
        public string name { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
    }

    public class ScenarioResponse
    {
        public string status { get; set; }
        public List<Scenario> scenarios { get; set; }
    }

    // Gibt die Id für ein Szenarionamen zurück
    private int GetScenarioId(string name)
    {
        return scenarios.scenarios.FirstOrDefault(s => s.name == name).id;
    }

    // Gibt die ID für einen Kategorienamen zurück
    private int GetCategoryId(string name)
    {
        return scenarios.scenarios.FirstOrDefault(s => s.categoryName == name).categoryId;
    }

    // Gibt den Namen eines Szenarios für die Id zurück
    private string GetScenarioName(int id)
    {
        return scenarios.scenarios.FirstOrDefault(s => s.id == id).name;
    }

    // Gibt den Names einer Kategory für die Id zurück
    private string GetCategoryName(int id)
    {
        return scenarios.scenarios.FirstOrDefault(s => s.categoryId == id).categoryName;
    }

    // De(aktiviert) den jeweiligen anderen Toggle
    public void chkScenarioToggle()
    {
        chkCategory.isOn = !chkScenario.isOn;
    }

    // De(aktiviert) den jeweiligen anderen Toggle
    public void chkCategoryToggle()
    {
        chkScenario.isOn = !chkCategory.isOn;
    }

    public void GoBack(int i)
    {
        SceneManager.LoadScene(i);
    }
}