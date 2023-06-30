using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite normalImage;
    [SerializeField] private Sprite hoverImage;
    private Image imageRenderer;

    void Awake()
    {
        imageRenderer = GetComponent<Image>();
        ChangeImage(normalImage);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerId >= 0) return;
        ChangeImage(hoverImage);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(eventData.pointerId >= 0) return;
        ChangeImage(normalImage);
    }

    private void ChangeImage(Sprite image)
    {
        if(image == null) imageRenderer.enabled = false;
        else
        {
            imageRenderer.enabled = true;
            imageRenderer.sprite = image;
        }
    }
}
