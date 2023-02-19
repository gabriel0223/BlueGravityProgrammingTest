using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GrowerWindowView : MonoBehaviour
{
    [SerializeField] private AnimationCurve growEaseCurve;
    [SerializeField] private AnimationCurve shrinkEaseCurve;
    [SerializeField] private float animationDuration;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        Grow();
    }

    public void Grow(TweenCallback callback)
    {
        transform.DOScale(_originalScale, animationDuration).SetEase(growEaseCurve).OnComplete(callback);
    }
    
    public void Grow()
    {
        transform.DOScale(_originalScale, animationDuration).SetEase(growEaseCurve);
    }

    public void Shrink(TweenCallback callback)
    {
        transform.DOScale(0, animationDuration).SetEase(shrinkEaseCurve).OnComplete(callback);
    }
    public void Shrink()
    {
        transform.DOScale(0, animationDuration).SetEase(shrinkEaseCurve);
    }
}
