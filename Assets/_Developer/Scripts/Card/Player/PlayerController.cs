using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public List<CardController> myCards = new List<CardController>();
    public Transform myCardPos/*, myThrowCardPos*/;
    public Transform stackCardParent;
    public Transform specefic4Btn;
    public Transform swapBtn;
    public bool isMyPlayer;

    public RawImage profilePic;
    public Image profileFrm;
    public TextMeshProUGUI playerName;

    public GameObject playerAnimCardCopy; // throw
    public GameObject playerAnimCard; // throw
    public GameObject trowDestination;
    public GameObject flipTag;


    [SerializeField] int x, z;
    [SerializeField] int mySeatIndex;
    int nextUserSeatIndex = -1;


    private void Start()
    {

    }

    public IEnumerator TurnUTS(List<string> cardValue, bool lastCard = false) // Only work on tutorial
    {
        float ran = Random.Range(1, 2);

        Logger.Print($"TurnUTS {ran}");
        yield return new WaitForSeconds(ran);

        string cs = GameManager.getCardColor(cardValue[0]);
        if (GameManager.instance.getCardValue(cardValue[0]) == 10)
        {
            nextUserSeatIndex = mySeatIndex;
            nextUserSeatIndex += 1;
        }
        else if (GameManager.getCardColor(cardValue[0]).Equals("k"))
        {
            GameManager.instance.TutorialWild4PlusCard("y");
            cs = "y";
        }
        Logger.Print($"TurnUTS || str {GameManager.instance.getCardValue(cardValue[0])} - {GameManager.getCardColor(cardValue[0])}");
        AudioManager.instance.AudioPlay(AudioManager.instance.cardThrow);
        /*
           CardThrowAnimation(LeftPlayerAnimCard, LeftThrowCard, data["c"], LeftPlayerAnimCardCopy, 10, 45,
                data["isReverse"].AsBool, data["cs"], skipSi, (data["discardCards"].Count > 0), -1, SkipAll, leftFlipTag, isThrowFlipCard, closeDeck, isShildHas, isPreviousHas);*/
        GameManager.instance.CardThrowAnimation(playerAnimCard, trowDestination, cardValue[0], playerAnimCardCopy, x, z,
            GameManager.instance.CenterGlowCard.reverse, cs, nextUserSeatIndex, false, -1, null, flipTag, false);

        cardValue.RemoveAt(0);
        nextUserSeatIndex = -1;

        if (lastCard)
        {
            GameManager.instance.TutorialTopPlayerLastCardForget();
        }

        StartCoroutine(GameManager.instance.WaitTutorialAnimationOff(0));
    }
}
