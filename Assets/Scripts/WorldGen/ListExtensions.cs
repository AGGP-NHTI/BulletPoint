using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ListUtils
{
	public static class ListExtensions
	{
		public static int RandomIndex<T>(this List<T> i)
		{
			return Random.Range(0, i.Count);
		}

		public static int RandomIndex<T>(this List<T> i, List<int> weights)
		{
			int p = weights.Sum();
			float r = Random.value * p;
			int index = 0;
			int rollingSum = 0;
			while (index < i.Count && r > rollingSum)
			{
				rollingSum += weights[index];
				index++;
			}

			return index - 1;
		}

		public static int RandomIndex<T>(this List<T> i, int[] weights)
		{
			int p = weights.Sum();
			float r = Random.value * p;
			int index = 0;
			int rollingSum = 0;
			while (index < i.Count && r > rollingSum)
			{
				rollingSum += weights[index];
				index++;
			}

			return index - 1;
		}


		public static T RandomElement<T>(this List<T> i)
		{
			return i[i.RandomIndex()];
		}

		public static T RandomElement<T>(this List<T> i, List<int> weights)
		{
			return i[i.RandomIndex(weights)];
		}

		public static T RandomElement<T>(this List<T> i, int[] weights)
		{
			return i[i.RandomIndex(weights)];
		}


		public static int Sum(this List<int> i)
		{
			int sum = 0;
			foreach(int n in i)
			{
				sum += n;
			}
			return sum;
		}

		public static int Sum(this int[] i)
		{
			int sum = 0;
			foreach (int n in i)
			{
				sum += n;
			}
			return sum;
		}
	}
}
