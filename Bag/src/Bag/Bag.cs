namespace Bag;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Bag<T> : IEnumerable<KeyValuePair<T, uint>> where T : notnull
{
    private readonly Dictionary<T, uint> _bag = new();
    private uint _count;


    public Bag() { }

    public Bag(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    /// <summary>
    /// Add an item to the Bag. This sets its count to 1.
    /// <example>
    /// Example:
    /// <code>
    /// Bag bag = new<string>();
    /// bag.Add("one");
    /// Console.WriteLine(bag["one"] == 1);
    /// </code>
    /// Prints `true`
    /// </example>
    /// <exception cref="ArgumentNullException"> Thrown when argument is null </exception>
    /// </summary>
    public void Add(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "cannot be null");
        }

        if (_bag.TryGetValue(item, out var value))
        {
            _bag[item] = value + 1;
        }
        else
        {
            _bag.Add(item, 1);
        }
        _count++;
    }

    /// <summary>
    /// Gets an items count from the bag
    /// <example>
    /// Example:
    /// <code>
    /// Bag bag = new<string>();
    /// bag.Add("one");
    /// Console.WriteLine(bag["one"]);
    /// </code>
    /// Prints `1`
    /// </example>
    /// <exception cref="KeyNotFoundException"> Thrown when argument is not in bag </exception>
    /// </summary>
    public uint this[T key]
    {
        get
        {
            return Get(key);
        }
    }

    /// <summary>
    /// Gets an items count from the bag
    /// <example>
    /// Example:
    /// <code>
    /// Bag bag = new<string>();
    /// bag.Add("one");
    /// Console.WriteLine(bag["one"]);
    /// </code>
    /// Prints `1`
    /// </example>
    /// <exception cref="KeyNotFoundException"> Thrown when argument is not in bag </exception>
    /// </summary>
    public uint Get(T item)
    {
        if (!Contains(item))
        {
            throw new KeyNotFoundException($"{nameof(item)} not found");
        }
        return _bag[item];
    }

    /// <summary>
    /// Gets an items count from the bag or 0 when item is not found
    /// <example>
    /// Example:
    /// <code>
    /// Bag bag = new<string>();
    /// bag.Add("one");
    /// Console.WriteLine(bag["two"]);
    /// </code>
    /// Prints `0`
    /// </example>
    /// </summary>
    public uint GetOrDefault(T item)
    {
        if (!Contains(item))
        {
            return default;
        }
        return _bag[item];
    }

    /// <summary>
    /// Clears the bag of all items;
    /// </summary>
    public void Clear()
    {
        _bag.Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    /// <summary>
    /// <see cref="Remove(T, uint)"/>
    /// </summary>
    public bool Remove(T item) => Remove(item, 1);

    /// <summary>
    /// Removes an item <code>num</code> times from the bag to a minimum of 0.
    /// <example>
    /// Example:
    /// <code>
    /// Bag bag = new<string>();
    /// bag.Add("one");
    /// bag.Add("one");
    /// bag.Remove("one", 3);
    /// Console.WriteLine(bag["one"]);
    /// </code>
    /// Prints `0`
    /// </example>
    /// </summary>
    public bool Remove(T item, uint num)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "cannot be null");
        }

        if (_bag.TryGetValue(item, out var value))
        {
            if (value >= num)
            {
                _bag[item] = value - num;
                _count -= num;
            }
            else
            {
                _bag[item] = 0;
                _count = _count - value;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes all items whoose count is zero
    /// </summary>
    public void Purge()
    {
        foreach (var key in _bag.Keys)
        {
            if (_bag[key] == 0)
            {
                _bag.Remove(key);
            }
        }
    }

    public void IntersectWith(IEnumerable<T> other)
    {
        IntersectWith(other.Select(item => new KeyValuePair<T, uint>(item, 1)));
    }

    public void IntersectWith(IEnumerable<KeyValuePair<T, uint>> other)
    {
        var s = new HashSet<T>(other.Count());
        foreach (var item in other)
        {
            if (!Contains(item.Key))
            {
                _bag[item.Key] = 1;
            }
            else
            {
                _bag[item.Key] += item.Value;
            }
            s.Add(item.Key);
        }

        foreach (var key in _bag.Keys)
        {
            if (!s.Contains(key))
            {
                Remove(key);
            }
        }
    }

    public bool Overlaps(IEnumerable<T> other)
    {
        foreach (var item in other)
        {
            if (Contains(item))
                return true;
        }
        return false;
    }

    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        foreach (var key in _bag.Keys)
        {
            if (!other.Contains(key))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsProperSubsetOf(IEnumerable<KeyValuePair<T, uint>> other)
    {
        return IsProperSubsetOf(other.Select(item => item.Key));
    }

    public bool IsProperSupersetOf(IEnumerable<T> other)
    {
        foreach (var item in other)
        {
            if (!Contains(item))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsProperSupersetOf(IEnumerable<KeyValuePair<T, uint>> other)
    {
        return IsProperSupersetOf(other.Select(item => item.Key));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item)
    {
        return _bag.ContainsKey(item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Delete(T item)
    {
        return _bag.Remove(item);
    }

    public void ExceptWith(IEnumerable<T> other)
    {
        foreach (var item in other)
        {
            Remove(item);
        }
    }

    public void ExceptWith(IEnumerable<KeyValuePair<T, uint>> other)
    {
        foreach (var item in other)
        {
            Remove(item.Key, item.Value);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint TotalCount() => _count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ItemCount() => _bag.Keys.Count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerator<KeyValuePair<T, uint>> GetEnumerator() => _bag.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
