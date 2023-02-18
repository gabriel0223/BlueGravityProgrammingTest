using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverGrowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    [SerializeField] private float hoverScaleMultiplier;
    [SerializeField] private float animationDuration;
    [SerializeField] private string hoverSound;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Grow();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Shrink();
    }

    public void Grow()
    {
        transform.DOScale(originalScale * hoverScaleMultiplier, animationDuration);
        AudioManager.instance.Play(hoverSound);
    }

    public void Shrink()
    {
        transform.DOScale(originalScale, animationDuration);
    }

    public void SetOriginalScale(Vector2 scale)
    {
        originalScale = scale;
    }
}
