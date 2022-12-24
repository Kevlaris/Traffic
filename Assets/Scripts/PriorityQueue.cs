using System;
using System.Collections.Generic;

//ChatGPT
public class PriorityQueue<T>
{
	private List<Tuple<T, float>> items;

	public int Count
	{
		get { return items.Count; }
	}

	public PriorityQueue()
	{
		items = new List<Tuple<T, float>>();
	}

	public void Enqueue(T item, float priority)
	{
		items.Add(new Tuple<T, float>(item, priority));
		int index = items.Count - 1;
		while (index > 0)
		{
			int parentIndex = (index - 1) / 2;
			if (items[parentIndex].Item2 <= items[index].Item2)
			{
				break;
			}
			Tuple<T, float> tmp = items[parentIndex];
			items[parentIndex] = items[index];
			items[index] = tmp;
			index = parentIndex;
		}
	}

	public T Dequeue()
	{
		int lastIndex = items.Count - 1;
		T frontItem = items[0].Item1;
		items[0] = items[lastIndex];
		items.RemoveAt(lastIndex);

		--lastIndex;
		int index = 0;
		while (index < lastIndex)
		{
			int childIndex = index * 2 + 1;
			if (childIndex > lastIndex)
			{
				break;
			}
			int rightChild = childIndex + 1;
			if (rightChild <= lastIndex && items[rightChild].Item2 < items[childIndex].Item2)
			{
				childIndex = rightChild;
			}
			if (items[index].Item2 <= items[childIndex].Item2)
			{
				break;
			}
			Tuple<T, float> tmp = items[index];
			items[index] = items[childIndex];
			items[childIndex] = tmp;
			index = childIndex;
		}
		return frontItem;
	}
}
