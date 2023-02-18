using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GrowerWindow : MonoBehaviour
{
    private Vector3 originalScale;
    [SerializeField] private AnimationCurve growEaseCurve;
    [SerializeField] private AnimationCurve shrinkEaseCurve;
    [SerializeField] private float animationDuration;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        Grow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grow(TweenCallback callback)
    {
        transform.DOScale(originalScale, animationDuration).SetEase(growEaseCurve).OnComplete(callback);
    }
    
    public void Grow()
    {
        transform.DOScale(originalScale, animationDuration).SetEase(growEaseCurve);
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
