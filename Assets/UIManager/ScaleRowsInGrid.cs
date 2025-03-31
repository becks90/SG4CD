using UnityEngine;
using UnityEngine.UI;

public class ScaleRowsInContainer : MonoBehaviour
{
    // Referenz zum ctnLogin Container (das GameObject, das die GridLayoutGroup enth�lt)
    public GameObject _ctn;

    private RectTransform parentRect;
    private GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        // Hole das RectTransform und die GridLayoutGroup-Komponente
        parentRect = _ctn.GetComponent<RectTransform>();
        gridLayoutGroup = _ctn.GetComponent<GridLayoutGroup>();

        // Setze Zellengr��e
        UpdateGridLayoutSize();
    }

    void Update()
    {
        // �berpr�fe, ob sich die Gr��e des Parent-Containers ge�ndert hat
        if (parentRect.hasChanged)
        {
            // Wenn sich die Gr��e ge�ndert hat, berechne die neue Zellengr��e
            UpdateGridLayoutSize();
            // Setze hasChanged auf false, um unn�tige Berechnungen zu vermeiden
            parentRect.hasChanged = false;
        }
    }

    void UpdateGridLayoutSize()
    {
        float containerWidth = parentRect.rect.width;
        float containerHeight = parentRect.rect.height;

        // 50% der Breite
        float columnWidth = containerWidth / gridLayoutGroup.constraintCount;

        // H�he der Elemente = H�he des Parent : (Anzahl der Elemente : Spalten)
        float rowHeight = containerHeight / (_ctn.transform.childCount / gridLayoutGroup.constraintCount);

        // Setze nun die Zellengr��e, sodass alle Zellen korrekt skaliert werden
        gridLayoutGroup.cellSize = new Vector2(columnWidth, rowHeight);

        // Erzwinge ein Layout-Update nach der Anpassung
        Canvas.ForceUpdateCanvases();
    }
}