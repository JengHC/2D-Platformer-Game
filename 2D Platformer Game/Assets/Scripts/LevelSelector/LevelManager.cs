using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelInfo
{
    public string LevelName;
    public Sprite LevelThumb;
    public GameObject LevelPrefab;
}

public class LevelManager : MonoBehaviour
{
    public List<LevelInfo> levels;
    public GameObject SelectedPrefab;

    private static LevelManager instance;
    public static LevelManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartLevel(int index)
    {
        SelectedPrefab = levels[index].LevelPrefab;
        SceneManager.LoadScene("GameScene");
    }
}