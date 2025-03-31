using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetFontsizes : MonoBehaviour
{
    public List<TextMeshProUGUI> texts;
    
    private void Start()
    {
        SetFontSize();
    }

    private void Update()
    {
        SetFontSize();
    }

    void SetFontSize()
    {
        // Variable f�r die kleinste maximale Fontgr��e
        float minMaxSize = float.MaxValue;

        // Zwinge das Canvas zur Aktualisierung, damit alle UI-Elemente ihre endg�ltigen Gr��en haben
        Canvas.ForceUpdateCanvases();

        // Gehe durch alle TextMeshProUGUI-Elemente und berechne die maximale Fontgr��e
        foreach (TextMeshProUGUI text in texts)
        {
            // Tempor�re Berechnung der maximalen Fontgr��e mit AutoSizing
            text.enableAutoSizing = true;
            text.fontSizeMin = 10;
            text.fontSizeMax = 1000;

            // Zwinge das Mesh-Update und berechne die FontSize
            text.ForceMeshUpdate();  // Erzwinge das Mesh-Update f�r TextMeshPro

            // Berechne die tats�chliche Fontgr��e, die der Text ben�tigt
            float currentMaxSize = text.fontSize;

            // Berechne die kleinste maximale Fontgr��e, die f�r alle Textfelder geeignet ist
            if (currentMaxSize < minMaxSize)
            {
                minMaxSize = currentMaxSize;
            }
        }

        // Setze nun die berechnete Fontgr��e auf allen Texten
        foreach (TextMeshProUGUI text in texts)
        {
            text.fontSizeMax = minMaxSize;
            text.fontSizeMin = minMaxSize;
        }
    }
}