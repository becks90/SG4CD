using UnityEngine;
using UnityEngine.UI;

public class ScaleContainer : MonoBehaviour
{

    public RectTransform ctnData;          // Der Elterncontainer (ctnData)
    public RectTransform ctnUserData;      // Das erste Kind (ctnUserData)
    public RectTransform ctnGamedata;      // Das zweite Kind (ctnGamedata)

    public GridLayoutGroup ctnUserDataGrid;  // Erste GridLayoutGroup
    public GridLayoutGroup ctnGamedataGrid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdjustContainerSizes();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustContainerSizes();
    }

    void AdjustContainerSizes()
    {
        // Holen der Breite und Höhe von ctnData
        float dataWidth = ctnData.rect.width;
        float dataHeight = ctnData.rect.height;

        // Setze die Position und die Größe von ctnUserData (linke Hälfte)
        SetContainerSizeAndPosition(ctnUserData, 0, 0, dataWidth * 0.5f, dataHeight);

        // Setze die Position und die Größe von ctnGamedata (rechte Hälfte)
        SetContainerSizeAndPosition(ctnGamedata, dataWidth * 0.5f, 0, dataWidth * 0.5f, dataHeight);
    }

    void SetContainerSizeAndPosition(RectTransform container, float xPos, float yPos, float width, float height)
    {
        // Stelle die Position des Containers ein
        container.anchoredPosition = new Vector2(xPos, yPos);

        // Stelle die Größe des Containers ein
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}
