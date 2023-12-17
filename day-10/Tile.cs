class Tile
{
    public Tile(int row, int column, char c)
    {
        Type = c;
        Row = row;
        Column = column;
        NorthOpen = Type == '|' || Type == 'L' || Type == 'J' || Type == 'S';
        EastOpen  = Type == '-' || Type == 'L' || Type == 'F' || Type == 'S';
        SouthOpen = Type == '|' || Type == '7' || Type == 'F' || Type == 'S';
        WestOpen  = Type == '-' || Type == 'J' || Type == '7' || Type == 'S';
    }

    public Tile? Parent { get; set; } = null;
    public int Row { get; set; }
    public int Column { get; set; }

    public int Distance { get; set; } = 0;
    public bool Visited { get; set; } = false;
    public bool Filled { get; set; } = false;

    public bool NorthOpen { get; set; }
    public bool EastOpen { get; set; }
    public bool SouthOpen { get; set; }
    public bool WestOpen { get; set; }
    public char Type { get; set; }
}