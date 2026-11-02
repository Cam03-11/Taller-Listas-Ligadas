namespace LinkedLists.Backend;

public class DoublyLinkedList<T> where T : IComparable<T>
{
    private DoubleNode<T>? _head;
    private DoubleNode<T>? _tail;
    private bool _isDescending;

    public int Count { get; private set; }

    public bool IsEmpty => _head is null;

    public void Add(T data)
    {
        var newNode = new DoubleNode<T>(data);

        if (_head is null)
        {
            _head = newNode;
            _tail = newNode;
            Count++;
            return;
        }

        var current = _head;
        while (current is not null && !GoesBefore(data, current.Data))
        {
            current = current.Next;
        }

        if (current is null)
        {
            newNode.Prev = _tail;
            _tail!.Next = newNode;
            _tail = newNode;
        }
        else if (current.Prev is null)
        {
            newNode.Next = _head;
            _head.Prev = newNode;
            _head = newNode;
        }
        else
        {
            newNode.Prev = current.Prev;
            newNode.Next = current;
            current.Prev.Next = newNode;
            current.Prev = newNode;
        }

        Count++;
    }

    public IEnumerable<T> GetForward()
    {
        var current = _head;
        while (current is not null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public IEnumerable<T> GetBackward()
    {
        var current = _tail;
        while (current is not null)
        {
            yield return current.Data;
            current = current.Prev;
        }
    }

    public void SortDescending()
    {
        if (_isDescending)
        {
            return;
        }

        var current = _head;
        while (current is not null)
        {
            (current.Next, current.Prev) = (current.Prev, current.Next);
            current = current.Prev;
        }

        (_head, _tail) = (_tail, _head);
        _isDescending = true;
    }

    public List<(T Value, int Count)> GetOccurrences()
    {
        var occurrences = new List<(T Value, int Count)>();
        var current = _head;

        while (current is not null)
        {
            var value = current.Data;
            var count = 0;

            while (current is not null && current.Data.CompareTo(value) == 0)
            {
                count++;
                current = current.Next;
            }

            occurrences.Add((value, count));
        }

        return occurrences;
    }

    public List<T> GetModes()
    {
        var occurrences = GetOccurrences();
        if (occurrences.Count == 0)
        {
            return [];
        }

        var maxCount = occurrences.Max(o => o.Count);
        return occurrences
            .Where(o => o.Count == maxCount)
            .Select(o => o.Value)
            .ToList();
    }

    public bool Exists(T data) => Find(data, _head) is not null;

    public bool RemoveFirst(T data)
    {
        var node = Find(data, _head);
        if (node is null)
        {
            return false;
        }

        RemoveNode(node);
        return true;
    }

    public int RemoveAll(T data)
    {
        var removed = 0;
        var node = Find(data, _head);

        while (node is not null)
        {
            var next = node.Next;
            RemoveNode(node);
            removed++;
            node = Find(data, next);
        }

        return removed;
    }

    private bool GoesBefore(T data, T other)
    {
        var comparison = data.CompareTo(other);
        return _isDescending ? comparison > 0 : comparison < 0;
    }

    private static DoubleNode<T>? Find(T data, DoubleNode<T>? start)
    {
        var current = start;
        while (current is not null)
        {
            if (current.Data.CompareTo(data) == 0)
            {
                return current;
            }

            current = current.Next;
        }

        return null;
    }

    private void RemoveNode(DoubleNode<T> node)
    {
        if (node.Prev is null)
        {
            _head = node.Next;
        }
        else
        {
            node.Prev.Next = node.Next;
        }

        if (node.Next is null)
        {
            _tail = node.Prev;
        }
        else
        {
            node.Next.Prev = node.Prev;
        }

        node.Next = null;
        node.Prev = null;
        Count--;
    }
}
