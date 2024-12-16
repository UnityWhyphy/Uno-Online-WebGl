using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelClass
{

}

[System.Serializable]
public class MessageClass
{
    public const string opps = "OPPS!";
    public const string exit = "Exit?";
    public const string alert = "ALERT!";
    public const string thanks = "THANKS!";
    public const string congratulations = "CONGRATULATIONS!";
    public const string emptyMessage = "EMPTY MESSAGE!";
    public const string invelidEmail = "INVELID EMAIL!";
    public const string invelidFEEDBACK = "INVELID FEEDBACK!";
    public const string dontGiveUp = "DON'T GIVE UP!";
    public const string tournamentLoss = "TOURNAMENT LOSS!";
    public const string tournamentWin = "TOURNAMENT WIN!";
    public static string tutorialBGTitle = "WHAT TIPS DO YOU HAVE FOR BEGINNERS?";
    public static string SelectRule = "Select at least one rule!";
    public static string Only6Rule = "You can select only 6 modes at a time!";

    public static string TURNMISSTOOL = "You have Time Out 1 Time if you done one time than Not allow for play.";

    public static string victorieMsg = "You won your last game! Fantastic job! Keep up the great work and aim for even more victories in the next game!";
    public static string loseMsg = "You didn’t win this time, but don't worry! Keep playing, and your next victory is just around the corner. Best of luck for your next game!";

    public static string tutorialInfomation = "Welcome to Card party, a thrilling adventure where your journey begins with mastering the basics-explore, strategize, " +
        "and immerse yourself in the fun as you learn step by step to become a pro! If you're already familiar with the game rules, feel free to skip this step.";


    public static string introCustom = "Got something new? Yes! Welcome to your new journey in Card Party. In this mode, you can customize the rules and enjoy exciting new feature cards. Don’t worry, we’ll guide you through them one step at a time!";
    public static string introInvert = "In the Card Party game, we’ve introduced a new INVERT (FLIP) mode, one you’ve never played before. In this mode, you can play with both sides of the card. Let’s begin this exciting journey with a simple click!";
    public static string introliveTable = "In the live table, you can directly join another player's table, complete with gold and mode details.";
    public static string introTour = "You've just unlocked the ultimate challenge! The Tournament lets you compete against 16 other players for a chance to win 5X more Gold than in regular gameplay.";
}


[System.Serializable]
public class TutorialStepsTexts
{
    public const string step1 = "Your opponents are Ananya, Zainab, and Gabriel. The goal of the game is to be the first to have zero cards left.";
    public const string step2 = "You can play a card that matches the color of the card currently in play.";
    public const string step3 = "You can also play a card that matches the number of the card currently in play.You can play a card by either clicking on it or swiping it.";
    public const string step4 = "When you play a Skip card, the next player skips their turn.";
    public const string step5 = "When you play a reverse card, The direction change of play!";
    public const string step6 = "When a Wild +4 is played, the next player must choose to either DRAW or CHALLENGE. If you choose DRAW, 4 cards are added to your hand.";
    public const string step6_1 = "If the challenge is correct, Ananya gets 4 penalty cards!";
    public const string step6_2 = "If the challenge is incorrect, you get 6 penalty cards!";
    public const string step7 = "When you play a '+2' card, the Next player draws 2 cards and skips their turn";
    public const string step8 = "Remember to HIT 'Last Card!' when you play your second-to-last card. If you forget and someone catches you, you'll draw penalty cards.";
    public const string step9 = "You can choose the color you need, and your opponent must draw 4 cards!";
    public const string step9_1 = "If an opponent forgets to call their last card, tag them with 'LAST CARD' on their profile to catch them!";
    public const string step10 = "This is the deck (or pile) from which you can draw a card whenever necessary.";
    public const string step11 = "Change the color to whatever you need!";
}

[System.Serializable]
public class TutorialActionSteps
{
    public const string sp_OpThrow = " has decided to assign 4 additional cards to ";
    public const string sp_PlThrow = "Choose an opponent to whom you want to assign 4 additional cards.";
    public const string cyclone_Hint = "will keep drawing cards from the open deck until he gets a";
    public const string shield_Hint = "uses a shield card to avoid the cards penalty, and";
    public const string high_fiveHint = "gave the last high-five, so he receives a 2-card penalty.";
    public const string wild_upHint = "card with a number between 0 and 9. If he plays a card, the penalty will increase by one; otherwise, he will receive penalty cards.";
    public const string wild_upHint2 = "must play a card with the same number but a different suit or a card with the next higher number in the same suit.";
    public const string zeroHint = "All Player cards swap from";
    public const string sevenHint = "Select the player you want to swap cards with.";

    public const string cardPic = "This is the deck (or pile). Whenever you don't have a playable card, you must draw one from here.";
    public const string matchColor = "You can play a card that matches the color of the card currently in play.";
    public const string matchNumber = "You can also play a card that matches the number of the card currently in play.";

    public const string skip = "The player whose turn is next skips their turn and passes it to the following player.";
    public const string reverse = "The turn direction changes: all players now follow the turn in a";
    public const string penalty = "card penalty is added to";
    public const string discardAll = "cards from your hand.";

    public const string wildplus4Own = "Choose a color, and the next player must draw 4 cards as a penalty.";
    public const string wildcolor = "Select a color you would like to change to.";

}


[System.Serializable]
public class GoldHistoryModel
{
    public string _id;
    public long gold;
    public string tp;
    public long previous_gold;
    public int isgems;
    public long total_gold;
}
