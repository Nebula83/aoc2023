enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    Three = 3,
    FullHouse = 4,
    Four = 5,
    Five = 6,
}

enum CardType
{
    HighCard = 'K',
    OnePair,
    TwoPair,
    Three,
    FullHouse,
    Four,
    Five,
}


class Hand : IComparable<Hand> 
{
    private static Dictionary<char, int> cCardScore1 = new Dictionary<char, int>
    {
        {'A', 12}, 
        {'K', 11}, 
        {'Q', 10}, 
        {'J', 9}, 
        {'T', 8}, 
        {'9', 7}, 
        {'8', 6}, 
        {'7', 5}, 
        {'6', 4},
        {'5', 3},
        {'4', 2},
        {'3', 1},
        {'2', 0}
    };

    private static Dictionary<char, int> cCardScore2 = new Dictionary<char, int>
    {
        {'A', 12}, 
        {'K', 11}, 
        {'Q', 10}, 
        {'T', 9}, 
        {'9', 8}, 
        {'8', 7}, 
        {'7', 6}, 
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1},
        {'J', 0}, 
    };

    private static Dictionary<char, int> cCardScore = cCardScore1;

    public Hand(bool useScore2 = false)
    {
        if (useScore2)
        {
            cCardScore = cCardScore2;
        }
    }

    public List<char> Cards { get; set; } = new List<char>();

    public HandType Type { get; set; }

    public int Bid { get; set; }
    
    public int CompareTo(Hand? other) 
    {
        if (Type == other?.Type)
        {
            int compare = 0;
            for (int cardIndex = 0; (cardIndex < Cards.Count) && (compare == 0); cardIndex++)
            {
                compare = cCardScore[Cards[cardIndex]].CompareTo(cCardScore[other.Cards[cardIndex]]);
            }
            return compare;
        }
        return Type.CompareTo(other?.Type);
    }
}
