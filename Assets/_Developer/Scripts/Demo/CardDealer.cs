using UnityEngine;
using DG.Tweening; // Import DOTween namespace
using System.Collections;

public class CardDealer : MonoBehaviour
{
    public GameObject cardPrefab; // Reference to your card prefab or sprite
    public Transform[] playerPositions; // Array of positions where players will receive cards
    public int cardsPerPlayer = 7; // Number of cards each player will receive
    public float dealDuration = 0.5f; // Duration of the deal animation
    public float cardOffset = 1.0f; // Offset between each card's stop position

    private void Start()
    {
       StartCoroutine(DealCards());
    }

    private IEnumerator DealCards()
    {
        int cardIndex = 0;
        for (int p = 0; p < playerPositions.Length; p++)
        {
            for (int i = 0; i < cardsPerPlayer; i++)
            {
                GameObject card = Instantiate(cardPrefab, transform.position, Quaternion.identity);
                card.transform.SetParent(transform);
                Vector3 targetPosition = playerPositions[p].position + Vector3.right * cardOffset * i;

                // Use DOTween to animate the card moving to the player's position
                card.transform.DOMove(targetPosition, dealDuration)
                    .SetEase(Ease.OutQuad) // Optional easing function
                    .OnComplete(() => {
                        // Optional: Add logic after card reaches target position
                        Debug.Log("Card reached position: " + targetPosition);
                    });

                // Adjust delay if needed
                float dealDelay = 0.1f * (cardIndex); // Adjust delay based on card index
                cardIndex++;

                yield return new WaitForSeconds(0.3f);
                DOTween.Sequence()
                    .AppendInterval(dealDelay)
                    .Append(card.transform.DOMove(targetPosition, dealDuration).SetEase(Ease.OutQuad));
            }
        }
    }
}
