using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
	private Tilemap platform;
	private Tilemap trap;
	private Tilemap banArea;

	private Vector3Int _offset;
	private int[,] tileMask;

	public GuidancePlatform.Graph graph {get; private set;}
	public GuidancePlatform.TrailMaker pathfinder {get; private set;}
	public Vector2Int offset
	{
		get
		{
			return (Vector2Int)this._offset;
		}
	}

	private Tilemap GetChildTilemap(int index)
	{
		if (transform.childCount <= index) return null;
		Transform child = transform.GetChild(index);
		if (child == null) return null;
		return child.gameObject.GetComponent<Tilemap>();
	}

	private void MakeMask()
	{
		Vector3Int size = platform.cellBounds.size;

		_offset = platform.cellBounds.position;
		tileMask = new int[size.x, size.y];

		for (int x=0; x<size.x; x++)
		{
			for (int y=0; y<size.y; y++)
			{
				int mask = 0;
				Vector3Int tilePos = new Vector3Int(x, y, 0) + _offset;
				if(platform.HasTile(tilePos)) mask |= TileMask.platform;
				if(trap.HasTile(tilePos)) mask |= TileMask.trap;
				if(banArea.HasTile(tilePos)) mask |= TileMask.banned;
				tileMask[x,y] = mask;
			}
		}
	} 

	// Start is called before the first frame update
	void Awake()
	{
		platform = GetChildTilemap(0);
		trap = GetChildTilemap(1);
		banArea = GetChildTilemap(2);
	}

	void Start()
	{
		InitLevel();
	}

	void InitLevel()
	{
		MakeMask();
		IBarrier[] barriers = FindAllBarriers();
		IMovablePlatform[] movingPlatforms = FindAllMovingPlatforms();
		//graph = GuidancePlatform.GraphMaker.Make(tileMask, barriers, movingPlatforms);
		graph = GuidancePlatform.GraphMaker.Make(tileMask);
		pathfinder = new GuidancePlatform.TrailMaker(tileMask, graph, offset);
		//graph.Debug(_offset);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public Vector2Int GetCellIndex(Vector3 worldPosition)
	{
		Vector3Int cellPos = platform.WorldToCell(worldPosition);
		Vector3Int cellIndex = cellPos - _offset;
		return new Vector2Int(cellIndex.x, cellIndex.y);
	}
	public int GetTileMask(Vector3 worldPosition)
	{
		Vector3Int cellPos = platform.WorldToCell(worldPosition);
		Vector3Int cellIndex = cellPos - _offset;
		if(!IsInbound(cellIndex)) return TileMask.banned;
		return tileMask[cellIndex.x, cellIndex.y];
	}
	public int GetTileMask(Vector2 worldPosition)
	{
		return GetTileMask((Vector3)worldPosition);
	}
	public bool IsInbound(Vector3Int cellPosition)
	{
		int width = tileMask.GetLength(0);
		int height = tileMask.GetLength(1);
		if(cellPosition.x < 0 || cellPosition.x >= width) return false;
		if(cellPosition.y < 0 || cellPosition.y >= height) return false;
		return true;
	}

	private IBarrier[] FindAllBarriers()
	{
		GameObject[] barrierObjects = GameObject.FindGameObjectsWithTag(Constants.barrierTag);
		return barrierObjects.Select( obj => obj.GetComponent<IBarrier>() )
			.Where( obj => obj != null )
			.ToArray();
	}

	private IMovablePlatform[] FindAllMovingPlatforms()
	{
		GameObject[] moverObjects = GameObject.FindGameObjectsWithTag(Constants.movingPlatformTag);
		return moverObjects.Select( obj => obj.GetComponent<IMovablePlatform>() )
			.Where( obj => obj != null )
			.ToArray();
	}

	[ContextMenu("Compress Bounds")]
	void CompressTilemapBounds()
	{
		Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();

		foreach (Tilemap tilemap in tilemaps)
		{
			tilemap.CompressBounds();
		}
	}
}
