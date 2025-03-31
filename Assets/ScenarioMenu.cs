using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    private Scenario _scenario;
    private Answer _answer;
    public GameObject buttonPrefab;
    public Transform contentPanel;

    public GameObject btnChatPrefab;
    public Transform chatPanel;

    public Sprite _sprtChatLeft;
    public Sprite _sprtChatRight;

    public Image imgFillBar;
    public TMP_Text txtLevel;

    private List<Button> _buttons = new();

    void Start()
    {
        StartScenario();
        SetXPBar();
    }

    // Ruft das Szenario ab und speichert es
    private void StartScenario()
    {
        DataManager.Instance.GetScenario(Data.user.id, scenario =>
        {
            if (scenario != null && scenario.scenarioId != null)
            {
                _scenario = scenario;
                StartCoroutine(StartChat());
            }
            else
            {
                Debug.LogError("Scenario konnte nicht abgerufen werden!");
            }
        });
    }

    IEnumerator StartChat()
    {
        // Erstelle den Button aus dem Prefab und setze ihn ins Panel
        GameObject btnQuestion = Instantiate(btnChatPrefab, chatPanel);
        btnQuestion.SetActive(true);

        // Hole Textelement und setze Text
        TextMeshProUGUI txtBtnQuestion = btnQuestion.GetComponentInChildren<TextMeshProUGUI>();
        txtBtnQuestion.text = "Ben schreibt ...";
        txtBtnQuestion.enabled = true;
        txtBtnQuestion.margin = new Vector4(20, 0, 20, 0);

        btnQuestion.GetComponent<Image>().enabled = true;

        // Hole Bildelement und setz Hintergrundbild
        Image chatImage = btnQuestion.GetComponent<Image>();
        chatImage.sprite = _sprtChatLeft;
        chatImage.type = Image.Type.Simple;
        chatImage.preserveAspect = false;

        // Hole das RectTransform des Buttons und setze Höhe
        RectTransform questionRect = btnQuestion.GetComponent<RectTransform>();
        questionRect.sizeDelta = new Vector2(questionRect.sizeDelta.x, txtBtnQuestion.preferredHeight + 20f);

        yield return new WaitForSeconds(5);

        // Setze Frage
        txtBtnQuestion.text = _scenario.question;
        questionRect.sizeDelta = new Vector2(questionRect.sizeDelta.x, txtBtnQuestion.preferredHeight + 20f);

        StartCoroutine(ShowAnswers());
    }

    // Zeigt die Antwortmöglichkeiten
    IEnumerator ShowAnswers()
    {
        ReorderAnswers();
        
        yield return new WaitForSeconds(3);


        foreach (Answer _answer in _scenario.answers)
        {
            // Erstelle Button aus Prefab und aktiviere ihn
            GameObject newObject = Instantiate(buttonPrefab, contentPanel);
            newObject.SetActive(true);

            // Hole Textelement
            TextMeshProUGUI objTxt = newObject.GetComponentInChildren<TextMeshProUGUI>();
            objTxt.text = _answer.answer;
            objTxt.enabled = true;
            objTxt.margin = new Vector4(20, 0, 20, 0);

            // Hole Imageelement und setze es
            Image imgAnswer = newObject.GetComponent<Image>();
            imgAnswer.sprite = _sprtChatRight;
            imgAnswer.type = Image.Type.Simple;
            imgAnswer.preserveAspect = false;

            // Button holen und Clicklistener setzen
            Button btnAnswer = newObject.GetComponent<Button>();
            _buttons.Add(btnAnswer);
            btnAnswer.onClick.AddListener(() => OnAnswerButtonClicked(_answer));

            // Passe die Höhe des Buttons an den Text an (wird durch Layout Group gemacht)
            RectTransform btnRect = newObject.GetComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(btnRect.sizeDelta.x, objTxt.preferredHeight + 50f);
        }
    }

    // Klicklistener für die Antwortbuttons
    private void OnAnswerButtonClicked(Answer answer)
    {
        _answer = answer;
        foreach (Button btn in _buttons)
        {
            // Entferne alle Listener
            btn.onClick.RemoveAllListeners();

            // Deaktiviere den Button, sodass er nicht mehr klickbar ist
            btn.interactable = false;
        }
        CompleteScenario();
    }

    IEnumerator AddChat(string text, bool isRight, int startDelay, int writingDelay)
    {
        yield return new WaitForSeconds(startDelay);
        // Erstelle den Button aus dem Prefab und setze ihn ins Panel
        GameObject obj = Instantiate(btnChatPrefab, chatPanel);
        obj.SetActive(true);

        // Hole Textelement und setze Text
        TextMeshProUGUI txtObj = obj.GetComponentInChildren<TextMeshProUGUI>();
        txtObj.text = "Schreibt...";
        txtObj.margin = new Vector4(20, 0, 20, 0);
        txtObj.alignment = isRight ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
        txtObj.enabled = true;
        obj.GetComponent<Image>().enabled = true;

        // Hole Bildelement und setz Hintergrundbild
        Image imgObj = obj.GetComponent<Image>();
        imgObj.sprite = isRight? _sprtChatRight : _sprtChatLeft;
        imgObj.type = Image.Type.Simple;
        imgObj.preserveAspect = false;

        // Hole das RectTransform des Buttons und setze Höhe
        RectTransform questionRect = obj.GetComponent<RectTransform>();
        questionRect.sizeDelta = new Vector2(questionRect.sizeDelta.x, txtObj.preferredHeight + 20f);
        yield return new WaitForSeconds(writingDelay);
        txtObj.text = text;
        txtObj.alignment = TextAlignmentOptions.Left;
        questionRect.sizeDelta = new Vector2(questionRect.sizeDelta.x, txtObj.preferredHeight + 20f);
        if (!isRight)
        {
            StartCoroutine(UpdateXPBar(3));
        }
    }

    // Das Szenario in der Datenbank vervollständigen
    private void CompleteScenario()
    {
        DataManager.Instance.SetScenario(_scenario, _answer, success =>
        {
            if (success)
            {
                // Punkte hinzufügen, evtl Level erhöhen und Datenbank aktualisieren
                DataManager.Instance.AddPoints(_answer.points, (newPoints, newLevel) => { });
                
                // Ratschlag und Antwort im Chat anzeigen
                StartCoroutine(AddChat(_answer.answer, true, 0, 3));
                StartCoroutine(AddChat(_answer.response, false, 5, 3));
            }
            else
            {
                Debug.LogError("Scenario konnte nicht abgeschlossen werden!");
            }
        });
    }

    // Ordnet die Antworten zufällig neu an
    private void ReorderAnswers()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < _scenario.answers.Count; i++)
        {
            int j = rand.Next(i, _scenario.answers.Count);
            (_scenario.answers[j], _scenario.answers[i]) = (_scenario.answers[i], _scenario.answers[j]);
        }
    }

    // XP Bar zu beginn setzen
    private void SetXPBar()
    {
        imgFillBar.fillAmount = (float)Data.user.userpoints / (float)Data.level.points;
        txtLevel.text = "Level " + Data.user.userlevel;
    }

    // XP-Bar Fortschritt anzeigen
    IEnumerator UpdateXPBar(float time)
    {
        float startFill = imgFillBar.fillAmount;
        float elapsedTime = 0f;
        float targetfill = (float)Data.user.userpoints / (float)Data.level.points;

        txtLevel.text = $"+ {_answer.points} XP!";
        if (targetfill < startFill)
        {
            txtLevel.text = "LevelUp! Weiter so";
            startFill = 0;
        }
        while (elapsedTime < time)
        {
            imgFillBar.fillAmount = Mathf.Lerp(startFill, targetfill, elapsedTime / time);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        txtLevel.text = "Level " + Data.user.userlevel;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(5);
    }
}