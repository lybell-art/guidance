using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderIcon : MonoBehaviour, IPointerClickHandler
{
    private Image image;
    private ISliderController sliderController;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    [SerializeField] private GameObject sliderControllerObj;
    void Awake()
    {
        image = GetComponent<Image>();
        sliderController = sliderControllerObj.GetComponent<ISliderController>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        sliderController.ToggleMute();
    }
    public void OnChange(float value)
    {
        if(Mathf.Approximately(value, 0)) image.sprite = offSprite;
        else image.sprite = onSprite;
    }
}
