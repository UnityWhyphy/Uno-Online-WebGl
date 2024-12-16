using UnityEngine;
using UnityEngine.UI;

public class SelfTurnTimerControl : MonoBehaviour
{
    [SerializeField] private Image turnImage;

    [SerializeField] private Image endTimerFadeImage;


    public Text timerTxt;

    private void Start()
    {

    }

    public Image TurnTimerImg()
    {
        return turnImage;
    }

    public Image EndTurnFadeImg()
    {
        return endTimerFadeImage;
    }
}
