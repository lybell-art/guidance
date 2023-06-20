using UnityEngine;

[CreateAssetMenu(fileName = "ItemTutorialData", menuName = "Data/ItemTutorialData", order = 1)]
public class ItemTutorialData : ScriptableObject
{
    public Sprite sprite;
    public string title;
    [TextArea] public string description;
    public string identifier;
}