using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;

public class MapSaver : Editor
{
    [MenuItem("Guidance/Save Map")]
    public static void TrySaveStage()
    {
        SaveMapWindow window = EditorWindow.GetWindow<SaveMapWindow>();
        window.Show();
    }


    public static void SaveStage(int stageNo, string savePath)
    {
        StageData data = new StageData();
        data.stageNo = stageNo;
        SavePointData(data);
        SaveMapData(data);
        SaveObjectData(data);

        string jsonData = data.ToJson();
        System.IO.File.WriteAllText(savePath, jsonData);
        Debug.Log("성공적으로 수행됨!");
    }

    private static void SavePointData(StageData data)
    {
        // start Point
        GameObject student = GameObject.Find("player");
        if(student == null)
        {
            Debug.LogError("학생 오브젝트를 찾을 수 없음!");
            return;
        }
        data.startPoint = student.transform.position;

        // start Point
        GameObject goal = GameObject.Find("goal");
        if(goal == null)
        {
            Debug.LogError("골 오브젝트를 찾을 수 없음!");
            return;
        }
        data.endPoint = student.transform.position;
    }
    private static void SaveMapData(StageData data)
    {
        GameObject grid = GameObject.Find("Grid");
        if(grid == null)
        {
            Debug.LogError("맵 그리드 오브젝트를 찾을 수 없음!");
            return;
        }

        Tilemap platform = grid.transform.GetChild(0).GetComponent<Tilemap>();
        Tilemap trap = grid.transform.GetChild(1).GetComponent<Tilemap>();
        Tilemap banArea = grid.transform.GetChild(2).GetComponent<Tilemap>();
        Tilemap props = grid.transform.GetChild(3).GetComponent<Tilemap>();

        Vector3Int size = platform.cellBounds.size;
        Vector3Int offset = platform.cellBounds.position;

        // 기본 타일정보 저장
        data.width = size.x;
        data.height = size.y;
        data.offset = new Vector2Int(offset.x, offset.y);
        data.tiles = new int[size.x, size.y];

        // 플랫폼, 트랩, 밴에리어 저장
        bool wroteTileType = false;
        for (int x=0; x<size.x; x++)
        {
            for (int y=0; y<size.y; y++)
            {
                int mask = 0;
                Vector3Int tilePos = new Vector3Int(x, y, 0) + offset;
                if(platform.HasTile(tilePos))
                {
                    mask |= TileMask.platform;
                    if(!wroteTileType)
                    {
                        TileBase platformTile = platform.GetTile(tilePos);
                        data.platformTileType = platformTile.name;
                        wroteTileType = true;
                    }
                }
                if(trap.HasTile(tilePos)) mask |= TileMask.trap;
                if(banArea.HasTile(tilePos)) mask |= TileMask.banned;
                data.tiles[x,y] = mask;
            }
        }

        SavePropsData(props, data);
    }
    private static void SavePropsData(Tilemap props, StageData data)
    {
        // 프롭스 저장
        BoundsInt bounds = props.cellBounds;
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = props.GetTile(tilePosition);
                if (tile != null)
                {
                    MapTileData tileData = new MapTileData();
                    tileData.position = tilePosition;
                    tileData.tileType = tile.name;
                    data.propTiles.Add(tileData);
                }
            }
        }
    }
    private static void SaveObjectData(StageData data)
    {
        ISaveableObject[] saveableObjects = FindObjectsOfType<Component>()
            .Where(c => c is ISaveableObject)
            .Select(c => (ISaveableObject)c)
            .ToArray();
        foreach(ISaveableObject obj in saveableObjects)
        {
            data.objectData.Add(obj.ToObjectData());
        }
    }
}

public class SaveMapWindow : EditorWindow
{
    public int stageNo=1;
    public string fileName="Assets/Resources/test.json";

    private void OnGUI()
    {
        stageNo = EditorGUILayout.IntField("Stage No.", stageNo);
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Save"))
        {
            if(string.IsNullOrWhiteSpace(fileName))
            {
                Debug.LogError("File Name은 공백이 될 수 없습니다!");
            }
            else
            {
                MapSaver.SaveStage(stageNo, fileName);
                Close();
            }
        }
    }
}