namespace Maui.FreakyControls;

public interface IAlphabetProvider
{
    IEnumerable<char> GetAlphabet();
    int GetCount();
}