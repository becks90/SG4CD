using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public TMP_InputField inpServer;
    public void GoToScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void SetServer()
    {
        Server.ip = inpServer.text + "/sqlconnect/";
    }
}