using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHaveUI : MonoBehaviour
{
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private Sprite enabledSprite;
    private Image image;
    void Awake()
    {
        image = GetComponent<Image>();
        image.sprite = disabledSprite;
    }
    public void ActivateIcon()
    {
        image.sprite = enabledSprite;
    }
    public void DeactivateIcon()
    {
        image.sprite = disabledSprite;
    }
}
