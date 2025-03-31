using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddScenario : MonoBehaviour
{
    private Categories categories;
    private List<Answer> answerList;
    private int index = 0;
    public TMP_InputField inpAnswer, inpResponse, inpScenarioName, inpScenarioQuestion;
    public TextMeshProUGUI lblAnswerCategory;
    public TMP_Dropdown ddErCategory;
    public Button btnNext, btnBack;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DataManager.Instance.GetCategories(callback =>
        {
            this.categories = callback;
            ddErCategory.ClearOptions();
            ddErCategory.AddOptions(callback.erCategories.Select(e => e.name).ToList());
            lblAnswerCategory.text = $"Beschreibe Antwort & Reaktion für die Kategorie: {categories.answerCategories[0].name}";
            answerList = new List<Answer>();
        });
    }
    public class CategoryResponse
    {
        public string status { get; set; }
        public Categories categories { get; set; }
        public string message { get; set; }
    }
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Categories
    {
        public List<Category> erCategories { get; set; }
        public List<Category> answerCategories { get; set; }
    }

    public void SaveAnswer(int i) {
        btnNext.interactable = false;
        Answer answer = new Answer();
        answer.answercategory = categories.answerCategories[index].id;
        answer.answer = inpAnswer.text;
        answer.response = inpResponse.text;
        answerList.Add(answer);
        index++;
        if (index < categories.answerCategories.Count)
        {
            inpAnswer.text = string.Empty;
            inpResponse.text = string.Empty;
            lblAnswerCategory.text = categories.answerCategories[index].name;
        }
        else
        {
            Scenario scenario = new Scenario();
            scenario.answers = answerList;
            scenario.Name = inpScenarioName.text;
            scenario.question = inpScenarioQuestion.text;
            scenario.categoryId = categories.erCategories.FirstOrDefault(c => c.name.Equals(ddErCategory.options[ddErCategory.value].text)).id;

            DataManager.Instance.SendScenario(scenario, success =>
            {
                if (success)
                {
                    lblAnswerCategory.text = "Erfolgreich hinzugefügt!";
                    //btnNext.interactable = false;
                    inpAnswer.interactable = false;
                    inpResponse.interactable = false;
                }
                else
                {
                    lblAnswerCategory.text = "Fehler beim hinzugefügen, neu starten!";
                    //btnNext.interactable = false;
                    inpAnswer.interactable = false;
                    inpResponse.interactable = false;
                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckInputs()
    {
        btnNext.interactable = inpAnswer.text.Length > 16 && inpResponse.text.Length > 16;
    }

    public void GoBack(int i)
    {
        SceneManager.LoadScene(i);
    }
}
