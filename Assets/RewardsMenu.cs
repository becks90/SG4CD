using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardsMenu : MonoBehaviour
{

    public Image[] imgReward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Data.user.userlevel; i++)
        {
            imgReward[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBack(int i)
    {
        SceneManager.LoadScene(i);
    }
}
