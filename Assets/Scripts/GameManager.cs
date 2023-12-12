using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScene
{
    Main = 0,
    Level1 = 1,
    Level2 = 2,

    Nothing,
}

public sealed class GameManager : SingletonMono<GameManager>
{
    public event Action UpdateHandle;
    public event Action<float> TimeUpdateHandle;

    private GameScene levelScene = GameScene.Nothing;

    protected override void OnAwake()
    {
        Application.targetFrameRate = 60;
        DataManager.Instance.LoadDatas();
    }

    private void Start()
    {
        UIManager.Instance.Open<MainForm>();
    }

    private void Update()
    {
        UpdateHandle?.Invoke();
        TimeUpdateHandle?.Invoke(Time.deltaTime);
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene(GameScene.Level1));
    }

    IEnumerator LoadScene(GameScene gameScene)
    {
        AsyncOperation asyncOperation;
        if (levelScene != GameScene.Nothing)
        {
            CharacterManager.Instance.ClearEnemies();
            asyncOperation = SceneManager.UnloadSceneAsync((int)levelScene);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
        else
        {
            CharacterManager.Instance.CreatePlayerEntity();
        }
        UIManager.Instance.Close<MainForm>();
        asyncOperation = SceneManager.LoadSceneAsync((int)gameScene, LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        Scene scene = SceneManager.GetSceneAt((int)gameScene);
        GameObject[] objs = scene.GetRootGameObjects();
        List<Character> characters = new List<Character>();
        for (int i = -1; ++i < objs.Length;)
        {
            Character character = objs[i].GetComponent<Character>();
            if (character)
            {
                characters.Add(character);
            }
        }
        CharacterManager.Instance.GetEnemiesOnSceneLoaded(characters);
        levelScene = gameScene;
    }
}