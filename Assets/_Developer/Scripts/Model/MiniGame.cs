
using System.Collections.Generic;

[System.Serializable]
public class MiniGameData
{
    public string uid;
    public List<string> close_deck;
    public List<string> spades_deck;
    public List<string> heart_deck;
    public int win_index;
    public int isclose;
    public int play_try;
    public bool isover;
    public List<long> win_reward;
    public int isfreemini;
    public bool isFree;
    public bool ispromo;
    public string _id;
}
[System.Serializable]
public class UserTurnData
{
    public List<string> red_deck;
    public List<string> green_deck;
    public int win_index;
    public int play_try;
    public string win_type;
    public string type;
    public bool isover;
    public string card;
}
