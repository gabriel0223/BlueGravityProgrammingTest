using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverGrowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _hoverScaleMultiplier;
    [SerializeField] private float _animationDuration;

    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        _originalScale = transform.localScale;
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
        transform.DOScale(_originalScale * _hoverScaleMultiplier, _animationDuration);
        AudioManager.instance.Play(Sounds.Hover);
    }

    public void Shrink()
    {
        transform.DOScale(_originalScale, _animationDuration);
    }

    public void SetOriginalScale(Vector2 scale)
    {
        _originalScale = scale;
    }
}
