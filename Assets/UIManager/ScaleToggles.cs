using UnityEngine;
using UnityEngine.UI;

public class ToggleSizer : MonoBehaviour
{
    public GameObject grpToggles;  // Das Parent-Objekt f�r die Toggles
    public float padding = 20f;  // Abstand zu den R�ndern des Parents (oben, unten, links, rechts)
    public float spacing = 20f;  // Abstand zwischen den Toggles

    private RectTransform parentRect;  // Das RectTransform des Parent-Objekts
    private Toggle[] toggles;  // Array f�r die Toggles
    private GridLayoutGroup gridLayout;  // GridLayoutGroup des Parent-Objekts

    void Start()
    {
        // Holen des RectTransforms des Parent-Objekts und der GridLayoutGroup
        parentRect = grpToggles.GetComponent<RectTransform>();
        gridLayout = grpToggles.GetComponent<GridLayoutGroup>();

        // Holen der Toggles im Parent
        toggles = grpToggles.GetComponentsInChildren<Toggle>();
    }

    void Update()
    {
        // Berechne die verf�gbare Breite des Containers, unter Ber�cksichtigung von Padding und Spacing
        float availableWidth = parentRect.rect.width - 2 * padding - (toggles.Length - 1) * spacing;

        // Berechne die maximale Breite f�r jedes Toggle, sodass alle den verf�gbaren Platz gleichm��ig nutzen
        float maxWidth = availableWidth / toggles.Length;

        // Berechne die maximale H�he, damit die Toggles quadratisch bleiben
        float maxHeight = parentRect.rect.height - 2 * padding; // Die H�he des Containers ohne Padding

        // Bestimme die maximale Gr��e, die ein Toggle haben kann
        float maxSize = Mathf.Min(maxWidth, maxHeight); // Der kleinere Wert bestimmt die maximale Gr��e

        // Setze die Gr��e f�r jedes Toggle
        foreach (var toggle in toggles)
        {
            RectTransform toggleRect = toggle.GetComponent<RectTransform>();
            toggleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxSize);
            toggleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxSize);
        }
    }
}
