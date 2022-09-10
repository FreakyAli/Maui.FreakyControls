

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace Maui.FreakyControls.Shared.Controls
{

    public class RepeaterView : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RepeaterView), null);

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterView), null);

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public RepeaterView()
        {
            Spacing = 0d;

            ReloadItems();
        }

        #region Event Handlers
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName && ItemsSource is INotifyCollectionChanged observableCollection)
            {
                observableCollection.CollectionChanged -= OnItemsSourceCollectionChanged;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemTemplateProperty.PropertyName)
            {
                // Template or separator changed
                ReloadItems();
            }
            else if (propertyName == ItemsSourceProperty.PropertyName)
            {
                // Items source changed
                ReloadItems();

                if (ItemsSource is INotifyCollectionChanged observableCollection)
                {
                    // Listen for collection change events
                    observableCollection.CollectionChanged += OnItemsSourceCollectionChanged;
                }
            }
        }

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InsertItems(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItems(e.NewItems, e.OldStartingIndex, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(e.OldItems, e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItems(e.NewItems, e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ReloadItems();
                    break;
            }
        }
        #endregion

        private View GetChild(object bindingContext)
        {
            DataTemplate itemTemplate = ItemTemplate;

            if (itemTemplate is DataTemplateSelector selector)
            {
                itemTemplate = selector.SelectTemplate(bindingContext, this);
            }

            if (itemTemplate != null && itemTemplate.CreateContent() is View child)
            {
                child.BindingContext = bindingContext;

                return child;
            }

            return null;
        }

        private void ReloadItems()
        {
            Opacity = 0d;
            Children.Clear();

            if (ItemsSource?.Count > 0)
            {
                foreach (var item in ItemsSource)
                {
                    View child = GetChild(item);
                    Children.Add(child);
                }

                this.FadeTo(1d);
            }
        }

        private void InsertItems(IList items, int startIndex)
        {
            if (startIndex > -1 && items?.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    int index = startIndex + i;

                    if (index == Children.Count)
                    {
                        Children.Add(GetChild(items[i]));
                    }
                    else
                    {
                        Children.Insert(index, GetChild(items[i]));
                    }

                }
            }
        }

        // Move is logically interpreted as a remove operation followed by an add operation
        // 1. Retrieve the views to be moved
        // 2. Remove the views from children
        // 3. Insert the views to children at toIndex
        private void MoveItems(IList items, int fromIndex, int toIndex)
        {
            if (fromIndex > -1 && toIndex > -1 && items?.Count > 0)
            {
                List<View> views = new List<View>();

                for (int i = 0; i < items.Count; i++)
                {
                    var child = Children[fromIndex] as View;
                    if (child != null)
                    {
                        views.Add(child);
                        Children.RemoveAt(fromIndex);
                    }
                }

                // Insert backwards so that the first view in the list is inserted at toIndex
                for (int i = views.Count - 1; i >= 0; i--)
                {
                    Children.Insert(toIndex, views[i]);
                }
            }
        }

        private void RemoveItems(IList items, int fromIndex)
        {
            if (fromIndex > -1 && items?.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Children.RemoveAt(fromIndex);
                }
            }
        }

        private void ReplaceItems(IList items, int fromIndex)
        {
            if (fromIndex > -1 && items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    int childIndex = fromIndex + i;

                    Children.RemoveAt(childIndex);
                    Children.Insert(childIndex, GetChild(items[i]));
                }
            }
        }
    }
}

