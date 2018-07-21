using System.Collections;
using System.Collections.Generic;

public class CircularBuffer<T> : IEnumerable
{
    public int Count { get { return _queue.Count; } }
    private Queue<T> _queue;
    private int _size;

    public CircularBuffer(int size)
    {
        _queue = new Queue<T>(size);
        _size = size;
    }

    public void Add(T obj)
    {
        if (_queue.Count == _size)
        {
            _queue.Dequeue();
            _queue.Enqueue(obj);
        }
        else
            _queue.Enqueue(obj);
    }
    public T Read()
    {
        return _queue.Dequeue();
    }

    public T Peek()
    {
        return _queue.Peek();
    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)_queue).GetEnumerator();
    }
}