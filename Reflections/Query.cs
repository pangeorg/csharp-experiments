using System.Collections;

public class Query<T> where T : class
{
    private IEnumerable<T> _items = [];

    public Query(IEnumerable<object> items)
    {
        _items = items.OfType<T>();
    }

    public IEnumerable<T> Iter()
    {
        return _items.OfType<T>();
    }

}
