using Maui.FreakyControls;

namespace Samples.JumpList;

public class EnglishAlphabetProvider : IAlphabetProvider
{
    private readonly string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ#";

    public IEnumerable<char> GetAlphabet()
    {
        foreach (var c in alphabets)
        {
            yield return c;
        }
    }

    public int GetCount()
    {
        return alphabets.Length;
    }
}