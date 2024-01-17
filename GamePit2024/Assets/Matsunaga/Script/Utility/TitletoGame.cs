using Cysharp.Threading.Tasks;
using Game.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class TitletoGame : MonoBehaviour
{
    [SerializeField] private TitleSelectUI _FirstSelect;
    [SerializeField] private TitleSelectUI _SecondSelect;
    [SerializeField] private TitleSelectUI _ThirdSelect;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject RotateSoundImage;
    [SerializeField] private GameObject RotateImageL;
    [SerializeField] private GameObject RotateImageR;
    private AsyncOperation async;
    private bool isDecide = false;
    private void Awake()
    {
    }

    private void Start()
    {
        BGMManager.Instance.Play(BGMPath.START);
        UICanvas.SetActive(true);
        loadingUI.SetActive(false);
        isDecide = false;
    }
    void Update()
    {
        if (isDecide) return;
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

        isDecide = true;
        switch (_FirstSelect.DecideState)
        {
            case TitleSelectUI.StateEnum.Start:
                loadingUI.SetActive(true);
                UICanvas.SetActive(false);
                isDecide = true;
                StartCoroutine(StageLoad());
                break;
            case TitleSelectUI.StateEnum.Setting:
                //ChangeActive((int)StateEnum.Setting);
                StartCoroutine(SettingLoad());
                UICanvas.SetActive(false);
                isDecide = true;
                break;
            case TitleSelectUI.StateEnum.HomePage:
                Game.Manager.GameManager.Instance.OpenHomePage();
                isDecide = false;
                _FirstSelect.DeSelect();
                break;
            case TitleSelectUI.StateEnum.Exit:
                StartCoroutine("ExitGame");
                break;
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
        isDecide = true;
        switch (_FirstSelect.DecideState)
        {
            case TitleSelectUI.StateEnum.Start:
                loadingUI.SetActive(true);
                UICanvas.SetActive(false);
                isDecide = true;
                StartCoroutine(StageLoad());
                break;
            case TitleSelectUI.StateEnum.Setting:
                StartCoroutine(SettingLoad());
                UICanvas.SetActive(false);
                isDecide = true;
                break;
            case TitleSelectUI.StateEnum.HomePage:
                Game.Manager.GameManager.Instance.OpenHomePage();
                isDecide = false;
                _FirstSelect.DeSelect();
                _SecondSelect.DeSelect();
                break;
            case TitleSelectUI.StateEnum.Exit:
                StartCoroutine("ExitGame");
                break;
        }
    }

    void ChoiceThree()
    {
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.None || _SecondSelect.DecideState == TitleSelectUI.StateEnum.None || _ThirdSelect.DecideState == TitleSelectUI.StateEnum.None) return;
        // 二人の選択が異なる場合

        if (_FirstSelect.DecideState == _SecondSelect.DecideState && _SecondSelect.DecideState == _ThirdSelect.DecideState)
        {
            isDecide = true;
            switch (_FirstSelect.DecideState)
            {
                case TitleSelectUI.StateEnum.Start:
                    loadingUI.SetActive(true);
                    UICanvas.SetActive(false);
                    isDecide = true;
                    StartCoroutine(StageLoad());
                    break;
                case TitleSelectUI.StateEnum.Setting:
                    StartCoroutine(SettingLoad());
                    UICanvas.SetActive(false);
                    isDecide = true;
                    break;
                case TitleSelectUI.StateEnum.HomePage:
                    Game.Manager.GameManager.Instance.OpenHomePage();
                    isDecide = false;
                    _FirstSelect.DeSelect();
                    _SecondSelect.DeSelect();
                    _ThirdSelect.DeSelect();
                    break;
                case TitleSelectUI.StateEnum.Exit:
                    StartCoroutine("ExitGame");
                    break;
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
        /*if(async!=null && !async.isDone)
        {
            yield break;
        }*/

        while (RotateImageL.transform.eulerAngles.y <= 90.0f)
        {
            RotateImageL.transform.Rotate(0, 0.3f, 0f, Space.World);
            RotateImageR.transform.Rotate(0, -0.3f, 0f, Space.World);
            yield return null;
        }
        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync("CharaSelect");

        yield return new WaitUntil(() => async.isDone == true);

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }

    private IEnumerator SettingLoad()
    {
        /*if (async != null && !async.isDone)
        {
            yield break;
        }*/
        while (RotateSoundImage.transform.eulerAngles.y >= 0.2f)
        {
            RotateSoundImage.transform.Rotate(0, -0.5f, 0f, Space.World);
                yield return null;
        }
        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync("Setting");

        yield return new WaitUntil(() => async.isDone == true);
        // ロードが完了するまで待機する
        /*while (!async.isDone)
        {
            yield return null;
        }*/

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }
}
