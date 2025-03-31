using System.Collections.Generic;

public class Answer
{
    public string answer { get; set; }
    public int points { get; set; }
    public int answerId { get; set; }
    public string response { get; set; }
    public int answercategory { get; set; }
}

public class Scenario
{
    public int scenarioId { get; set; }
    public string Name { get; set; }
    public string question { get; set; }
    public int categoryId { get; set; }
    public List<Answer> answers { get; set; }
}

public class ScenarioResponse
{
    public string status { get; set; }
    public Scenario scenario { get; set; }
}

public class ScenarioPlans
{
    public string status { get; set; }
    public List<ScenarioPlan> scenarioplan { get; set; }
}

public class ScenarioPlan
{
    public int id {  get; set; }
    public int userId { get; set; }
    public int? nextScenarioId   { get; set; }
    public int? nextCategoryId   { get; set; }
    public int questionOrder    { get; set; }
}