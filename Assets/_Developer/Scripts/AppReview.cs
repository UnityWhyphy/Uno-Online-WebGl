using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using Google.Play.Review;
#endif

public class AppReview : MonoBehaviour
{
#if UNITY_ANDROID
    private ReviewManager _reviewManager;
    PlayReviewInfo _playReviewInfo;
#endif

    public static AppReview Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
#if UNITY_ANDROID
        _reviewManager = new ReviewManager();
#endif
    }

    internal IEnumerator ReviewPopup()
    {
        yield return new WaitForSeconds(1f);

#if UNITY_ANDROID
        var requestFlowOperation = _reviewManager.RequestReviewFlow();

        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);

        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            yield break;
        }

#elif UNITY_IOS
        IOSReviewRequest.RequestReview();
#endif

    }

}