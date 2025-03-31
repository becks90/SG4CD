using UnityEngine;
using UnityEngine.UI;

public class ScaleInParent : MonoBehaviour
{   
    public RectTransform trfCtn, trfDate, trfType, trfEntry, trfImage, trfButton;

    void Start()
    {
        ResizeEntries();
    }

    private void Update()
    {
        ResizeEntries();
    }

    private void ResizeEntries()
    {
        float height = trfCtn.rect.width * 0.05f;
        // Setze die Breite für das Datum (z.B. 15% der Parent-Breite)
        RectTransform dateRect = trfDate.GetComponent<RectTransform>();
        dateRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trfCtn.rect.width * 0.15f);
        LayoutElement dateLayout = trfDate.GetComponent<LayoutElement>();
        dateLayout.preferredWidth = trfCtn.rect.width * 0.15f; // Setze preferred width auf 15%

        // Setze die Breite für den Typ (z.B. 10% der Parent-Breite)
        RectTransform typeRect = trfType.GetComponent<RectTransform>();
        typeRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trfCtn.rect.width * 0.10f);
        LayoutElement typeLayout = trfType.GetComponent<LayoutElement>();
        typeLayout.preferredWidth = trfCtn.rect.width * 0.10f;

        // Setze die Breite für den Text (z.B. 60% der Parent-Breite)
        RectTransform entryRect = trfEntry.GetComponent<RectTransform>();
        entryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trfCtn.rect.width * 0.60f);
        LayoutElement entryLayout = trfEntry.GetComponent<LayoutElement>();
        entryLayout.preferredWidth = trfCtn.rect.width * 0.60f;

        // Setze die Breite für das Bild (z.B. 10% der Parent-Breite)
        RectTransform imageRect = trfImage.GetComponent<RectTransform>();
        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trfCtn.rect.width * 0.10f);
        LayoutElement imageLayout = trfImage.GetComponent<LayoutElement>();
        imageLayout.preferredWidth = trfCtn.rect.width * 0.10f;

        // Setze die Breite für den Button (z.B. 15% der Parent-Breite)
        RectTransform buttonRect = trfButton.GetComponent<RectTransform>();
        buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, trfCtn.rect.width * 0.15f);
        LayoutElement buttonLayout = trfButton.GetComponent<LayoutElement>();
        buttonLayout.preferredWidth = trfCtn.rect.width * 0.15f;
    }
}
