using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SineMoveTweener : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _endYPosition;
    [SerializeField] private float _animationDuration;

    void Start()
    {
        _targetTransform.DOLocalMoveY(_endYPosition, _animationDuration)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
