using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ListUtils;

public class MapGen : MonoBehaviour
{
	public int mapSize = 16;
	public Renderer ren;
	public Sprite[] sprTiles;
	// ======================

	// stores adjacency rules for tiles
	List<TileData> tileData;
	List<int> tileWeight;

	// stores a list of tile indices at each position in the chunk
	List<int>[,] map;

	// stores whether a position in the chunk has been 
	bool[,] visited;

	void Start()
	{
		// setup tile data
		tileData = new List<TileData>();

		foreach (Sprite spr in sprTiles)
		{
			tileData.Add(new TileData(
				spr.texture.GetPixel((int)spr.rect.x + 1, (int)spr.rect.y + 3),
				spr.texture.GetPixel((int)spr.rect.x + 2, (int)spr.rect.y + 3),
				spr.texture.GetPixel((int)spr.rect.x, (int)spr.rect.y + 1),
				spr.texture.GetPixel((int)spr.rect.x, (int)spr.rect.y + 2)
				));
		}

		// setup tile weights
		tileWeight = new List<int>();

		foreach (Sprite spr in sprTiles)
		{
			tileWeight.Add(
				(int)(spr.texture.GetPixel((int)spr.rect.x, (int)spr.rect.y + 3).r * 255)
				);
		}

		// populate chunk
		map = new List<int>[mapSize, mapSize];

		for (int y = 0; y < mapSize; y++)
		{
			for (int x = 0; x < mapSize; x++)
			{
				map[x, y] = new List<int>();
				for (int i = 0; i < sprTiles.Length; i++)
				{
					map[x, y].Add(i);
				}
			}
		}

		visited = new bool[mapSize, mapSize];

		(short x, short y) randPos = ((short)(mapSize / 2), (short)(mapSize / 2));

		map[randPos.x, randPos.y].Clear();
		map[randPos.x, randPos.y].Add(tileData.RandomIndex(tileWeight));

		Collapse(randPos.x, randPos.y);
		StartCoroutine(DoResolve());

		ren.material.mainTexture = Render();
	}

	IEnumerator DoResolve()
	{
		float t = 0;
		while (Resolve())
		{
			t += Time.deltaTime;
			ren.material.mainTexture = Render();
			if (t > .05f)
			{
				t = 0;
				yield return null;
			}
			
		}
	}

	// eliminate conflicting possibilities around x,y, and propagate
	void Collapse(short init_x, short init_y)
	{
		Stack<(short x, short y)> callstack = new Stack<(short x, short y)>();

		callstack.Push((init_x, init_y));

		(short x, short y) pair;
		short entropy;

		while (callstack.Count > 0)
		{
			pair = callstack.Pop();
			visited[pair.x, pair.y] = true;
			entropy = (short)map[pair.x, pair.y].Count;


			List<Color> matches = new List<Color>();

			// left
			if (pair.x > 0)
			{
				foreach (int tileIndex in map[pair.x, pair.y])
				{
					Color c = tileData[tileIndex].left;

					if (!matches.Contains(c))
					{
						matches.Add(c);
					}
				}

				map[pair.x - 1, pair.y].RemoveAll(i => !matches.Contains(tileData[i].right));
			}

			matches.Clear();

			// right
			if (pair.x < mapSize - 1)
			{
				foreach (int tileIndex in map[pair.x, pair.y])
				{
					Color c = tileData[tileIndex].right;
					if (!matches.Contains(c))
					{
						matches.Add(c);
					}
				}

				map[pair.x + 1, pair.y].RemoveAll(i => !matches.Contains(tileData[i].left));
			}

			matches.Clear();

			// down
			if (pair.y > 0)
			{
				foreach (int tileIndex in map[pair.x, pair.y])
				{
					Color c = tileData[tileIndex].down;
					if (!matches.Contains(c))
					{
						matches.Add(c);
					}
				}

				map[pair.x, pair.y - 1].RemoveAll(i => !matches.Contains(tileData[i].up));
			}

			matches.Clear();

			// up
			if (pair.y < mapSize - 1)
			{
				foreach (int tileIndex in map[pair.x, pair.y])
				{
					Color c = tileData[tileIndex].up;
					if (!matches.Contains(c))
					{
						matches.Add(c);
					}
				}

				map[pair.x, pair.y + 1].RemoveAll(i => !matches.Contains(tileData[i].down));
			}

			if (entropy != (short)map[pair.x, pair.y].Count)
			{
				// propagate
				if (pair.x > 0) { if (!visited[pair.x - 1, pair.y]) { callstack.Push(((short)(pair.x - 1), pair.y)); } }

				if (pair.x < mapSize - 1) { if (!visited[pair.x + 1, pair.y]) { callstack.Push(((short)(pair.x + 1), pair.y)); } }

				if (pair.y > 0) { if (!visited[pair.x, pair.y - 1]) { callstack.Push((pair.x, (short)(pair.y - 1))); } }

				if (pair.y < mapSize - 1) { if (!visited[pair.x, pair.y + 1]) { callstack.Push((pair.x, (short)(pair.y + 1))); } }
			}
			
		}
	}

