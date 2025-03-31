using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;  // Referenz zur GridLayoutGroup
    public List<Button> buttons = new List<Button>();  // Liste der Buttons
    public float minTextSize = 14f;  // Mindestschriftgr��e f�r Auto-Size
    public float buttonPadding = 40f;  // Padding um den Text in den Buttons
    private RectTransform parentRect;
    private CanvasScaler canvasScaler;  // CanvasScaler Referenz

    void Start()
    {
        // RectTransform des Parent-Objekts (GridLayoutGroup)
        parentRect = gridLayoutGroup.GetComponent<RectTransform>();
        canvasScaler = GetComponentInParent<CanvasScaler>();  // CanvasScaler-Referenz holen

        // Initialisiere das Layout und zwinge das Canvas, alle Updates durchzuf�hren
        Canvas.ForceUpdateCanvases();

        // Setze die Schriftgr��en und Button-Gr��en f�r alle Buttons
        SetScaling();
    }

    void Update()
    {
        // Wenn die Gr��e sich ge�ndert hat, Skalierung neu setzen
        SetScaling();

    }

    void SetScaling()
    {
        // Berechne die Zellgr��e, damit die Buttons den verf�gbaren Raum ausf�llen
        float cellWidth = (parentRect.rect.width - (gridLayoutGroup.spacing.x * (gridLayoutGroup.constraintCount - 1))) / gridLayoutGroup.constraintCount;
        float cellHeight = (parentRect.rect.height - (gridLayoutGroup.spacing.y * (gridLayoutGroup.constraintCount - 1))) / gridLayoutGroup.constraintCount;
        gridLayoutGroup.cellSize = new Vector2(cellWidth, cellHeight);

        // Hier kannst du das Padding der GridLayoutGroup anpassen
        gridLayoutGroup.padding.top = 20;  // Beispiel: Einr�cken von oben
        gridLayoutGroup.padding.right = 20;  // Beispiel: Einr�cken von rechts

        // Verwende die CanvasScaler-Skalierung
        float scaleFactor = canvasScaler.scaleFactor;

        // Bestimme f�r jeden Button die maximale Schriftgr��e, die in den Button passt
        List<float> maxFontSizes = buttons.Select(btn =>
        {
            RectTransform btnRect = btn.GetComponent<RectTransform>();
            float maxWidth = btnRect.rect.width - 2 * buttonPadding;  // Padding links und rechts
            float maxHeight = btnRect.rect.height - 2 * buttonPadding;  // Padding oben und unten

            // Berechne die maximale Schriftgr��e basierend auf der kleineren Dimension (Breite oder H�he)
            float maxFontSizeForThisButton = Mathf.Min(maxWidth, maxHeight) * 0.4f; // 40% der kleineren Dimension
            return maxFontSizeForThisButton;
        }).ToList();

        // Finde die kleinste der maximalen Schriftgr��en (der Wert, den alle Buttons bekommen)
        float minFontSize = Mathf.Max(maxFontSizes.Min(), minTextSize);  // Stelle sicher, dass die Schriftgr��e nicht kleiner als der Mindestwert wird

        // Skalierung der Schriftgr��e basierend auf dem CanvasScaler
        float finalFontSize = minFontSize * scaleFactor;

        // Setze nun f�r alle Buttons die Schriftgr��e auf den finalen Wert
        SetTextSizeForAllButtons(finalFontSize);

        // Erzwinge das Layout-Update, um sicherzustellen, dass der Text korrekt angezeigt wird
        Canvas.ForceUpdateCanvases();
    }


    void SetTextSizeForAllButtons(float fontSize)
    {
        // Setze f�r jeden Button die gleiche Schriftgr��e und aktiviere AutoSizing
        foreach (Button btn in buttons)
        {
            TextMeshProUGUI text = btn.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                // AutoSizing deaktivieren und die Schriftgr��e auf den berechneten Wert setzen
                text.enableAutoSizing = false;
                text.fontSize = fontSize;  // Schriftgr��e auf den berechneten Wert setzen

                // Setze das TextWrappingMode auf Normal f�r Wortumbruch
                text.textWrappingMode = TextWrappingModes.Normal;  // Zeilenumbruch nach W�rtern

                // Setze den OverflowMode auf Overflow, um den Text nicht abzuschneiden
                text.overflowMode = TextOverflowModes.Overflow;  // Text wird nicht abgeschnitten

                // Padding hinzuf�gen, um Text vom Rand des Buttons zu entfernen
                text.margin = new Vector4(buttonPadding, buttonPadding, buttonPadding, buttonPadding);  // Padding links, rechts, oben und unten

                // Erzwinge das Layout-Update, um sicherzustellen, dass der Text korrekt angezeigt wird
                text.ForceMeshUpdate();
            }
        }
    }
}