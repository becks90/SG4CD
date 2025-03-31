using System;
using System.Collections.Generic;
using System.Linq;

public class ScenariosPlayed
{
    public string status { get; set; }
    public List<ScenarioPlayed> scenariosPlayed { get; set; }

    // Gibt ein DateTime Element des neuesten PlayedScenario zurück
    public DateTime GetLatestScenarioDate()
    {
        if (scenariosPlayed == null || scenariosPlayed.Count == 0)
        {
            throw new InvalidOperationException("Es gibt keine Szenarien.");
        }

        var newestScenario = scenariosPlayed
            .OrderByDescending(s => s.GetDateTime())  
            .FirstOrDefault();

        return newestScenario.GetDateTime();
    }
}

public class ScenarioPlayed
{
    public int id { get; set; }
    public string name { get; set; }
    public string question { get; set; }
    public string category_name { get; set; }
    public string answer { get; set; }
    public string answercategory_name { get; set; }
    public string date { get; set; }

    public DateTime GetDateTime()
    {
        return DateTime.Parse(date);
    }
}
