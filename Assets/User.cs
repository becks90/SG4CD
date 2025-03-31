using System;

[Serializable]
public class User
{
    public int id;
    public string username; 
    public string lastlogin;
    public int streak; 
    public string name;
    public int recordstreak;
    public string email;
    public string registration;
    public int userlevel;
    public int userpoints;
    public int usertype;
    public int? advisorid;

    // Hilfsmethode, um 'lastlogin' als DateTime Objekt zu erhalten
    public DateTime GetLastLogin()
    {
        return DateTime.Parse(lastlogin);
    }
}
