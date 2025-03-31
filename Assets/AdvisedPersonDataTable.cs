using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI.Dates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvisedPersonDataTable : MonoBehaviour
{
    public DatePicker dpFrom, dpTo;
    private List<ScenarioPlayed> scenarios;

    public TextMeshProUGUI txtAllAll, txtAllFriends, txtAllFamily, txtAllSchool, txtAllSport;
    public TextMeshProUGUI txtAkzeptanzAll, txtAkzeptanzFriends, txtAkzeptanzFamily, txtAkzeptanzSchool, txtAkzeptanzSport;
    public TextMeshProUGUI txtProblemloesungAll, txtProblemloesungFriends, txtProblemloesungFamily, txtProblemloesungSchool, txtProblemloesungSport;
    public TextMeshProUGUI txtKognUmbAll, txtKognUmbFriends, txtKognUmbFamily, txtKognUmbSchool, txtKognUmbSport;
    public TextMeshProUGUI txtVermeidungAll, txtVermeidungFriends, txtVermeidungFamily, txtVermeidungSchool, txtVermeidungSport;
    public TextMeshProUGUI txtRuminationAll, txtRuminationFriends, txtRuminationFamily, txtRuminationSchool, txtRuminationSport;
    public TextMeshProUGUI txtUnterdrAll, txtUnterdrFriends, txtUnterdrFamily, txtUnterdrSchool, txtUnterdrSport;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scenarios = Data.advisedPerson.scenariosPlayed.scenariosPlayed;
        dpFrom.SelectedDate = DateTime.UtcNow.ToLocalTime().AddMonths(-1);
        dpTo.SelectedDate = DateTime.UtcNow.ToLocalTime();
        SetValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues()
    {
        Debug.Log($"Anzahl der gespielten Szenarios: {scenarios.Count}");
        List<ScenarioPlayed> filteredScenarios = Data.advisedPerson.scenariosPlayed.scenariosPlayed.Where(s => s.GetDateTime() >= dpFrom.SelectedDate &&
                                                                                                            s.GetDateTime() <= dpTo.SelectedDate).ToList();
        Debug.Log($"Anzahl der gespielten Szenarios im ausgewählten Zeitraum: {filteredScenarios.Count}");
        int countAll = filteredScenarios.Count();
        int countAkzeptanz = filteredScenarios.Where(s => s.answercategory_name.Equals("Akzeptanz")).Count();
        int countProb = filteredScenarios.Where(s => s.answercategory_name.Equals("Problemlösung")).Count();
        int countKogn = filteredScenarios.Where(s => s.answercategory_name.Equals("Kognitive Umbewertung")).Count();
        int countVermeidung = filteredScenarios.Where(s => s.answercategory_name.Equals("Vermeidung")).Count();
        int countRumination = filteredScenarios.Where(s => s.answercategory_name.Equals("Rumination")).Count();
        int countUnterdr = filteredScenarios.Where(s => s.answercategory_name.Equals("Unterdrückung des Gefühlsausdrucks")).Count();
        int countFriends = filteredScenarios.Where(s => s.category_name.Equals("Freunde")).Count();
        int countFamily = filteredScenarios.Where(s => s.category_name.Equals("Familie")).Count();
        int countSchool = filteredScenarios.Where(s => s.category_name.Equals("Schule")).Count();
        int countSport = filteredScenarios.Where(s => s.category_name.Equals("Sport")).Count();

        txtAllAll.SetText(filteredScenarios.Count().ToString());
        txtAllFriends.SetText(countFriends.ToString());
        txtAllFamily.SetText(countFamily.ToString());
        txtAllSchool.SetText(countSchool.ToString());
        txtAllSport.SetText(countSport.ToString());

        txtAkzeptanzAll.SetText(GetDisplayText(countAkzeptanz, countAll));
        txtProblemloesungAll.SetText(GetDisplayText(countProb, countAll));
        txtKognUmbAll.SetText(GetDisplayText(countKogn, countAll));
        txtVermeidungAll.SetText(GetDisplayText(countVermeidung, countAll));
        txtRuminationAll.SetText(GetDisplayText(countRumination, countAll));
        txtUnterdrAll.SetText(GetDisplayText(countUnterdr, countAll));

        txtAkzeptanzFriends.SetText(GetDisplayText(filteredScenarios.
                Where(s => s.category_name.Equals("Freunde") && s.answercategory_name.Equals("Akzeptanz"))
                .Count(), countFriends));
        txtAkzeptanzFamily.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Familie") && s.answercategory_name.Equals("Akzeptanz"))
                .Count(), countFamily));
        txtAkzeptanzSchool.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Schule") && s.answercategory_name.Equals("Akzeptanz"))
                .Count(), countSchool));
        txtAkzeptanzSport.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Sport") && s.answercategory_name.Equals("Akzeptanz"))
                .Count(), countSport));

        txtProblemloesungFriends.SetText(GetDisplayText(filteredScenarios.
                Where(s => s.category_name.Equals("Freunde") && s.answercategory_name.Equals("Problemlösung"))
                .Count(), countFriends));
        txtProblemloesungFamily.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Familie") && s.answercategory_name.Equals("Problemlösung"))
                .Count(), countFamily));
        txtProblemloesungSchool.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Schule") && s.answercategory_name.Equals("Problemlösung"))
                .Count(), countSchool));
        txtProblemloesungSport.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Sport") && s.answercategory_name.Equals("Problemlösung"))
                .Count(), countSport));

        txtKognUmbFriends.SetText(GetDisplayText(filteredScenarios.
                Where(s => s.category_name.Equals("Freunde") && s.answercategory_name.Equals("Kognitive Umbewertung"))
                .Count(), countFriends));
        txtKognUmbFamily.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Familie") && s.answercategory_name.Equals("Kognitive Umbewertung"))
                .Count(), countFamily));
        txtKognUmbSchool.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Schule") && s.answercategory_name.Equals("Kognitive Umbewertung"))
                .Count(), countSchool));
        txtKognUmbSport.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Sport") && s.answercategory_name.Equals("Kognitive Umbewertung"))
                .Count(), countSport));

        txtVermeidungFriends.SetText(GetDisplayText(filteredScenarios.
                Where(s => s.category_name.Equals("Freunde") && s.answercategory_name.Equals("Vermeidung"))
                .Count(), countFriends));
        txtVermeidungFamily.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Familie") && s.answercategory_name.Equals("Vermeidung"))
                .Count(), countFamily));
        txtVermeidungSchool.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Schule") && s.answercategory_name.Equals("Vermeidung"))
                .Count(), countSchool));
        txtVermeidungSport.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Sport") && s.answercategory_name.Equals("Vermeidung"))
                .Count(), countSport));

        txtRuminationFriends.SetText(GetDisplayText(filteredScenarios.
                Where(s => s.category_name.Equals("Freunde") && s.answercategory_name.Equals("Rumination"))
                .Count(), countFriends));
        txtRuminationFamily.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Familie") && s.answercategory_name.Equals("Rumination"))
                .Count(), countFamily));
        txtRuminationSchool.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Schule") && s.answercategory_name.Equals("Rumination"))
                .Count(), countSchool));
        txtRuminationSport.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Sport") && s.answercategory_name.Equals("Rumination"))
                .Count(), countSport));

        txtUnterdrFriends.SetText(GetDisplayText(filteredScenarios.
                Where(s => s.category_name.Equals("Freunde") && s.answercategory_name.Equals("Unterdrückung des Gefühlsausdrucks"))
                .Count(), countFriends));
        txtUnterdrFamily.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Familie") && s.answercategory_name.Equals("Unterdrückung des Gefühlsausdrucks"))
                .Count(), countFamily));
        txtUnterdrSchool.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Schule") && s.answercategory_name.Equals("Unterdrückung des Gefühlsausdrucks"))
                .Count(), countSchool));
        txtUnterdrSport.SetText(GetDisplayText(filteredScenarios
                .Where(s => s.category_name.Equals("Sport") && s.answercategory_name.Equals("Unterdrückung des Gefühlsausdrucks"))
                .Count(), countSport));
    }

    private string GetDisplayText(int filteredCount, int count)
    {
        if (count == 0)
        {
            return "0/0 (0%)";
        }

        float percentage = (float)filteredCount / count * 100;
        return $"{filteredCount}/{count} ({percentage:F2}%)";  // F2 gibt 2 Dezimalstellen an
    }

    public void GoBack(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
