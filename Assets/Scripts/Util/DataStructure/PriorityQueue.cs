using System.Collections.Generic;


[System.Serializable]
public class PriorityQueue<TElement, TPriority>
{
    public int Count { protected set; get; }
    public int Capacity { protected set; get; } = 10;
    protected (TElement element, TPriority priority)[] values;
    protected IComparer<TPriority> comparer;

    public PriorityQueue(IComparer<TPriority>? comparer = null)
    {
        values = new (TElement, TPriority)[Capacity];

        if (comparer != null)
            this.comparer = comparer;
        else
            this.comparer = Comparer<TPriority>.Default;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        if (Count == Capacity)
            ReCapacity();

        values[Count++] = (element, priority);

        SortEnqueue(Count - 1);
    }

    private void SortEnqueue(int index)
    {
        int parentsIndex = (index - 1) / 2;
        if (parentsIndex < 0 || parentsIndex == index)
            return;

        if (comparer.Compare(values[index].priority, values[parentsIndex].priority) < 0)
        {
            (values[index], values[parentsIndex]) = (values[parentsIndex], values[index]);
            SortEnqueue(parentsIndex);
        }
    }

    private void SortDequeue(int index)
    {
        int childLeftIndex = index * 2 + 1;
        if (childLeftIndex >= Count)
            return;

        int childRightIndex = childLeftIndex + 1;
        if (childRightIndex < Count && comparer.Compare(values[index].priority, values[childRightIndex].priority) > 0)
        {
            (values[index], values[childRightIndex]) = (values[childRightIndex], values[index]);
            SortDequeue(childRightIndex);
        }
        if (comparer.Compare(values[index].priority, values[childLeftIndex].priority) > 0)
        {
            (values[index], values[childLeftIndex]) = (values[childLeftIndex], values[index]);
            SortDequeue(childLeftIndex);
        }
    }

    public TElement Dequeue()
    {
        var returnValue = values[0];
        --Count;

        values[0] = values[Count];
        values[Count] = default;
        SortDequeue(0);

        return returnValue.element;
    }

    public TElement Peek()
    {
        return values[0].element;
    }

    public bool TryDequeue(out TElement element, out TPriority priority)
    {
        if (Count == 0)
        {
            element = default;
            priority = default;
            return false;
        }

        priority = values[0].priority;
        element = Dequeue();

        return true;
    }

    public bool TryPeek(out TElement element, out TPriority priority)
    {
        if (Count == 0)
        {
            element = default;
            priority = default;
            return false;
        }

        element = values[0].element;
        priority = values[0].priority;

        return true;
    }

    private void ReCapacity()
    {
        Capacity *= 2;
        System.Array.Resize(ref values, Capacity);
    }

    public void Clear()
    {
        for (int i = 0; i < Count; ++i)
        {
            values[i] = default;
        }

        Count = 0;
    }
}