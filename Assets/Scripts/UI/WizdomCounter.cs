using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizdomCounter : MonoBehaviour
{
    private Image[] icons;
    private GameObject[] children;
    [SerializeField] private Sprite fillImage;
    [SerializeField] private Sprite blankImage;

    void Awake()
    {
        children = new GameObject[Constants.maxWizdomCount];
        icons = new Image[Constants.maxWizdomCount];
        for(int i=0; i<Constants.maxWizdomCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
            icons[i] = children[i]?.GetComponent<Image>();
            children[i].SetActive(false);
        }
    }

    public void OnChangeWizdomCount(int wizdom)
    {
        for(int i=0; i<Constants.maxWizdomCount; i++)
        {
            if(wizdom > 0)
            {
                children[i].SetActive(true);
                icons[i].sprite = (i < wizdom) ? fillImage : blankImage;
            }
            else children[i].SetActive(false);
        }
    }
}
