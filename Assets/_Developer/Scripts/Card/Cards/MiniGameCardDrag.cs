using UnityEngine;
using UnityEngine.EventSystems;

public class MiniGameCardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerDownHandler
{
    private static string TAG = "CardMove ";
    public RectTransform parentImage;

    private RectTransform cardRectTransform;

    [SerializeField] GameObject CenterDeck;


    private void Start()
    {
        cardRectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Dashboard_MiniGame.instance.TransImageAnimFalse();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentImage, eventData.position, eventData.pressEventCamera, out localPoint);

        Vector2 cardPosition = localPoint;
        cardRectTransform.anchoredPosition = new Vector2(
            Mathf.Clamp(cardPosition.x, -parentImage.rect.width / 2 + cardRectTransform.rect.width / 2, parentImage.rect.width / 2 - cardRectTransform.rect.width / 2),
            Mathf.Clamp(cardPosition.y, -parentImage.rect.height / 2 + cardRectTransform.rect.height / 2, parentImage.rect.height / 2 - cardRectTransform.rect.height / 2)
        );

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        Logger.Print(TAG + " Drag Complete " + CenterDeck.transform.position + " Card " + transform.position);

        if (Mathf.Abs(CenterDeck.transform.position.x - transform.position.x) > 1)
        {
            Loading_screen.instance.ShowLoadingScreen(true);

            if (CenterDeck.transform.position.x > transform.position.x)
            {
                //green side move thai
                EventHandler.UserTurn(Dashboard_MiniGame.instance._id, Dashboard_MiniGame.instance.green);
            }
            else if (CenterDeck.transform.position.x < transform.position.x)
            {
                //red side move thai
                EventHandler.UserTurn(Dashboard_MiniGame.instance._id, Dashboard_MiniGame.instance.red);
            }
        }
    }
}
