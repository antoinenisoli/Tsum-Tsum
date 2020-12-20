using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dolly_ScaleAnim : MonoBehaviour
{
    [SerializeField] float duration = 3;
    [SerializeField] float amount = 0.5f;
    Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
        Sequence tween = DOTween.Sequence();
        tween.Append(transform.DOScale(baseScale + Vector3.one * amount, duration / 2));
        tween.SetLoops(-1, LoopType.Yoyo);
    }
}
