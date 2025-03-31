using UnityEngine;
using UnityEngine.UI;

public class ScaleRowsInContainer : MonoBehaviour
{
    // Referenz zum ctnLogin Container (das GameObject, das die GridLayoutGroup enthält)
    public GameObject _ctn;

    private RectTransform parentRect;
    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        // Hole das RectTransform und die GridLayoutGroup-Komponente
        parentRect = _ctn.GetComponent<RectTransform>();
        gridLayoutGroup = _ctn.GetComponent<GridLayoutGroup>();

        // Setze Zellengröße
        UpdateGridLayoutSize();
    }

    void Update()
    {
        // Überprüfe, ob sich die Größe des Parent-Containers geändert hat
        if (parentRect.hasChanged)
        {
            // Wenn sich die Größe geändert hat, berechne die neue Zellengröße
            UpdateGridLayoutSize();
            // Setze hasChanged auf false, um unnötige Berechnungen zu vermeiden
            parentRect.hasChanged = false;
        }
    }

    void UpdateGridLayoutSize()
    {
        float containerWidth = parentRect.rect.width;
        float containerHeight = parentRect.rect.height;

        // 50% der Breite
        float columnWidth = containerWidth / gridLayoutGroup.constraintCount;

        // Höhe der Elemente = Höhe des Parent : (Anzahl der Elemente : Spalten)
        float rowHeight = containerHeight / (_ctn.transform.childCount / gridLayoutGroup.constraintCount);

        // Setze nun die Zellengröße, sodass alle Zellen korrekt skaliert werden
        gridLayoutGroup.cellSize = new Vector2(columnWidth, rowHeight);

        // Erzwinge ein Layout-Update nach der Anpassung
        Canvas.ForceUpdateCanvases();
    }
}