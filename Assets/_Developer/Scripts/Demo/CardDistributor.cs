using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CardDistributor : MonoBehaviour
{
    public GameObject cardPrefab; // Reference to the card prefab
    public Transform[] playerPositions; // Positions where cards will be distributed to (assuming 7 positions)
    public Sprite[] cardSprites; // Sprites for front of the cards

    void Start()
    {
        StartCoroutine(DistributeCards());
    }

    IEnumerator DistributeCards()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            newCard.transform.SetParent(transform); // Set parent to the distributor (optional)

            // Set card sprite (front of the card)
            SpriteRenderer cardRenderer = newCard.GetComponent<SpriteRenderer>();
            if (cardRenderer != null && i < cardSprites.Length)
            {
                cardRenderer.sprite = cardSprites[i];
            }

            Vector3 startPos = transform.position;
            Vector3 endPos = playerPositions[i].position;

            // Initial animation (move to player position)
            newCard.transform.DOMove(endPos, 0.5f).SetEase(Ease.OutQuad);

            // Optional: Rotate and scale animation
            newCard.transform.DORotate(new Vector3(0, 180, 0), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
            newCard.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.OutBack);

            // Flip animation (from back to front)
            Sequence flipSequence = DOTween.Sequence();
            flipSequence.Append(newCard.transform.DOScaleX(0, 0.25f).SetEase(Ease.InQuad));
            flipSequence.AppendCallback(() =>
            {
                // Set back sprite (or perform other card flip logic)
                cardRenderer.sprite = cardSprites[i]; // Assuming cardSprites[i] is the front sprite

                // Flip back to front
                newCard.transform.DOScaleX(1, 0.25f).SetEase(Ease.OutQuad);
            });

            yield return new WaitForSeconds(0.2f); // Optional delay between card animations
        }
    }

    //IEnumerator InitCardWithAnimation(GameObject[] arrayCards)
    //{
    //    SortCards sort = new SortCards();
    //    arrayCards = sort.SortCard(arrayCards); ;
    //    Camera cam = Camera.main;
    //    float height = 2f * cam.orthographicSize;
    //    float widthCard = arrayCards[0].GetComponent<Renderer>().bounds.size.x;
    //    float x = -6 * (widthCard / 3);
    //    int layer = 1;
    //    float z = 13;
    //    foreach (GameObject item in arrayCards)
    //    {


    //        GameObject rocketClone = (GameObject)Instantiate(item, new Vector3(0, 0, z), transform.rotation);

    //        iTween.MoveTo(rocketClone, new Vector3(x, -height / 3, z), 1.5f);
    //        yield return rocketClone;
    //        yield return new WaitForSeconds(0.3f);

    //        x += (widthCard / 3);
    //        //increase ordermin layer
    //        rocketClone.GetComponent<SpriteRenderer>().sortingOrder += layer;
    //        layer++;
    //        z--;
    //        arrCard.Add(rocketClone);
    //    }
    //}
}
