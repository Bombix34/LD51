using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListOptim<T>
{
    public T[] Values;
    public int Count = 0;

    public ListOptim(int size)
    {
        Values = new T[size];
    }

    public void Add(T value)
    {
        Values[Count++] = value;
    }

    public void RemoveAt(int i)
    {
        if (i < Count)
        {
            ListOptim<T> newList = new ListOptim<T>(200);
            for(int j = 0; j < Count; ++j)
            {
                if( i != j )
                {
                    newList.Add(Values[j]);
                }
            }
            Count--;
            Values = newList.Values;
        }
    }

    public void Remove(T toRemove)
    {
        for(int i = 0; i < Count; ++i)
        {
            if(Values[i].Equals(toRemove))
            {
                RemoveAt(i);
                return;
            }
        }
    }

    public bool Contains(T element)
    {
        for(int i = 0; i < Count; ++i)
        {
            if(Values[i].Equals(element))
                return true;
        }
        return false;
    }

    public T this[int i]
    {
        get => Values[i];
        set => Values[i] = value;
    }

    public void Clear()
    {
        Count = 0;
    }

    public override string ToString()
    {
        string toReturn ="";
        for(int i = 0; i < Count; ++i)
        {
            toReturn += Values[i].ToString() +"\n";
        }
        return toReturn;
    }
}
