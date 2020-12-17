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
        ScaleAnim();
    }

    void ScaleAnim()
    {
        transform.DOScale(baseScale + Vector3.one * amount, duration/2);
        transform.DOScale(baseScale, duration / 2).SetDelay(duration / 2);
        Invoke(nameof(ScaleAnim), duration);
    }
}
