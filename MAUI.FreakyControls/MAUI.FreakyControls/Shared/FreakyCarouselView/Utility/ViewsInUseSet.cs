using System.Collections.Specialized;

namespace Maui.FreakyControls.Utility;

public sealed class ViewsInUseSet
{
    public event NotifyCollectionChangedEventHandler CollectionChanged;

    private readonly Dictionary<View, int> _viewsSet = new();

    public IReadOnlyList<View> Views => _viewsSet.Keys?.ToList();

    public void AddRange(IEnumerable<View> views)
    {
        foreach (var view in views.Where(x => x != null))
        {
            Add(view);
        }
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, views));
    }

    public void RemoveRange(IEnumerable<View> views)
    {
        foreach (var view in views)
        {
            Remove(view);
        }
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, views));
    }

    public bool Contains(View view)
        => view != null && _viewsSet.ContainsKey(view);

    private void Add(View view)
        => _viewsSet[view] = Contains(view)
            ? _viewsSet[view] + 1
            : 1;

    private void Remove(View view)
    {
        if (!Contains(view))
        {
            return;
        }

        var currentCount = _viewsSet[view] - 1;
        if (currentCount > 0)
        {
            _viewsSet[view] = currentCount;
            return;
        }

        _viewsSet.Remove(view);
    }
}
