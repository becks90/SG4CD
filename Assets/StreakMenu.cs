using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class streak : MonoBehaviour
{
    public TMP_Text txtstreak, txtLastLogin, txtLevel, txtRewardBtn;
    public Button btnMenu, btnReward;
    public Image imgFillBar;

    void Start()
    {
        btnReward.enabled = false;
        btnMenu.enabled = false;
        SetXPBar();
        SetTexts();
    }

    private void SetTexts()
    {
        txtLastLogin.SetText("Dein letzter Besuch war am " + Data.user.lastlogin);

        if (Data.user.GetLastLogin().Date.Equals(DateTime.UtcNow.ToLocalTime().Date))
        {
            btnMenu.enabled = true;
            txtRewardBtn.text = "Heute bereits abgeholt";
            txtstreak.SetText($"Du hast eine andauernde Serie von {Data.user.streak} Tagen in Folge. Weiter so!");
        } else 
        {
        if (Data.user.GetLastLogin().AddDays(1).Date.Equals(DateTime.UtcNow.ToLocalTime().Date))
            {
                txtRewardBtn.text = "Serien-Bonus abholen!";
                btnReward.enabled = true;
                txtstreak.SetText($"Du hast eine andauernde Serie von {Data.user.streak} Tagen in Folge. Weiter so!");
            }
        else
            {
                txtRewardBtn.text = "Neue Serie starten!";
                txtstreak.text = $"Leider ist deine Login-Serie von {Data.user.streak} Tagen gerissen, Zeit eine neue zu starten!";
                btnReward.enabled = true;
            }
        }
    }

    public void GoToNextScene()
    {
        if (Data.Diary.entries.Any(entry => DateTime.Parse(entry.entrydate).Date == DateTime.Today))
        {
            SceneManager.LoadScene(5);
        }
        else
            SceneManager.LoadScene(4);
    }

    // Initialisiere XP-Bar
    private void SetXPBar()
    {
        imgFillBar.fillAmount = (float)Data.user.userpoints / (float)Data.level.points;
        txtLevel.text = "Level " + Data.user.userlevel;
    }

    // Füge Punkte hinzu und setze Buttons
    public void GetReward()
    {
        DataManager.Instance.Updatestreak((newPoints, newLevel) =>
        {
            SetTexts();
            StartCoroutine(ProgressBarFill(2));
        });
        btnReward.enabled = false;
        btnMenu.enabled = true;
    }

    // XP-Bar Fortschritt anzeigen
    IEnumerator ProgressBarFill(float time)
    {
        float startFill = imgFillBar.fillAmount;
        float elapsedTime = 0f;
        float targetfill = (float)Data.user.userpoints / (float)Data.level.points;

        txtLevel.text = $"+ {Data.user.streak % 8} XP!";
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
}