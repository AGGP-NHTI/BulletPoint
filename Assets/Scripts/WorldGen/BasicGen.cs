using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ListUtils;

public class BasicGen : MonoBehaviour
{
	public NavMeshSurface navMeshSurface;
	[Range(3,64)]
	public int mapSize = 16;
	public TileCollection tileset;

	public StaticTile[] staticTiles;

	int?[,] map;

	void Start()
	{
		map = new int?[mapSize, mapSize];

		// build perimeter
		for (short y = 0; y < mapSize; y++)
		{
			for (short x = 0; x < mapSize; x++)
			{
				if (x == 0 || y == 0 || x == mapSize - 1 || y == mapSize - 1)
				{
					map[x, y] = tileset.Length - 1;
				}
			}
		}

		foreach (StaticTile tile in staticTiles)
		{
			map[(int)Mathf.Repeat(tile.x, mapSize), (int)Mathf.Repeat(tile.y, mapSize)] = tile.tileIndex;
		}
		
		// select tiles
		while (Resolve()) { }

		// spawn prefabs
		for (short y = 0; y < mapSize; y++)
		{
			for (short x = 0; x < mapSize; x++)
			{
				SpawnTile(x, y);
			}
		}

		// build navmesh
		navMeshSurface.BuildNavMesh();
	}
	
	bool Resolve()
	{
		List<(short x, short y)> options = new List<(short x, short y)>();
		
		for (short y = 0; y < mapSize; y++)
		{
			for (short x = 0; x < mapSize; x++)
			{
				if (!map[x,y].HasValue)
				{
					options.Add((x, y));
				}
			}
		}

		if (options.Count > 0)
		{
			(short x, short y) selectedPosition = options.RandomElement();
			map[selectedPosition.x, selectedPosition.y] = tileset.RandomIndex(true);

			return true;
		}
		else
		{
			return false;
		}

	}

	public void SpawnTile(short x, short y)
	{
		var t = Instantiate(tileset[map[x, y].Value].prefab, new Vector3(x * tileset.tileSize, 0, y * tileset.tileSize), Quaternion.identity, transform);
	}

	public void SpawnTile((short x, short y) pos)
	{
		SpawnTile(pos.x, pos.y);
	}

	public (short x, short y)? MapCoordinate(Vector3 worldPosition)
	{
		(short x, short y) pos;
		pos = ((short)(worldPosition.x / tileset.tileSize), (short)(worldPosition.z / tileset.tileSize));
		if (IsWithinBounds(pos))
		{
			return pos;
		}
		else
		{
			return null;
		}
	}

	public bool IsWithinBounds((short x, short y) mapCoordinate)
	{
		return (mapCoordinate.x >= 0 && mapCoordinate.x < mapSize && mapCoordinate.y >= 0 && mapCoordinate.y < mapSize);
	}
	
	[System.Serializable]
	public struct StaticTile
	{
		public int tileIndex;
		public short x;
		public short y;
	}
}
