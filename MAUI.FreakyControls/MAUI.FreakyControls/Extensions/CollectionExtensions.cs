using System.Collections.ObjectModel;

namespace Maui.FreakyControls.Extensions;

public static class CollectionExtensions
{
    public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> col)
    {
        return new ObservableCollection<T>(col);
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
}