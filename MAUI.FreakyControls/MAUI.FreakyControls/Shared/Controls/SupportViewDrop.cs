using System;
namespace Maui.FreakyControls.Shared.Controls
{
    public class Refresh
    {

    }

    public enum SupportAutoCompleteDropMode
    {
        SingleTitle, TitleWithDescription, IconAndTitle, FullTextAndIcon
    }

    public interface IAutoDropItem
    {
        string IF_GetTitle();
        string IF_GetDescription();
        string IF_GetIcon();
        Action IF_GetAction();
        bool IF_GetChecked();
        void IF_SetChecked(bool _Checked);
    }

    public class SupportViewDrop : SupportViewBase
    {
        public static readonly BindableProperty DropModeProperty = BindableProperty.Create("DropMode", typeof(SupportAutoCompleteDropMode), typeof(SupportViewDrop), SupportAutoCompleteDropMode.SingleTitle);
        public SupportAutoCompleteDropMode DropMode
        {
            get { return (SupportAutoCompleteDropMode)GetValue(DropModeProperty); }
            set { SetValue(DropModeProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable<IAutoDropItem>), typeof(SupportViewDrop), null);
        public IEnumerable<IAutoDropItem> ItemsSource
        {
            get { return (IEnumerable<IAutoDropItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create("HasShadow", typeof(bool), typeof(SupportViewDrop), false);
        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public static readonly BindableProperty SeperatorColorProperty = BindableProperty.Create("SeperatorColor", typeof(Color), typeof(SupportViewDrop), Color.FromArgb("#f1f1f1"));
        public Color SeperatorColor
        {
            get => (Color)GetValue(SeperatorColorProperty);
            set => SetValue(SeperatorColorProperty, value);
        }

        public static readonly BindableProperty SeperatorHeightProperty = BindableProperty.Create("SeperatorHeight", typeof(int), typeof(SupportViewDrop), 1);
        public int SeperatorHeight
        {
            get => (int)GetValue(SeperatorHeightProperty);
            set => SetValue(SeperatorHeightProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(SupportViewDrop), Colors.Black);
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty DescriptionTextColorProperty = BindableProperty.Create("DescriptionTextColor", typeof(Color), typeof(SupportViewDrop), Colors.DarkGray);
        public Color DescriptionTextColor
        {
            get => (Color)GetValue(DescriptionTextColorProperty);
            set => SetValue(DescriptionTextColorProperty, value);
        }

        public static readonly BindableProperty RefreshListProperty = BindableProperty.Create("RefreshList", typeof(Refresh), typeof(SupportViewDrop), null);
        public Refresh RefreshList
        {
            get => (Refresh)GetValue(RefreshListProperty);
            set => SetValue(RefreshListProperty, value);
        }
    }
}

