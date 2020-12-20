using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    LevelData levelData => LevelManager.Instance.SceneInfo;

    [Header("Score")]
    public int globalScore;
    [SerializeField] Text displayScore;
    [SerializeField] Text displayMinimumScore;
    Vector3 baseScale;

    [SerializeField] GameObject floatingText;
    [SerializeField] Transform spawnText;
    [SerializeField] Image banner;

    [Header("Timer")]
    [SerializeField] Text displayTimer;
    [SerializeField] Image imageTimer;
    int currentTimer;
    float realTimer;
    bool started;

    [Header("Endgame")]
    [SerializeField] Image fadingScreen;
    [SerializeField] Text endText;

    private void Start()
    {
        Time.timeScale = 1;
        EventManager.Instance.onAddScore.AddListener(AddScore);
        EventManager.Instance.onNewGame.AddListener(Launch);
        EventManager.Instance.onEndGame.AddListener(Ending);
        baseScale = displayScore.transform.localScale;
        currentTimer = levelData.gameTimer;
        displayMinimumScore.text = "Minimum : " + levelData.minScore;
        displayTimer.text = currentTimer.ToString();
    }

    void Launch()
    {
        imageTimer.DOFillAmount(0, levelData.gameTimer);
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

        float tweenDuration = 0.8f;
        newText.transform.DOScale(baseNewScale * 2f, tweenDuration/2).SetUpdate(true);
        newText.transform.DOScale(baseNewScale * 0.3f, tweenDuration / 2).SetUpdate(true).SetDelay(tweenDuration / 2);
        Text _text = newText.GetComponentInChildren<Text>();
        _text.text = "+" + amount;
        _text.DOFade(0, tweenDuration/2).SetUpdate(true).SetDelay(tweenDuration / 2);
        Destroy(newText, tweenDuration);

        globalScore += (int)amount;
        displayScore.transform.DOKill(true);
        displayScore.text = globalScore.ToString();
        displayScore.transform.DOPunchScale(baseScale * 0.5f, 1f, 5, 0.4f).SetUpdate(true);

        if (displayScore.color.g != Color.green.g)
        {
            if (globalScore > levelData.minScore)
                displayScore.DOColor(Color.green, 1f);
            else
                displayScore.color = Color.red;
        }
    }

    public void Ending(bool win)
    {
        endText.GetComponent<Text>().DOFade(1, 2f).SetUpdate(true);
        fadingScreen.GetComponent<Image>().DOFade(0.75f, 2f).SetUpdate(true);
        int i = SceneManager.GetActiveScene().buildIndex;

        if (win)
        {
            endText.text = "You win !!";
            endText.color = Color.green;
            StartCoroutine(Load(i + 1, 5f));
        }
        else
        {
            endText.text = "Game Over !!";
            endText.color = Color.red;
            StartCoroutine(Load(i, 5f));
        }

        Time.timeScale = 0;
    }

    IEnumerator Load(int index, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        if (index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
    }

    public void RestartOrHome(bool restart)
    {
        SceneManager.LoadScene(restart ? SceneManager.GetActiveScene().buildIndex : 0);
    }

    void AddTimer()
    {
        currentTimer--;
        displayTimer.text = currentTimer.ToString();
        Vector3 baseScale = displayTimer.transform.localScale;
        float tweenDuration = 0.4f;
        displayTimer.transform.DOScale(baseScale * 1.2f, tweenDuration);
        displayTimer.transform.DOScale(baseScale, tweenDuration / 2).SetDelay(tweenDuration / 2);
        realTimer = 0;
    }

    void ManageTimer()
    {
        if (!started)
            return;

        realTimer += Time.deltaTime;
        if (realTimer > 1 && currentTimer > 0)
            AddTimer();

        if (currentTimer <= 0)
            EventManager.Instance.onEndGame.Invoke(globalScore > levelData.minScore);
    }

    private void Update()
    {
        ManageTimer();
    }
}
