using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class TestingAnimation : MonoBehaviour
{
    string TAG = ">>DEMO ";

    // Start is called before the first frame update
    public GameObject[] AllCard;
    public GameObject EndPos;
    public GameObject[] CardDestination;
    public GameObject CloseCard,FistPos,CloseDeck,OpenDeck,MyspreadOne,TotalSpeard,Myspreadsec,SpradImg,SperadStartPos,SperadEndPos;
    public GameObject[] OpenDeckCards;

    public GameObject MyPlayerCardSuffle,OtherPlayerCardSuffle,WildcardAnim,Stemp,WildcardTag,WildcardNanu,WildcardMotu;
    public GameObject SpreadCard;
    public Sprite HitImage;

    int cardDeal = 0;
    int Spread = 0;
    public bool isFront;

    string  card = "";

    void Start()
    {

        for (int i = 0; i < AllCard.Length; i++)
        {
            AllCard[i].transform.GetComponent<Button>().onClick.AddListener(SpreadCards);

        }
        Invoke("SDC", 0.5f);
        HitVisible();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpreadCards()
    {

        isFront = true;

        GameObject cardObj = Instantiate(SpreadCard);
        cardObj.transform.SetParent(MyspreadOne.transform, false);

        for (int i = 0; i < MyspreadOne.transform.childCount; i++)
        {
            if(isFront && i == 0)
            {
                MyspreadOne.transform.GetChild(i).gameObject.transform.GetComponent<Image>().enabled = false;
            }

            else if (!isFront && i == (MyspreadOne.transform.childCount - 1))
            {
                MyspreadOne.transform.GetChild(i).gameObject.transform.GetComponent<Image>().enabled = false;
            }
            else
            {
                MyspreadOne.transform.GetChild(i).gameObject.transform.GetComponent<Image>().sprite = HitImage;
            }
        }

        card = GameObject.Find(EventSystem.current.currentSelectedGameObject.name).GetComponent<Button>().name.ToString();


        //AllCard[int.Parse(card)].transform.DOScale(1.2f, 0);

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(AllCard[int.Parse(card)].transform.transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(74f, 98f), 0.3f)).SetEase(Ease.Linear);

        mySequence.Insert(0, AllCard[int.Parse(card)].transform.DOMove(MyspreadOne.transform.GetChild(isFront ? 0 : MyspreadOne.transform.childCount - 2).transform.position, mySequence.Duration()).OnComplete(() =>
        {
            AllCard[int.Parse(card)].SetActive(false);
            MyspreadOne.transform.GetChild(isFront ? 0 : MyspreadOne.transform.childCount - 1).gameObject.transform.GetComponent<Image>().enabled = true;

        })).SetEase(Ease.Linear);
    }

    public void CloseDeckClick()
    {
        CloseCard.transform.DOMove(FistPos.transform.position, 0.7f, false).OnComplete(() =>
        {
            CloseCard.SetActive(false);

            CloseCard.transform.DOMove(CloseDeck.transform.position,0, false).OnComplete(() =>
            {
                CloseCard.SetActive(true);
                FistPos.SetActive(true);
                CloseCard.transform.position = CardDestination[int.Parse(card)].transform.position;

                CloseCard.transform.DOMove(CloseDeck.transform.position, 0, false).OnComplete(() =>
                {
                    CloseCard.SetActive(true);
                    AllCard[int.Parse(card)].SetActive(true);
                    CardDestination[int.Parse(card)].SetActive(true);
                });
            });
        });
    }

    public void OpenDeckCardClick()
    {
        OpenDeckCards[0].transform.DOMove(CardDestination[int.Parse(card)].transform.position,0.7f, false).OnComplete(() =>
        {
            OpenDeckCards[0].SetActive(false);
            AllCard[int.Parse(card)].SetActive(true);
            CardDestination[int.Parse(card)].SetActive(true);
            OpenDeckCards[0].transform.DOMove(OpenDeck.transform.position, 0, false);
           
        });
    }

    public void SDC()
    {
        CloseCard.transform.DOMove(OtherPlayerCardSuffle.transform.position,0.1f, false).OnComplete(() =>
        {
            CloseCard.SetActive(false);
            CloseCard.transform.DOMove(CloseDeck.transform.position,0,false).OnComplete(() =>
            {
                CloseCard.SetActive(true);

                CloseCard.transform.DOMove(MyPlayerCardSuffle.transform.position, 0.1f, false).OnComplete(() =>
                {
                    CloseCard.SetActive(false);
                    CloseCard.transform.DOMove(CloseDeck.transform.position, 0, false).OnComplete(() =>
                    {
                        CloseCard.SetActive(true);
                        Invoke("SDC", 0f);
                        cardDeal++;

                        if(cardDeal == 5)
                        {
                            CancelInvoke();
                            SMC();
                            //Wildcard();
                        }
                    });

                });
            });

        });
    }

    public void SpreadClick()
    {
        //Logger.Print("Card Count" + Spread);
        //if(Spread == 0)
        //{
        //    MyspreadOne.transform.DOMove(TotalSpeard.transform.position, 0.3f, false);
        //    MyspreadOne.transform.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
        //    Spread = 1;
        //    Spreading();
        //}

        //else if(Spread == 1)
        //{
        //    Logger.Print("Card Count 1 " + Spread);

        //    Spreading();
        //    Myspreadsec.transform.DOMove(TotalSpeard.transform.position, 0.3f, false).OnComplete(() =>
        //    {

        //        MyspreadOne.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(188f, 0);
        //        Myspreadsec.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-188f, 0);
        //        MyspreadOne.transform.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleRight;
        //        StartCoroutine(Wait());


        //    });

        //}

      
    }

    public void Spreading()
    {
        SpradImg.SetActive(true);

        SpradImg.transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(650f, 172f), 0.5f);

        SpradImg.transform.DOMove(SperadEndPos.transform.position, 0.5f, false).OnComplete(() =>
        {
            SpradImg.SetActive(false);
            SpradImg.transform.DOMove(SperadStartPos.transform.position, 0.5f, false);
            SpradImg.transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(50f,50f),0);


        });
    }

    public void SMC()
    {
        for(int i = 1;i<MyPlayerCardSuffle.transform.childCount;i++)
        {
            MyPlayerCardSuffle.transform.GetChild(i).gameObject.SetActive(true);
            MyPlayerCardSuffle.transform.GetChild(i).gameObject.transform.DOLocalRotate(new Vector3(0f,360f,0f),2f,RotateMode.FastBeyond360);
            Logger.Print("SMC Count" + i);
        }
    }

    public void Wildcard()
    {
        WildcardAnim.transform.GetComponent<RectTransform>().DOSizeDelta(new Vector2(113f,120f),0.5f).OnComplete(()=>
        {
            Stemp.SetActive(true);

            Stemp.transform.DOPunchScale(new Vector3(5f,5f),0.2f).OnComplete(() =>
            {
                Stemp.SetActive(false);
                WildcardTag.SetActive(true);
                Invoke("WildcardInvisible",1f);
               
            });
           
        });
    }

    public void WildcardInvisible()
    {
        WildcardAnim.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0f,0f);
        WildcardTag.SetActive(false);
        WildcardNanu.SetActive(true);

        WildcardNanu.transform.DORotate(new Vector3(0f, 0f, 50f), 0.3f, RotateMode.Fast);

        WildcardNanu.transform.DOMove(WildcardMotu.transform.position, 0.3f, false).OnComplete(() =>
        {
            WildcardNanu.SetActive(false);
            WildcardMotu.SetActive(true);
            WildcardNanu.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(458f,115f);

        });

    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        Myspreadsec.transform.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
    }

    void HitVisible()
    {

        for (int i = 0; i < 3; i++)
        {
            GameObject cardObj = Instantiate(SpreadCard);
            cardObj.transform.SetParent(MyspreadOne.transform, false);

            if (i == 0)
            {
                isFront = true;
            }


        }
    }

   
}
