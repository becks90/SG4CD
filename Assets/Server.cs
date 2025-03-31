public static class Server
{
    // Ngrok forwarding zum lokalen MAMP Server
    public static string ip = "https://8f34-213-185-75-60.ngrok-free.app/sqlconnect/"; 
    
    // DDNS Weiterleitung zum lokalen MAMP Server, funktioniert mit Itch.io nicht, wegen fehlendem SSL Zertifikat, mit der Folge eines MixedContent Fehlers
    //public static string ip = "http://becksen90.ddns.net:62387/sqlconnect/";

    // Localhost MAMP Server
    //public static string ip = "http://localhost/sqlconnect/";
}
