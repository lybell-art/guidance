using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizdomTutorialHandler: MonoBehaviour
{
    private ItemTutorialPanelController itemDescPanel;
    void Awake()
    {
        itemDescPanel = GetComponent<ItemTutorialPanelController>();
    }
    public void OnChangeWizdomCount(int count)
    {
        if(count == 1) itemDescPanel.Show(Constants.wisdomName);
        if(count == 3) itemDescPanel.Show(Constants.deliberationName);
    }
}