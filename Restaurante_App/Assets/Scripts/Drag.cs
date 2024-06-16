using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Hacer que el objeto sea semi-transparente mientras se arrastra.
        canvasGroup.blocksRaycasts = false; // Permitir que los raycasts pasen a través del objeto mientras se arrastra.

        // Calcular el offset inicial desde el centro del objeto al punto de clic
        offset = rectTransform.anchoredPosition - eventData.position / canvas.scaleFactor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Obtener la posición ajustada en el canvas
        Vector2 mousePosition = eventData.position / canvas.scaleFactor;
        rectTransform.anchoredPosition = mousePosition + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f; // Restaurar la opacidad.
        canvasGroup.blocksRaycasts = true; // Bloquear los raycasts nuevamente.
    }
}
