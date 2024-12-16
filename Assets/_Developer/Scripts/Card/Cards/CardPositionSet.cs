using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class CardPositionSet : Singleton<CardPositionSet>
{
    public string TAG = "CardPositionSet";

    public List<GameObject> cards = new List<GameObject>();
    [SerializeField] int[] widthArray;

    [Space(5)]
    public float height = 0.5f;
    public float width = 1f;
    [Range(0f, 90f)] public float maxCardAngle = 5f;
    public float yPerCard = -0.005f;
    public float zDistance;
    public float moveDuration;
    public bool isAnimationStart;
    //int completedAnimations = 0;

    private void Awake()
    {
        base.SingletonAwake();
    }

    private void CardListUpdate()
    {
        cards = new List<GameObject>();
        foreach (Transform childCard in gameObject.transform)
        {
            cards.Add(childCard.gameObject);
        }
    }

    private void CalculateCardsAngles(out float radius, out float angle, out float cardAngle)
    {
        radius = Mathf.Abs(height) < 0.001f ?
            width * width / 0.001f * Mathf.Sign(height) :
            height / 2f + width * width / (8f * height);

        angle = 2f * Mathf.Asin(0.5f * width / radius) * Mathf.Rad2Deg;
        angle = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), maxCardAngle * (cards.Count - 1));

        cardAngle = cards.Count == 1 ? 0f : angle / (cards.Count - 1f);
    }

    internal void CustomUpdateCardPosition()
    {
        isAnimationStart = true;

        float radius, angle, cardAngle;
        CalculateCardsAngles(out radius, out angle, out cardAngle);

        // To keep track of how many animations are still running
        int animationsRunning = CardDeckController.instance.playerData[0].myCards.Count;

        Logger.Print($"{TAG} : Compare both list {cards.Count} == {CardDeckController.instance.playerData[0].myCards.Count}");
        for (int i = 0; i < CardDeckController.instance.playerData[0].myCards.Count; i++)
        {
            Vector3 position = new Vector3(0f, radius, 0f);
            position = Quaternion.Euler(0f, 0f, angle / 2f - cardAngle * i) * position;
            position.y += height - radius;

            //Logger.Print($"Pos Y : {cards[i].transform.localPosition.y} || isThrowAble = {CardDeckController.instance.playerData[0].myCards[i].isThrowAble}");
            var myCard = CardDeckController.instance.playerData[0].myCards;
            if (CardDeckController.instance.playerData[0].myCards[i].isThrowAble)
            {
                if (!CardDeckController.instance.playerData[0].myCards[i].isGlowScale)
                {
                    myCard[i].transform.DOLocalMove(CardDeckController.instance.playerData[0].myCards[i].startPos, moveDuration);
                    myCard[i].transform.DOLocalRotate(new Vector3(0f, 0f, angle / 2f - cardAngle * i), .1f).OnComplete(() =>
                    {
                        animationsRunning--; // One animation completed
                        CheckAnimationCompletion();
                    });
                }
                else
                {
                    animationsRunning--; // One animation skipped (not thrown)
                    CheckAnimationCompletion();
                }
            }
            else
            {
                myCard[i].transform.DOKill();
                myCard[i].transform.DOLocalMove(position, moveDuration);
                myCard[i].transform.DOLocalRotate(new Vector3(0f, 0f, angle / 2f - cardAngle * i), moveDuration)
                    .OnComplete(() =>
                    {
                        animationsRunning--; // One animation completed
                        CheckAnimationCompletion();
                    });
            }
        }

        // Function to check if all animations are completed
        void CheckAnimationCompletion()
        {
            if (animationsRunning <= 0)
            {
                if (cards.Count <= 4) Invoke(nameof(DelayFalseFlag), 0.35f);
                else
                {
                    DelayFalseFlag();// Set to false when all animations are done
                }
            }
        }
    }

    void DelayFalseFlag()
    {
        isAnimationStart = false;
    }

    public void UpdateCardPosition(bool moveAnimation)
    {
        try
        {
            isAnimationStart = true;
            CardListUpdate();

            int cardCount = cards.Count;

            // Determine width and height based on card count
            width = (cardCount >= 2 && cardCount <= 8) ? widthArray[cardCount - 2] : widthArray[6];
            height = cardCount switch
            {
                4 or 5 => 15,
                > 3 => 30,
                3 => 5,
                _ => 0.5f
            };

            // Adjust width based on specific card counts
            width = cardCount switch
            {
                5 => 550,
                4 => 390,
                3 => 200,
                _ => width
            };

            CalculateCardsAngles(out float radius, out float angle, out float cardAngle);

            // Prepare the animation handling and card position updates
            Vector3 basePosition = new Vector3(0f, radius, 0f);
            Vector3 yOffset = new Vector3(0f, yPerCard, zDistance);

            for (int i = 0; i < cardCount; i++)
            {
                float currentAngle = angle / 2f - cardAngle * i;
                Quaternion rotation = Quaternion.Euler(0f, 0f, currentAngle);

                Vector3 position = rotation * basePosition;
                position.y += height - radius;
                position += i * yOffset;

                Transform cardTransform = cards[i].transform;

                if (cardTransform != null)
                {
                    if (moveAnimation)
                    {
                        cardTransform.DOKill();
                        cardTransform.DOLocalMove(position, moveDuration);
                        cardTransform.DOLocalRotate(new Vector3(0f, 0f, currentAngle), moveDuration)
                            .OnComplete(() => isAnimationStart = false);
                    }
                    else
                    {
                        cardTransform.localPosition = position;
                        cardTransform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
                        isAnimationStart = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Print($"ERROR : {ex.Message}");
        }
    }


    public void Add(GameObject card) => Add(card, -1);
    public void Add(GameObject card, int index)
    {
        if (index == -1)
        {
            cards.Add(card);
        }
        else
        {
            cards.Insert(index, card);
        }
    }

    public void RemoveCard(GameObject card)
    {
        if (cards.Count == 0)
            return;
        Remove(card);

        //if (cards.Count < 7 && cards.Count >= 4)
        //{
        //    width -= 180;
        //}
        //else
        //    width -= 100;

        //height -= 5;
        UpdateCardPosition(true);
    }
    public void Remove(GameObject card)
    {
        if (!cards.Contains(card))
            return;
        cards.Remove(card);
        card.transform.DOKill();
    }
    public void ResetCardData()
    {
        cards.Clear();
    }
}

