using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static void DrawBound(Bounds bounds, Color color)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        Vector3 minAlt = new Vector3(bounds.min.x, bounds.max.y, 0f);
        Vector3 maxAlt = new Vector3(bounds.max.x, bounds.min.y, 0f);
        Debug.DrawLine(min, minAlt, color, 100f);
        Debug.DrawLine(max, minAlt, color, 100f);
        Debug.DrawLine(max, maxAlt, color, 100f);
        Debug.DrawLine(min, maxAlt, color, 100f);
    }
    public static bool IsZero(float value) 
    {
        const float epsilon = 0.001f;
        return value < epsilon && value > -epsilon;
    }
    public static float Remain(float value, float cycle)
    {
        float result = value % cycle;
        if(result > 0) return result;
        return cycle + result;
    }
    public static Vector2Int GetCellPosition(Vector3 pos)
    {
        return new Vector2Int( (int)Mathf.Floor(pos.x), (int)Mathf.Floor(pos.y) );
    }
    public static Vector2 GetCellObjectPosition(Vector2 pos)
    {
        return new Vector2( Mathf.Floor(pos.x) + 0.5f, Mathf.Floor(pos.y) + 0.5f );
    }
    public static Vector3 GetCellObjectPosition(Vector3 pos)
    {
        return new Vector3( Mathf.Floor(pos.x) + 0.5f, Mathf.Floor(pos.y) + 0.5f, 0f );
    }
    public static List<List<T>> Convert2dimArrayToList<T>(T[,] array)
    {
        List<List<T>> list = new List<List<T>>();
        for(int i=0; i<array.GetLength(0); i++)
        {
            List<T> innerList = new List<T>();
            for(int j=0; j<array.GetLength(1); j++)
            {
                innerList.Add(array[i,j]);
            }
            list.Add(innerList);
        }
        return list;
    }
    public static T[,] Convert2dimListToArray<T>(List<List<T>> list)
    {
        int width = list.Count;
        int height = list[0].Count;
        T[,] array = new T[width, height];
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                array[i,j] = list[i][j];
            }
        }
        return array;
    }
    public static List<T> FindAll<T>()
    {
        List<T> result = new List<T>();
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(var rootGameObject in rootGameObjects)
        {
            T[] children = rootGameObject.GetComponentsInChildren<T>();
            result.AddRange(children);
        }
        return result;
    }
    public static bool IsSpecialCharacter(char c)
    {
        const string specialCharacters = " \n\t`~!@#$%^&*()_+-=[]{};:'\",./<>?";
        return specialCharacters.IndexOf(c) >= 0;
    }
}
