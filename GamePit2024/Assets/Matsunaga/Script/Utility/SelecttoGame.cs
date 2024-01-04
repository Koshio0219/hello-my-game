using Cysharp.Threading.Tasks;
using Game.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelecttoGame : MonoBehaviour
{
    [SerializeField] private CharaSelectUI _FirstSelect;
    [SerializeField] private CharaSelectUI _SecondSelect;
    [SerializeField] private CharaSelectUI _ThirdSelect;
    [SerializeField] private GameObject CubeChange;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject UICanvas;
    private AsyncOperation async;
    private void Awake()
    {
    }

    private void Start()
    {
        UICanvas.SetActive(true);
    }
    void Update()
    {
        switch (Gamepad.all.Count)
        {
            case 0:
                break;
            case 1:
                ChoiceOne();
                break;
            case 2:
                ChoiceTwo();
                break;
            case 3:
                ChoiceThree();
                break;
            default:
                break;
        }
    }

    void ChoiceOne()
    {
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == CharaSelectUI.StateEnum.None) return;

        // ロード画面を表示する

        var sceneIndex = (int)_FirstSelect.DecideState;
        StartCoroutine(CubeChanger());
    }

    void ChoiceTwo()
    {
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == CharaSelectUI.StateEnum.None || _SecondSelect.DecideState == CharaSelectUI.StateEnum.None) return;
        // 二人の選択が異なる場合
        if (_FirstSelect.DecideState != _SecondSelect.DecideState)
        {
            // ロード画面を表示する
           // loadingUI.SetActive(true);
            var sceneIndex = (int)_FirstSelect.DecideState;
            StartCoroutine(CubeChanger());
            //SceneId nextScene = (SceneId)Enum.ToObject(typeof(SceneId), sceneIndex + 1);
            //SceneTransferManager.Instance.Load(nextScene);
        }
        if (_FirstSelect.DecideState == _SecondSelect.DecideState)
        {
            //SoundManager.Instance.Stop();
            //SoundManager.Instance.Play(SoundManager.SoundID.Decide02, 0.8f);
            // 二人の選択を解除
            _FirstSelect.DeSelect();
            _SecondSelect.DeSelect();
            return;
        }
    }

    void ChoiceThree()
    {
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == CharaSelectUI.StateEnum.None || _SecondSelect.DecideState == CharaSelectUI.StateEnum.None || _ThirdSelect.DecideState == CharaSelectUI.StateEnum.None) return;
        // 二人の選択が異なる場合

        if (_FirstSelect.DecideState != _SecondSelect.DecideState && _SecondSelect.DecideState != _ThirdSelect.DecideState)
        {
            //SoundManager.Instance.Stop();
            //SoundManager.Instance.Play(SoundManager.SoundID.Decide02, 0.8f);
            // ロード画面を表示する
            //loadingUI.SetActive(true);
            var sceneIndex = (int)_FirstSelect.DecideState;
            StartCoroutine(CubeChanger());
            //SceneId nextScene = (SceneId)Enum.ToObject(typeof(SceneId), sceneIndex + 1);
            //SceneTransferManager.Instance.Load(nextScene);
        }
        else
        {
            // 二人の選択を解除
            _FirstSelect.DeSelect();
            _SecondSelect.DeSelect();
            _ThirdSelect.DeSelect();
            return;
        }
    }

    private IEnumerator StageLoad()
    {

        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync("Stage");

        // ロードが完了するまで待機する
        while (!async.isDone)
        {
            yield return null;
        }

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }

    private IEnumerator CubeChanger()
    {
        UICanvas.SetActive(false);
        while (CubeChange.transform.eulerAngles.y <= 90.0f)
        {
            CubeChange.transform.Rotate(0, 0.01f, 0f, Space.World);
            yield return null;
        }
       
        loadingUI.SetActive(true);
        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync("Stage");

        // ロードが完了するまで待機する
        while (!async.isDone)
        {
            yield return null;
        }

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }
}
