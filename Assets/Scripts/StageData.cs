using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class StageData
{
	public int stageNo;
	public SerializableVector3 startPoint;
	public SerializableVector3 endPoint;

	// tile data
	public string platformTileType;
	public int width;
	public int height;
	public SerializableVector2Int offset;
	public List<MapTileData> propTiles = new List<MapTileData>();

	[NonSerialized]
	public int[,] tiles;
	[SerializeField]
	public List<List<int>> tilesSerialized;

	// object data
	[NonSerialized]
	public List<MapObjectData> objectData = new List<MapObjectData>();
	[SerializeField]
	public List<string> objectDataSerialized;

	public string ToJson()
	{
		objectDataSerialized = objectData.Select( obj => JsonUtility.ToJson(obj) ).ToList();
		tilesSerialized = Utils.Convert2dimArrayToList<int>(tiles);
		Debug.Log(tilesSerialized[0][3]);
		return JsonConvert.SerializeObject(this, Formatting.Indented);
	}
	static public StageData FromJson(string json)
	{
		StageData data = JsonConvert.DeserializeObject<StageData>(json);
		data.tiles = Utils.Convert2dimListToArray<int>(data.tilesSerialized);
		data.objectData = data.objectDataSerialized
			.Select( str => MapObjectData.FromJson<MapObjectData>(str) )
			.ToList();
		return data;
	}
}

[Serializable]
public class MapTileData
{
	public Vector3Int position;
	public string tileType;
}

public abstract class PolymorphicObject
{
	public string type;

	public static T FromJson<T>(string json) where T : PolymorphicObject
	{
		var type = PolymorphicObject.GetType(JsonUtility.FromJson<T>(json).type);
		return (T)JsonUtility.FromJson(json, type);
	}

	private static Type GetType(string type)
	{
		switch(type)
		{
			case DistractorData.typeString: return typeof(DistractorData);
			default: return typeof(MapObjectData);
		}
	}
}

[Serializable]
public class MapObjectData : PolymorphicObject
{
	public string uuid = System.Guid.NewGuid().ToString();
	public Vector3 position;
	protected MapObjectData(string typeString)
	{
		type = typeString;
	}
}

[Serializable]
public class DistractorData : MapObjectData
{
	public const string typeString = "distractor";
	public DistractorData() : base(typeString){}
}

[Serializable]
public class SerializableVector3
{
	public float x;
	public float y;
	public float z;
	public SerializableVector3(Vector3 vector)
	{
		x = vector.x;
		y = vector.y;
		z = vector.z;
	}
	public static implicit operator Vector3(SerializableVector3 serializableVector)
	{
		return new Vector3(serializableVector.x, serializableVector.y, serializableVector.z);
	}
	public static implicit operator Vector2(SerializableVector3 serializableVector)
	{
		return new Vector2(serializableVector.x, serializableVector.y);
	}
	public static implicit operator SerializableVector3(Vector3 vector)
	{
		return new SerializableVector3(vector);
	}
}

[Serializable]
public class SerializableVector2Int
{
	public int x;
	public int y;
	public SerializableVector2Int(Vector2Int vector)
	{
		x = vector.x;
		y = vector.y;
	}
	public static implicit operator Vector2Int(SerializableVector2Int vector)
	{
		return new Vector2Int(vector.x, vector.y);
	}
	public static implicit operator SerializableVector2Int(Vector2Int vector)
	{
		return new SerializableVector2Int(vector);
	}
}