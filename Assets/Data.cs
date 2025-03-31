
using System.Collections.Generic;

public static class Data
{
    public static User user { get; set; }
    public static Diary Diary { get; set; }
    public static Level level { get; set; }
    public static ScenariosPlayed scenariosPlayed { get; set; }
    public static List<AdvisedPerson> advisedPersons { get; set; }
    public static AdvisedPerson advisedPerson {  get; set; }
    
    static Data()
    {
        user = new User();
        Diary = new Diary();
        level = new Level();
        scenariosPlayed = new ScenariosPlayed();
        advisedPersons = new List<AdvisedPerson>();
    }
}