using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTutorialPanelController : MonoBehaviour
{
    private ItemTutorialPanel viewer;
    private Dictionary<string, ItemTutorialData> data;
    private AudioSource sfxPlayer;

    [SerializeField] private GameObject target;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private ItemTutorialData[] rawData;

    void Awake()
    {
        data = new Dictionary<string, ItemTutorialData>();
        foreach(var item in rawData)
        {
            data[item.identifier] = item;
        }
        viewer = target.GetComponent<ItemTutorialPanel>();
        sfxPlayer = GetComponent<AudioSource>();
    }
    public void Show(string identifier)
    {
        string seenFlag = "seen_" + identifier;
        ItemTutorialData toShowData;
        if(!data.TryGetValue(identifier, out toShowData)) return;
        if(GameManager.Instance == null || GameManager.Instance.GetFlag(seenFlag) == true) return;
        viewer.SetData(toShowData);
        viewer.Show();
        sfxPlayer.PlayOneShot(sfx);
        GameManager.Instance.SaveFlag(seenFlag, true);
    }
}