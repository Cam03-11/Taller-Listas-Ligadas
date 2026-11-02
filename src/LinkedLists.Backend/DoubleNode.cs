namespace LinkedLists.Backend;

public class DoubleNode<T>
{
    public DoubleNode(T data)
    {
        Data = data;
    }

    public T Data { get; set; }

    public DoubleNode<T>? Next { get; set; }

    public DoubleNode<T>? Prev { get; set; }
}