	// randomly resolve one of/the lowest entropy position
	bool Resolve()
	{
		int? lowEntropy = null;
		List<(short x, short y)> lowEntropyPositions = new List<(short x, short y)>();

		// identify lowest entropy
		for (int y = 0; y < mapSize; y++)
		{
			for (int x = 0; x < mapSize; x++)
			{
				if (lowEntropy == null)
				{
					if (map[x, y].Count > 1)
					{
						lowEntropy = map[x, y].Count;
					}
				}
				else
				{
					if (map[x, y].Count > 1)
					{
						if (map[x, y].Count < lowEntropy.Value)
						{
							lowEntropy = map[x, y].Count;
						}
					}
				}
			}
		}

		if (lowEntropy.HasValue)
		{
			// add positions with lowest entropy to list
			for (short y = 0; y < mapSize; y++)
			{
				for (short x = 0; x < mapSize; x++)
				{
					if (map[x, y].Count == lowEntropy.Value)
					{
						lowEntropyPositions.Add((x, y));
					}
				}
			}

			// resolve
			(short x, short y) selectedPosition = lowEntropyPositions.RandomElement();

			int resolution = map[selectedPosition.x, selectedPosition.y].RandomElement(tileWeight);

			map[selectedPosition.x, selectedPosition.y].Clear();
			map[selectedPosition.x, selectedPosition.y].Add(resolution);

			// collapse from here
			Collapse(selectedPosition.x, selectedPosition.y);

			return true;
		}
		else
		{
			return false;
		}

	}


	Texture2D Render()
	{
		Texture2D tx = new Texture2D(mapSize * 3, mapSize * 3, TextureFormat.RGB24, false, false);
		tx.filterMode = FilterMode.Point;

		for (int y = 0; y < mapSize; y++)
		{
			for (int x = 0; x < mapSize; x++)
			{

				if (map[x, y].Count == 1)
				{
					Sprite spr = sprTiles[map[x, y][0]];
					
					Graphics.CopyTexture(
						spr.texture, 0, 0,
						(int)spr.rect.x + 1,
						(int)spr.rect.y,
						3, 3,
						tx, 0, 0,
						x * 3, y * 3
						);
				}
				else
				{
					tx.SetPixel(x * 3 + 1, y * 3 + 1, Color.HSVToRGB(.333f, 1, map[x, y].Count / (float)sprTiles.Length));
				}
			}
		}

		tx.Apply();

		return tx;
	}




	struct TileData
	{
		public Color up;
		public Color right;
		public Color down;
		public Color left;

		public TileData(Color up, Color right, Color down, Color left)
		{
			this.up = up;
			this.right = right;
			this.down = down;
			this.left = left;
		}
	}
}
