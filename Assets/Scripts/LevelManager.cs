using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class LevelData
{
    [SerializeField] string dataName = "Level1";
    public Color color = Color.white;
    [Range(0, 60)]
    public int gameTimer = 50;
    [Range(0, 5000)]
    public int minScore = 150;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] Image banner;
    [SerializeField] LevelData[] levelDatas = new LevelData[5];
    public LevelData SceneInfo => levelDatas[SceneManager.GetActiveScene().buildIndex - 1];

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        banner.color = levelDatas[SceneManager.GetActiveScene().buildIndex - 1].color;
    }
}
