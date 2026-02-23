namespace Eyedrop.MineXplorer.Types;

public class Token
{
    public string ID { get; set; }
    public bool Legitimate { get; set; }
    
    public Token() { }
    
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