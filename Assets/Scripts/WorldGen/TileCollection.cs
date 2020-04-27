using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollection : DataObject
{
	[Space(32)]
	[Header("The last tile should be a void tile.")]
	[Header("The first tile should be fully open.")]
	
	
	public float tileSize;
	[SerializeField]
	private TileData[] tiles;
	private int[] weights = null;

	public int Length
	{
		get { return tiles.Length; }
	}

	public int[] Weights
	{
		get
		{
			if (weights == null || weights.Length == 0)
			{
				weights = new int[Length];
				for (int i = 0; i < Length; i++)
				{
					weights[i] = tiles[i].weight;
				}
			}
			return weights;
		}
	}

	public int RandomIndex(bool useWeight = true)
	{
		if (useWeight)
		{
			int p = 0;
			foreach(TileData t in tiles)
			{
				p += t.weight;
			}

			float r = Random.value * p;
			int index = 0;
			int rollingSum = 0;
			while (index < Length && r > rollingSum)
			{
				rollingSum += tiles[index].weight;
				index++;
			}

			return index - 1;
		}
		else
		{
			return Random.Range(0, Length);
		}
	}
	
	public TileData RandomElement(bool useWeight = true)
	{
		return tiles[RandomIndex(useWeight)];
	}


	public TileData this[int index]
	{
		get { return tiles[index]; }
	}

	public IEnumerator GetEnumerator()
	{
		return (IEnumerator)this;
	}
}
