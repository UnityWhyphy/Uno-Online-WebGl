using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : MonoBehaviour
{
    private static string TAG = ">>Rotation ";
    public bool start = false, reverse = false;
    public string RingColor = "";
    public List<Sprite> RingSprite, RingSpriteReverse;
    public Image ringImg;
    public Sprite defaultImg;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, reverse ? 360 : -360) * Time.deltaTime * 0.2f);
    }

    public void SetRingColor(string c)
    {
        Logger.Print(TAG + " SetRingColor " + c + " Reverse " + reverse);
        Logger.Print($"<color=white><b> SetRingColor ::: {c} </b></color>|  Reverse :{reverse} " );

        switch (c)
        {
            case "r":
                ringImg.sprite = reverse ? RingSpriteReverse[0] : RingSprite[0];
                break;

            case "g":
                ringImg.sprite = reverse ? RingSpriteReverse[1] : RingSprite[1];
                break;

            case "b":
                ringImg.sprite = reverse ? RingSpriteReverse[2] : RingSprite[2];
                break;

            case "y":
                ringImg.sprite = reverse ? RingSpriteReverse[3] : RingSprite[3];
                break;

            case "o":
                ringImg.sprite = reverse ? RingSpriteReverse[4] : RingSprite[4]; // orenge
                break;

            case "t":
                ringImg.sprite = reverse ? RingSpriteReverse[5] : RingSprite[5]; // Teal
                break;

            case "j":
                ringImg.sprite = reverse ? RingSpriteReverse[6] : RingSprite[6];// Jambli
                break;

            case "p":
                ringImg.sprite = reverse ? RingSpriteReverse[7] : RingSprite[7];// Purple
                break;
        }
    }
}
