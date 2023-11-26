using UnityEditor;
using UnityEngine;

public class PlayerResetter : Editor
{
    [MenuItem("Guidance/Reset Student Position")]
    public static void TrySaveStage()
    {
        Vector3 initialPosition = new Vector3(-4f, -2f, 0f);
        GameObject player = GameObject.Find("player");
        GameObject cameraMover = GameObject.Find("cameraMover");
        player.transform.position = initialPosition;
        cameraMover.transform.position =initialPosition;
    }
    [MenuItem("Guidance/Reset All Data")]
    private static void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}