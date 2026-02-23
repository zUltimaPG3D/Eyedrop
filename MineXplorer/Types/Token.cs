namespace Eyedrop.MineXplorer.Types;

public class Token
{
    public string ID;
    public bool Legitimate;
    
    public Token(string id)
    {
        ID = id;
        Legitimate = true;
    }
    
    public Token(string id, bool legit)
    {
        ID = id;
        Legitimate = legit;
    }
}