using System.Text;

namespace Eyedrop.MineXplorer.Types;

public static class SpeakManager
{
    public static readonly string SpeakPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Eyedrop", "speak.txt");

    static SpeakManager()
    {
        if (!File.Exists(SpeakPath)) File.CreateText(SpeakPath).Close();
    }
    
    public static string WordlistPath
    {
        get
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Content", "wordlist.txt");
        }
    }
    
    public static void AddLine(string content)
    {
        using var file = File.Open(SpeakPath, FileMode.Append);
        file.Write(Encoding.UTF8.GetBytes($"{content}\n"));
    }
    
    public static void RemoveTopLine()
    {
        var lines = File.ReadAllLines(SpeakPath).Skip(1);
        File.WriteAllLines(SpeakPath, lines);
    }
    
    public static int LineAmount()
    {
        return File.ReadAllLines(SpeakPath).Length;
    }
    
    public static string ConvertIndicesToString(int[] indices)
    {
        var final = "";
        
        var path = WordlistPath;
        var wordList = File.ReadAllText(path).Split(" ");
        
        var lastWord = "";
        var linkAmount = 0;
        
        for (int i = 0; i < indices.Length; i++)
        {
            var idx = indices[i];
            
            if (idx >= wordList.Length)
            {
                final += " undefined";
                lastWord = "undefined";
            }
            else if (idx < 0)
            {
                final += " ";
                lastWord = "";
            }
            else
            {
                var word = wordList[idx];
                if (word != lastWord) linkAmount = 0;
                if (word == "s" || word == "ing" || word == "ed" || word == "d" ||
                    word == "." || word == "," || word == ":" || lastWord == ":" || word == "!" || word == "?"
                    || word == "'s" || word == "'ll" || word == "ly" || (lastWord == "un" && i > 1))
                {
                    if (linkAmount == 2 && (word == "s" || word == "ed" || word == "d" || word == "un"))
                    {
                        final += $" {word}";
                        linkAmount = 1;
                    }
                    else
                    {
                        final += word;
                        linkAmount++;
                    }
                }
                else
                {
                    final += $" {word}";
                    linkAmount = lastWord == "un" ? 1 : 0;
                }
                
                lastWord = word;
            }
        }
        
        return final.TrimStart();
    }
}