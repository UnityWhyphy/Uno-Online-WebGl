using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private string TAG = "AUDIO MANAGER >> ";
    public static AudioManager instance;
    public AudioSource tikAudioSource;
    public AudioSource audioSource;
    public AudioSource BackGroundAudioSource;
    public AudioSource gIFSoundSource;
    public AudioClip ButtonClickClip;
    public AudioClip cardPick, cardDeal, OpenCard, unoSound, aleartPopUp, userTurn, colorChange, reverseCard, skipTurn, winnerSound, collectBV, openingCards, cardThrow,
        challengePopUp, normalPanelty, unoPanelty, lossSound, lastCard, coinCollectNew;

    [Space(5)]
    public AudioClip questReward, card_open_mining, minigame_lose, minigame_won;
    [Space(5)]

    public AudioClip dailySpin, spinner;
    public AudioClip missionClaim;
    public AudioClip purchaseThing;
    public AudioClip countDown;
    public AudioClip hummar, chgTrue, chgFalse, hummarBreak;


    [Header("New sounds")]
    public AudioClip stopCard1;
    public AudioClip stopCard2;
    public AudioClip reversArrow1;
    public AudioClip reversArrow2;
    public AudioClip luckSpiner;
    public AudioClip colorChangeClip;

    public AudioClip userLastTurnClip;

    public AudioClip lightingClip;

    [Header("GST Clip")]
    public AudioClip[] gstClips;

    [Header("EMOJI MODE")]
    public List<AudioClip> gIFSounds;

    [Header("Flip MODE")]
    public AudioClip darkFlip;
    public AudioClip lightFlip;
    public AudioClip stackFlip;

    [Space(5)]
    [Header("Chest Clips")]
    public AudioClip ItemOpenChest;
    public AudioClip PopupClip;
    public AudioClip MissionTimer;
    public AudioClip ChestTimeComplete;
    public AudioClip OfferComeClip;
    public AudioClip ChestAdd;

     [Space(5)]
    [Header("Action Clips")]
    public AudioClip highfive; //HIGH-FIVE kare tyare all user
    public AudioClip otcDeck; //OTCDECK ave and animation thai tyare
    public AudioClip shield; //WILD SHIELD throw kari and profile par image ave SHIELD ni tyare
    public AudioClip wildUp; //WILD UP card center ma animation thai tyare
    public AudioClip cycloneClip; //cycloneClip profile ma animation thai tyare
    public AudioClip sevenClip; //seven profile ma animation thai tyare
    public AudioClip zeroClip; //zero profile ma animation thai tyare



    //[Space(5)]
    //BackGroundSound
    //public AudioClip EventSoundClip, EventTenSoundClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AudioPlay(AudioClip clip)
    {
        Logger.Print(TAG + " Clip Name :::: " + clip.name);
        if (PrefrenceManager.Sound == 0)
            audioSource.PlayOneShot(clip);
    }

    public void PlayGIFSound(AudioClip clip)
    {
        //Logger.Print(TAG + " >>>>>>>>>> GIF SOUND PLAY :::: " + clip.name);
        gIFSoundSource.Stop();
        if (PrefrenceManager.Sound == 0)
            gIFSoundSource.PlayOneShot(clip);
    }

    public void AudioPlayBackGround(AudioClip clip)
    {
        BackGroundAudioSource.clip = clip;
        BackGroundAudioSource.Play();
    }

    public void PlayVibration()
    {
        //if (PrefrenceManager.Vibrate == 0)
        //{
        //    Logger.Print(TAG + " Vibration Performed !!!");
        //    Handheld.Vibrate();
        //}
    }

}
