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
        // Variable für die kleinste maximale Fontgröße
        float minMaxSize = float.MaxValue;

        // Zwinge das Canvas zur Aktualisierung, damit alle UI-Elemente ihre endgültigen Größen haben
        Canvas.ForceUpdateCanvases();

        // Gehe durch alle TextMeshProUGUI-Elemente und berechne die maximale Fontgröße
        foreach (TextMeshProUGUI text in texts)
        {
            // Temporäre Berechnung der maximalen Fontgröße mit AutoSizing
            text.enableAutoSizing = true;
            text.fontSizeMin = 10;
            text.fontSizeMax = 1000;

            // Zwinge das Mesh-Update und berechne die FontSize
            text.ForceMeshUpdate();  // Erzwinge das Mesh-Update für TextMeshPro

            // Berechne die tatsächliche Fontgröße, die der Text benötigt
            float currentMaxSize = text.fontSize;

            // Berechne die kleinste maximale Fontgröße, die für alle Textfelder geeignet ist
            if (currentMaxSize < minMaxSize)
            {
                minMaxSize = currentMaxSize;
            }
        }

        // Setze nun die berechnete Fontgröße auf allen Texten
        foreach (TextMeshProUGUI text in texts)
        {
            text.fontSizeMax = minMaxSize;
            text.fontSizeMin = minMaxSize;
        }
    }
}