using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    [Header("Score")]
    public int globalScore;
    [SerializeField] int minScore = 100;
    [SerializeField] Text displayScore;
    [SerializeField] Text displayMinimumScore;
    Vector3 baseScale;

    [SerializeField] GameObject floatingText;
    [SerializeField] Transform spawnText;

    [Header("Timer")]
    [SerializeField] int gameTimer = 40;
    int currentTimer;
    float realTimer;
    bool started;
    [SerializeField] Text displayTimer;
    [SerializeField] Image imageTimer;

    [Header("Endgame")]
    [SerializeField] Image fadingScreen;
    [SerializeField] Text endText;

    private void Start()
    {
        EventManager.Instance.onAddScore.AddListener(AddScore);
        EventManager.Instance.onNewGame.AddListener(Launch);
        EventManager.Instance.onEndGame.AddListener(Ending);
        baseScale = displayScore.transform.localScale;
    }

    void Launch()
    {
        imageTimer.DOFillAmount(0, gameTimer);
        displayMinimumScore.text = "Minimum : " + minScore;
        currentTimer = gameTimer;
        started = true;
    }

    public void AddScore(float amount)
    {
        if (currentTimer <= 0)
            return;

        GameObject newText = Instantiate(floatingText, transform);
        Vector3 baseNewScale = newText.transform.localScale;
        newText.transform.position = spawnText.transform.position;
        newText.transform.localScale = Vector3.one * 0.01f;

        float tweenDuration = 2;
        newText.transform.DOScale(baseNewScale * 2f, tweenDuration);
        Text _text = newText.GetComponentInChildren<Text>();
        _text.text = "+" + amount;
        _text.DOFade(0, tweenDuration + 1);
        Destroy(newText, tweenDuration);

        globalScore += (int)amount;
        displayScore.transform.DOKill(true);
        displayScore.text = globalScore.ToString();
        displayScore.transform.DOPunchScale(baseScale * 0.5f, 1f, 5, 0.4f);

        if (globalScore > minScore)
            displayScore.color = Color.green;
        else
            displayScore.color = Color.red;
    }

    public void Ending(bool value)
    {
        endText.GetComponent<Text>().DOFade(1, 2f);
        fadingScreen.GetComponent<Image>().DOFade(0.75f, 2f);

        if (value)
        {
            endText.text = "You win !!";
            endText.color = Color.green;
            StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex + 1, 5f));
        }
        else
        {
            endText.text = "Game Over !!";
            endText.color = Color.red;
            StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex, 5f));
        }
    }

    IEnumerator Load(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ManageTimer()
    {
        displayTimer.text = currentTimer.ToString();
        if (!started)
            return;

        realTimer += Time.deltaTime;
        if (realTimer > 1 && currentTimer > 0)
        {
            currentTimer--;
            Vector3 baseScale = displayTimer.transform.localScale;
            float tweenDuration = 0.4f;
            displayTimer.transform.DOScale(baseScale * 1.2f, tweenDuration);
            displayTimer.transform.DOScale(baseScale, tweenDuration / 2).SetDelay(tweenDuration / 2);
            realTimer = 0;
        }

        if (currentTimer <= 0)
        {
            EventManager.Instance.onEndGame.Invoke(globalScore > minScore);
        }
    }

    private void Update()
    {
        ManageTimer();
    }
}
