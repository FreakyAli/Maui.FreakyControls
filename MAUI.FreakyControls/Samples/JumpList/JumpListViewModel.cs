namespace Samples.JumpList;

public class JumpListViewModel : MainViewModel
{
    public List<string> Names { get; }

    public JumpListViewModel()
    {
        Names = names.OrderBy(x => x).ToList();
    }
}