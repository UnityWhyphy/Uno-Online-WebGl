using UnityEngine;
using DG.Tweening;

public class DashboardAnimControl : MonoBehaviour
{
    [SerializeField] private bool isScaleAnim, isRotate, isRotate360, isUpSide;
    [SerializeField] private float upYValue = 7f;
    [SerializeField] private float scale = 1.03f;

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float intervalScale = 4f;
    [SerializeField] private float intervalRotate = 4f;
    [SerializeField] private float intervalRotate360 = 2f;


    private Sequence scaleSequence;
    private Sequence upSideSequence;
    private Sequence rotateSequence;
    private Sequence rotate360Sequence;

    private void Start()
    {
        RestartAnimation();
    }

    internal void RestartAnimation()
    {
        if (isScaleAnim)
        {
            StartScaleAnimation();
        }
        else if (isRotate)
        {
            StartHalfRotateAnimation();
        }
        else if (isRotate360)
        {
            StartRotate360DegreesAnimation();
        }
        else if (isUpSide)
        {
            StartUpSideMoveAnimation();
        }
    }

    private void StartScaleAnimation()
    {
        scaleSequence = DOTween.Sequence();

        // Scale up
        scaleSequence.Append(transform.DOScale(new Vector2(scale, scale), duration)
            .SetEase(Ease.Linear)
            .SetDelay(0.3f));

        // Scale down
        scaleSequence.Append(transform.DOScale(Vector2.one, duration)
            .SetEase(Ease.Linear)
            .SetDelay(0.1f));

        scaleSequence.SetLoops(-1, LoopType.Yoyo)
            .SetDelay(intervalScale);
    }

    private void StartUpSideMoveAnimation()
    {
        upSideSequence = DOTween.Sequence();

        // Move up
        upSideSequence.Append(transform.DOLocalMoveY(upYValue, duration)
            .SetEase(Ease.Linear)
            .SetDelay(0.3f));

        // Move down
        upSideSequence.Append(transform.DOLocalMoveY(0, duration)
            .SetEase(Ease.Linear)
            .SetDelay(0.1f));

        upSideSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void StartHalfRotateAnimation()
    {
        rotateSequence = DOTween.Sequence();

        // Rotate to -5
        rotateSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, -5), duration)
            .SetEase(Ease.Linear)
            .SetDelay(0.3f));

        // Rotate to 5
        rotateSequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 5), duration)
            .SetEase(Ease.Linear)
            .SetDelay(0.1f));

        rotateSequence.SetLoops(-1, LoopType.Yoyo)
            .SetDelay(intervalRotate);
    }

    private void StartRotate360DegreesAnimation()
    {
        rotate360Sequence = DOTween.Sequence();

        // Perform the 360-degree rotation
        Vector3 currentRotation = transform.localEulerAngles;
        Vector3 targetRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z + 360f);
        rotate360Sequence.Append(transform.DOLocalRotate(targetRotation, 1, RotateMode.FastBeyond360)
             .SetEase(Ease.Linear));

        // Perform the scaling animation
        rotate360Sequence.Join(transform.DOScale(new Vector2(scale, scale), duration)
            .SetEase(Ease.Linear))
            .Append(transform.DOScale(Vector2.one, duration)
            .SetEase(Ease.Linear));

        rotate360Sequence.SetLoops(-1)
            .SetDelay(intervalRotate360);
    }

    private void OnDisable()
    {
        // Kill all active animations
        if (scaleSequence != null) scaleSequence.Kill();
        if (upSideSequence != null) upSideSequence.Kill();
        if (rotateSequence != null) rotateSequence.Kill();
        if (rotate360Sequence != null) rotate360Sequence.Kill();
    }

    private void OnDestroy()
    {
        // Optionally, you can also kill animations when the object is destroyed
        OnDisable();
    }
}
