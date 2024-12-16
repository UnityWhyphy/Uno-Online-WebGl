#define UNITASK_DOTWEEN_SUPPORT

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectCoinAnimation : MonoBehaviour
{
    private string TAG = ">>COINANIM ";
    [SerializeField] GameObject coinPrefab, gemsPrefab;
    [SerializeField] Transform coinParent, SpawnLocation;
    [SerializeField] private int coinAmount;
    [SerializeField] private float minX, minY, maxX, maxY;

    [SerializeField] private Transform endPosition;
    [SerializeField] float duration;
    [SerializeField] private TextMeshProUGUI UserCoin;

    List<GameObject> Coins = new List<GameObject>();
    long coin, increament;

    private Tween coinReactionTween;
    int isGold = 0;

    public static CollectCoinAnimation instance;
    Action CompleteCallBack;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetConfiguration(Transform coinParent, Transform SpawnLocation, Transform endposition, TextMeshProUGUI UserCoinTxt, long GoldAdd, Action AnimComplete, int isGold, int HowManyGold)
    {
        Logger.Print(TAG + " Prefrence Manager " + long.Parse(PrefrenceManager.GOLD) + " Gems " + long.Parse(PrefrenceManager.GEMS));

        this.coinParent = coinParent;
        this.SpawnLocation = SpawnLocation;
        this.endPosition = endposition;
        this.isGold = isGold;
        this.coinAmount = HowManyGold;
        UserCoin = UserCoinTxt;
        coin = (isGold == 0 ? long.Parse(PrefrenceManager.GOLD) : long.Parse(PrefrenceManager.GEMS)) - GoldAdd;
        increament = isGold == 0 ? (GoldAdd / coinAmount) : 1;
        CompleteCallBack = AnimComplete;

        UserCoinTxt.text = AppData.numDifferentiation(coin);

        Logger.Print(TAG + " Coin " + coin + " increment " + increament);

        CollectCoin();
    }

    private void SetCoin(long i)
    {
        coin = i;

        if (UserCoin != null)
            UserCoin.text = coin.ToString();
    }
    public async void CollectCoin()
    {
        for (int i = 0; i < Coins.Count; i++)
        {
            if (Coins[i] != null)
            {
                Destroy(Coins[i]);
            }
        }

        Coins.Clear();
        List<UniTask> spawnCoinTaskList = new List<UniTask>();

        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = Instantiate(isGold == 0 ? coinPrefab : gemsPrefab, coinParent);
            float xPosition = SpawnLocation.transform.position.x + UnityEngine.Random.Range(minX, maxX);
            float yPosition = SpawnLocation.transform.position.y + UnityEngine.Random.Range(minY, maxY);

            coin.transform.position = new Vector3(xPosition, yPosition);
            var punchTween = coin.transform.DOPunchPosition(new Vector3(0, 30, 0), UnityEngine.Random.Range(0, 0.3f))
                .SetEase(Ease.InOutElastic)
                .ToUniTask();

            spawnCoinTaskList.Add(punchTween);

            Coins.Add(coin);

            await UniTask.Delay(TimeSpan.FromSeconds(0.01f));
        }

        await UniTask.WhenAll(spawnCoinTaskList);

        await MoveCoinsTask();
    }

    private async UniTask MoveCoinsTask()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.coinCollectNew);
        try
        {
            List<UniTask> moveCoinTask = new List<UniTask>();

            for (int i = Coins.Count - 1; i >= 0; i--)
            {
                if (Coins[i] != null)
                {
                    moveCoinTask.Add(MoveCoinTask(Coins[i]));
                    await UniTask.Delay(TimeSpan.FromSeconds(isGold == 0 ? 0.05f : 0.05f));
                }
            }
            await UniTask.WhenAll(moveCoinTask);
            Logger.Print(TAG + "Move Coin Animation Complete");

            CompleteCallBack?.Invoke();
        }
        catch (Exception ex)
        {
            Logger.Print($"EX {ex}");
        }     
    }

    private async UniTask MoveCoinTask(GameObject coinObject)
    {
        if (coinObject == null) return;

        // Move coin to the end position with animation
        await coinObject.transform.DOMove(endPosition.position, duration)
            .SetEase(Ease.InBack)
            .ToUniTask();

        // Remove and destroy the coin object
        Coins.Remove(coinObject);
        Destroy(coinObject);

        // React to coin collection and update coin count
        await ReactToCollectionCoin();
        SetCoin(coin + increament);
    }


    public async UniTask ReactToCollectionCoin()
    {
        if (coinReactionTween == null)
        {
            coinReactionTween = endPosition.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).SetEase(Ease.InOutElastic);
            await coinReactionTween.ToUniTask();
            coinReactionTween = null;
        }
    }
}
