using UnityEngine;

[CreateAssetMenu(fileName = "parallax", menuName = "CustomSource/BackgroundSource", order = 1)]
public class BackgroundSource : ScriptableObject
{
    public int minY;
    public int maxY;
    public BackgroundLayerSource[] sources;
    public int length
    {
        get { return sources.Length; }
    }
    public BackgroundLayerSource GetLayer(int i)
    {
        if(i < 0 || i >= length) return null;
        return sources[i];
    }
}