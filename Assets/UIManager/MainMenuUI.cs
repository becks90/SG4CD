using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;  // Referenz zur GridLayoutGroup
    public List<Button> buttons = new List<Button>();  // Liste der Buttons
    public float minTextSize = 14f;  // Mindestschriftgröße für Auto-Size
    public float buttonPadding = 40f;  // Padding um den Text in den Buttons
    private RectTransform parentRect;
    private CanvasScaler canvasScaler;  // CanvasScaler Referenz

    void Start()
    {
        // RectTransform des Parent-Objekts (GridLayoutGroup)
        parentRect = gridLayoutGroup.GetComponent<RectTransform>();
        canvasScaler = GetComponentInParent<CanvasScaler>();  // CanvasScaler-Referenz holen

        // Initialisiere das Layout und zwinge das Canvas, alle Updates durchzuführen
        Canvas.ForceUpdateCanvases();

        // Setze die Schriftgrößen und Button-Größen für alle Buttons
        SetScaling();
    }

    void Update()
    {
        // Wenn die Größe sich geändert hat, Skalierung neu setzen
        SetScaling();

    }

    void SetScaling()
    {
        // Berechne die Zellgröße, damit die Buttons den verfügbaren Raum ausfüllen
        float cellWidth = (parentRect.rect.width - (gridLayoutGroup.spacing.x * (gridLayoutGroup.constraintCount - 1))) / gridLayoutGroup.constraintCount;
        float cellHeight = (parentRect.rect.height - (gridLayoutGroup.spacing.y * (gridLayoutGroup.constraintCount - 1))) / gridLayoutGroup.constraintCount;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Hier kannst du das Padding der GridLayoutGroup anpassen
        gridLayoutGroup.padding.top = 20;  // Beispiel: Einrücken von oben
        gridLayoutGroup.padding.right = 20;  // Beispiel: Einrücken von rechts

        // Verwende die CanvasScaler-Skalierung
        float scaleFactor = canvasScaler.scaleFactor;

        // Bestimme für jeden Button die maximale Schriftgröße, die in den Button passt
        List<float> maxFontSizes = buttons.Select(btn =>
        {
            RectTransform btnRect = btn.GetComponent<RectTransform>();
            float maxWidth = btnRect.rect.width - 2 * buttonPadding;  // Padding links und rechts
            float maxHeight = btnRect.rect.height - 2 * buttonPadding;  // Padding oben und unten

            // Berechne die maximale Schriftgröße basierend auf der kleineren Dimension (Breite oder Höhe)
            float maxFontSizeForThisButton = Mathf.Min(maxWidth, maxHeight) * 0.4f; // 40% der kleineren Dimension
            return maxFontSizeForThisButton;
        }).ToList();

        // Finde die kleinste der maximalen Schriftgrößen (der Wert, den alle Buttons bekommen)
        float minFontSize = Mathf.Max(maxFontSizes.Min(), minTextSize);  // Stelle sicher, dass die Schriftgröße nicht kleiner als der Mindestwert wird

        // Skalierung der Schriftgröße basierend auf dem CanvasScaler
        float finalFontSize = minFontSize * scaleFactor;

        // Setze nun für alle Buttons die Schriftgröße auf den finalen Wert
        SetTextSizeForAllButtons(finalFontSize);

        // Erzwinge das Layout-Update, um sicherzustellen, dass der Text korrekt angezeigt wird
        Canvas.ForceUpdateCanvases();
    }


    void SetTextSizeForAllButtons(float fontSize)
    {
        // Setze für jeden Button die gleiche Schriftgröße und aktiviere AutoSizing
        foreach (Button btn in buttons)
        {
            TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                // AutoSizing deaktivieren und die Schriftgröße auf den berechneten Wert setzen
                text.enableAutoSizing = false;
                text.fontSize = fontSize;  // Schriftgröße auf den berechneten Wert setzen

                // Setze das TextWrappingMode auf Normal für Wortumbruch
                text.textWrappingMode = TextWrappingModes.Normal;  // Zeilenumbruch nach Wörtern

                // Setze den OverflowMode auf Overflow, um den Text nicht abzuschneiden
                text.overflowMode = TextOverflowModes.Overflow;  // Text wird nicht abgeschnitten

                // Padding hinzufügen, um Text vom Rand des Buttons zu entfernen
                text.margin = new Vector4(buttonPadding, buttonPadding, buttonPadding, buttonPadding);  // Padding links, rechts, oben und unten

                // Erzwinge das Layout-Update, um sicherzustellen, dass der Text korrekt angezeigt wird
                text.ForceMeshUpdate();
            }
        }
    }
}