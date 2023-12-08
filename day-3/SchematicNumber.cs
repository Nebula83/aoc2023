class SchematicNumber
{
    public SchematicNumber(int lineNr, int start)
    {
        Start = start;
        Line = lineNr;
    }

    public int Line { get; }
    public int Start {get; }
    public string Text { get; set; } = "";
    public int Number { get; set; }
    public int Size { get{ return Text.Length; } }
    public bool IsPart { get; set; }
    public int CogX { get; set; } = -1;
    public int CogY { get; set; } = -1;
}
