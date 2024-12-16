using System;
using System.Collections.Generic;

[Serializable]
public class Lottoland
{
    public string _id;
    public string name;
    public int ticket;
    public int amt;
    public int wamt;
    public int lsh;
    public int leh;
    public int pcount;
    public bool over;
}
[Serializable]
public class Winner
{
    public int si;
    public int points;
    public string pn;
    public string pp;
    public string frameImg;
    public string uid;
    public int _iscom;
    public List<string> cards;
    public int leftgame;
    public int w;
    public int nw;
    public int rw;
    public int team;
    public int rank;
    public int totalsecondspossionwin;

    //public int extraadd1;
    //public Oldxp oldxp;
    //public bool isleaveStargame;
    //public int oldsxp;
    //public string olddxp;
    //public int iscentralmuseumlock;
    //public int doublexp;
    //public string newdxp;
    //public Oldxp newxp;
}
[Serializable]
public class Oldxp
{
    public int clvl;
    public int cp;
    public int per;
    public int nlvp;
    public int xp;
}

//SDC
[System.Serializable]
public class Allplayercard
{
    public int si;
    public List<string> cards;
}
[System.Serializable]
public class SdcData
{
    public List<int> s;
    public int time;
    public int d;
    public string backcard;
    public string c;
    public List<string> deck;
    public List<Allplayercard> Allplayercard;
}

[System.Serializable]
public class SDCMainRoot
{
    public string en;
    public SdcData data;
    public bool flag;
}

//ITPP
[Serializable]
public class ITPPdata
{
    public string _id;
    public string pn;
    public string pp;
    public string tbid;
    public string ccf;
    public long gold;
    public int iscom;
    public bool OppUserBlock;
    public bool IBlockOppUser;
    public bool SendFreindRequest;
    public bool MyFreind;
    public Pi level;
}
[Serializable]
public class Pi
{
    public int si;
    public int clvl;
    public Ui ui;
    public string status;
    public int tc;
    public int isplay;
    public string s;
    public string giftImg;
    public int comp;
    public Oldxp oldxp;
    public int sxp;
}
[Serializable]
public class Ui
{
    public string pn;
    public string uid;
    public int _iscom;
    public int si;
    public int group;
    public int status;
    public int team;
    public string pp;
    public string furl;
    public string frameImg;
}
[Serializable]
public class NotiModel
{
    public string _id;
    public string s;
    public string r;
    public string un;
    public string pp;
    public string frameImg;
    public string msg;
    public string hmsg;
    public string t;
    public int bv;
    public string mode;
    public string tbid;
    public string title;
    public int si;
    public int gems;
    public int gold;
    public int @is;
    public int ip;
    public int isvideo;
}

[Serializable]
public class ChestModel
{
    public string chestname;
    public int time;
    public int gems;
    public List<Pay> pay;
    public int againopen;
    public bool isvideo;
    public Rw rw;
    public float lefttime;
    public int unlock;
    public int isopen;
    public int iscalim;
    public int isagainclaim;
    public int isagainopen;
    public int deducttime;
    public int issurprise;
    public int skipvideo;
    public string uuid;
}
[Serializable]
public class Pay
{
    public int s;
    public int gems;
}

[Serializable]
public class ChestReward
{
    public int gold;
    public string type;
    public string color;
    public string name;
    public int index;
}

[Serializable]
public class DeckListClass
{
    public string _id;
    public string img;
    public int price;
    public string name;
    public int isselect;
    public int isAvatar;
}

[Serializable]
public class LiveTableItemModel
{
    public string _id;
    public int ap;
    public int bv;
    public int gems;
    public List<Pi> pi;
    public List<int> rules;
    public int _ip;
    public string mode;
}

[Serializable]
public class TableBootValue
{
    public int bv;
    public int winbv;
    public int deduct;
    public int level;
    public int xp;
    public int gems;
    public string chest;
    public int position;
    public int solowinbv1;
    public int solowinbv2;
}
[Serializable]
public class TournamentSlote
{
    public int bv;
    public int winbv;
    public int gems;
}
[Serializable]
public class TounamentData
{
    public string tbid;
    public string winid;
    public int count;
    public IList<Player> player;

}
[Serializable]
public class DeckImg
{
    public string deck;
    public string bunch;
    public string backcard;

}
[Serializable]
public class Player
{
    public string pn;
    public string uid;
    public int si;
    public string pp;
    public int group;
    public int team;
    public DeckImg deckImg;
    public string frameImg;

}
[Serializable]
public class GiftModel
{
    public string img;
    public long price;
}
[Serializable]
public class DailyMissionData
{
    public string txt;
    public int total;
    public Gameplay gameplay;
    public int count;
    public int isresult;
    public int isanimation;

}
[Serializable]
public class Gameplay
{
    public string mode;
    public int bv;
    public int gems;
    public int isleague;
    public int player;

}
[Serializable]
public class WeeklyWinnerData
{
    public string _id;
    public string pn;
    public string pp;
    public string frameImg;
    public long wxp;
    public int sloterank;
    public int slote;
    public int rank;
    public Rw rw;

}
[Serializable]
public class Rw
{
    public long gold;
    public int gems;
    public int rank;
    public int min;
    public int max;
}
[Serializable]
public class Teams
{
    public string tbid;
    public string winid;
    public int count;
    public List<Player> player;

}
[Serializable]
public class LOTSData
{
    public string _id;
    public int bv;
    public int winbv;
    public string mode;
    public int count;
    public long gems;
    public List<Teams> round1;
}
[Serializable]
public class GMAM //Message Screen Message
{
    public string s;
    public string spp;
    public string sframeImg;
    public string spn;
    public string r;
    public string body;
    public DateTime cd;
    public int iur;

}
[Serializable]
public class PC //Message Screen Message
{
    public string s;
    public string pp;
    public string frameImg;
    public string pn;
    public string body;
    public DateTime cd;
}
[Serializable]
public class IS
{
    public string _id;
    public string last_msg_sender;
    public string last_msg_body;
    public string pp;
    public string frameImg;
    public string pn;
    public int newmsg_unread;
    public string cov_id;
    public DateTime lastdate;
    public int count;
    public int msg_unread;

}
[Serializable]
public class PurchaseData
{
    public double price;
    public int gems;
    public int isoffer;
    public string txt;
    public int iscombo;
    public string packid;
    public int initgold;
    public int initgems;
    public int nGold;
    public int nGems;
    public int totalgold;
    public int totalgems;
    public int isremove;
    public int promogold;
    public string _id;
    public int free_gold;
    public int free_gems;
    public int isother;
    public int isextra;
}

[Serializable]
public class LWUData
{
    public List<LevelWiseUnlock> level_wise_unlock;
}

[Serializable]
public class LevelWiseUnlock
{
    public string _id;
    public int level;
    public List<string> unlock;
}

[Serializable]
public class MainLWU
{
    public string en;
    public LWUData data;
    public bool flag;
}




