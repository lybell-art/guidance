using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
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
        ChangeImage(hoverImage);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ChangeImage(normalImage);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
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
