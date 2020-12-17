using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartTimer : MonoBehaviour
{
    Text t_Text => GetComponent<Text>();
    [SerializeField] int timer = 5;

    private void Awake()
    {
        StartCoroutine(TimerText());
    }

    IEnumerator TimerText()
    {
        Vector3 baseScale = t_Text.transform.localScale;
        for (int i = timer; i > 0; i--)
        {
            t_Text.text = i.ToString();
            t_Text.transform.DOScale(baseScale * 1.5f, 0.2f);
            t_Text.transform.DOScale(baseScale, 0.6f).SetDelay(0.2f);
            yield return new WaitForSeconds(1);
        }

        t_Text.text = "Go !!!";
        t_Text.transform.DOScale(baseScale * 1.5f, 1.5f);
        t_Text.DOFade(0, 2f);
        EventManager.Instance.onNewGame.Invoke();
    }
}
