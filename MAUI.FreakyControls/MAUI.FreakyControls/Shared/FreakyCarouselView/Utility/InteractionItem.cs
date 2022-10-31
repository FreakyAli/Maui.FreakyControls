using Maui.FreakyControls.Shared.Enums;

namespace Maui.FreakyControls.Utility;

public sealed class InteractionItem
{
    private static readonly object _itemsPoolLocker = new();
    private static readonly Stack<InteractionItem> _itemsPool = new();

    public static InteractionItem GetItem(Guid id, InteractionType type, InteractionState state)
    {
        lock (_itemsPoolLocker)
        {
            var item = _itemsPool.Count == 0
                ? new InteractionItem()
                : _itemsPool.Pop();
            item.Id = id;
            item.Type = type;
            item.State = state;
            item.IsInvolved = false;
            return item;
        }
    }

    public static InteractionItem PutItem(InteractionItem item)
    {
        lock (_itemsPoolLocker)
        {
            _itemsPool.Push(item);
            return item;
        }
    }

    private InteractionItem()
    {
    }

    public Guid Id { get; set; }
    public InteractionType Type { get; set; }
    public InteractionState State { get; set; }
    public bool IsInvolved { get; set; }
}
