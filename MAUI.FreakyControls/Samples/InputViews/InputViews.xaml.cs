using Maui.FreakyControls;

namespace Samples.InputViews;

public partial class InputViews : ContentPage
{
    InputViewModel vm;
    public InputViews()
    {
        InitializeComponent();
        BindingContext = vm = new InputViewModel();
    }
    private void SuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
    {
        var box = (AutoCompleteView)sender;
        // Filter the list based on text input
        box.ItemsSource = GetSuggestions(box.Text);
    }

    private List<string> GetSuggestions(string text)
    {
        return string.IsNullOrWhiteSpace(text) ? null : vm.Countries.Where(s => s.StartsWith(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
    }

    private void SuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
    {
       
    }
}