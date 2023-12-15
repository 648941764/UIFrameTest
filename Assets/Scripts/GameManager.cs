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

    private Transform[] cameraRestrictTrans = new Transform[2];

    public Vector3 RestrictLeft => cameraRestrictTrans[0].position;
    public Vector3 RestrictRight => cameraRestrictTrans[1].position;

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
        CharacterManager.Instance.CreatePlayerEntity();
        SwithScene(GameScene.Level1);
    }

    IEnumerator SwitchScene(GameScene gameScene)
    {
        UpdateHandle -= DropManager.Instance.OnUpdatePick;
        AsyncOperation asyncOperation;
        CharacterManager.Instance.ClearEnemies();
        if (levelScene != GameScene.Nothing)
        {
            asyncOperation = SceneManager.UnloadSceneAsync((int)levelScene);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }

        levelScene = gameScene;
        if (gameScene == GameScene.Nothing)
        {
            yield break;
        }
        
        asyncOperation = SceneManager.LoadSceneAsync((int)gameScene, LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        UIManager.Instance.Close<MainForm>();
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
            if (objs[i].name == "CamerSportPoint")
            {
                cameraRestrictTrans[0] = objs[i].transform.GetChild(0);
                cameraRestrictTrans[1] = objs[i].transform.GetChild(1);
            }
        }
        CharacterManager.Instance.CreateEnemiesOnSceneLoaded(characters);
        UpdateHandle += DropManager.Instance.OnUpdatePick;
    }

    public void SwithScene(GameScene gameScene)
    {
        StartCoroutine(SwitchScene(gameScene));
    }
}