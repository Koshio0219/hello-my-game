using Cysharp.Threading.Tasks;
using Game.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitletoGame : MonoBehaviour
{
    [SerializeField] private TitleSelectUI _FirstSelect;
    [SerializeField] private TitleSelectUI _SecondSelect;
    [SerializeField] private TitleSelectUI _ThirdSelect;
    [SerializeField] private GameObject loadingUI;
    private AsyncOperation async;
    private void Awake()
    {
    }

    private void Start()
    {

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
        if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.None) return;

        if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.Exit)
        {
            StartCoroutine("ExitGame");
        }
        else
        {
            // ロード画面を表示する
            loadingUI.SetActive(true);
            var sceneIndex = (int)_FirstSelect.DecideState;
            StartCoroutine(StageLoad());
            //SceneId nextScene = (SceneId)Enum.ToObject(typeof(SceneId), sceneIndex + 1);
            //SceneTransferManager.Instance.Load(nextScene);
        }
    }

    void ChoiceTwo() {
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.None || _SecondSelect.DecideState == TitleSelectUI.StateEnum.None) return;
        // 二人の選択が異なる場合
        if (_FirstSelect.DecideState != _SecondSelect.DecideState)
        {
            // 二人の選択を解除
            _FirstSelect.DeSelect();
            _SecondSelect.DeSelect();
            return;
        }
        if (_FirstSelect.DecideState == _SecondSelect.DecideState)
        {
            //SoundManager.Instance.Stop();
            //SoundManager.Instance.Play(SoundManager.SoundID.Decide02, 0.8f);
            if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.Exit)
            {
                StartCoroutine("ExitGame");
            }
            else
            {
                // ロード画面を表示する
                loadingUI.SetActive(true);
                var sceneIndex = (int)_FirstSelect.DecideState;
                StartCoroutine(StageLoad());
                //SceneId nextScene = (SceneId)Enum.ToObject(typeof(SceneId), sceneIndex + 1);
                //SceneTransferManager.Instance.Load(nextScene);
            }
        }
    }

    void ChoiceThree()
    {
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.None || _SecondSelect.DecideState == TitleSelectUI.StateEnum.None || _ThirdSelect.DecideState == TitleSelectUI.StateEnum.None) return;
        // 二人の選択が異なる場合

        if (_FirstSelect.DecideState == _SecondSelect.DecideState && _SecondSelect.DecideState == _ThirdSelect.DecideState)
        {
            //SoundManager.Instance.Stop();
            //SoundManager.Instance.Play(SoundManager.SoundID.Decide02, 0.8f);
            if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.Exit)
            {
                StartCoroutine("ExitGame");
            }
            else
            {
                // ロード画面を表示する
                loadingUI.SetActive(true);
                var sceneIndex = (int)_FirstSelect.DecideState;
                StartCoroutine(StageLoad());
                //SceneId nextScene = (SceneId)Enum.ToObject(typeof(SceneId), sceneIndex + 1);
                //SceneTransferManager.Instance.Load(nextScene);
            }
        } else
        {
            // 二人の選択を解除
            _FirstSelect.DeSelect();
            _SecondSelect.DeSelect();
            _ThirdSelect.DeSelect();
            return;
        }
    }

    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1.0f);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        #else
                Application.Quit();//ゲームプレイ終了
        #endif
    }

    private IEnumerator StageLoad()
    {
        if(async!=null && !async.isDone)
        {
            yield break;
        }

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
