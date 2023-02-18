using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextboxAnimation : MonoBehaviour
{
    private Vector3 originalScale;
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private float animationDuration;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(originalScale, animationDuration).SetEase(easeCurve);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
