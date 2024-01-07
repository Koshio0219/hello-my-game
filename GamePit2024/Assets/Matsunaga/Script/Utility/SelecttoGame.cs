using Cysharp.Threading.Tasks;
using Game.Data;
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
    private bool isLoading = false;
    private void Awake()
    {
    }

    private void Start()
    {
        UICanvas.SetActive(true);
        loadingUI.SetActive(false);
    }
    void Update()
    {
        if (isLoading)
            return;
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

        switch (_FirstSelect.DecideState)
        {
            case CharaSelectUI.StateEnum.Directer:
                GameData.Instance.PlayerParameter.GamepadNumber_D = (int)TitleSelectUI.PlayerType.First;
                break;
            case CharaSelectUI.StateEnum.Melee:
                GameData.Instance.PlayerParameter.GamepadNumber_M = (int)TitleSelectUI.PlayerType.First;
                break;
            case CharaSelectUI.StateEnum.LongRange:
                GameData.Instance.PlayerParameter.GamepadNumber_L = (int)TitleSelectUI.PlayerType.First;
                break;
        }
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
            switch (_FirstSelect.DecideState)
            {
                case CharaSelectUI.StateEnum.Directer:
                    GameData.Instance.PlayerParameter.GamepadNumber_D = (int)TitleSelectUI.PlayerType.First;
                    break;
                case CharaSelectUI.StateEnum.Melee:
                    GameData.Instance.PlayerParameter.GamepadNumber_M = (int)TitleSelectUI.PlayerType.First;
                    break;
                case CharaSelectUI.StateEnum.LongRange:
                    GameData.Instance.PlayerParameter.GamepadNumber_L = (int)TitleSelectUI.PlayerType.First;
                    break;
            }
            switch (_SecondSelect.DecideState)
            {
                case CharaSelectUI.StateEnum.Directer:
                    GameData.Instance.PlayerParameter.GamepadNumber_D = (int)TitleSelectUI.PlayerType.Second;
                    break;
                case CharaSelectUI.StateEnum.Melee:
                    GameData.Instance.PlayerParameter.GamepadNumber_M = (int)TitleSelectUI.PlayerType.Second;
                    break;
                case CharaSelectUI.StateEnum.LongRange:
                    GameData.Instance.PlayerParameter.GamepadNumber_L = (int)TitleSelectUI.PlayerType.Second;
                    break;
            }
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
            switch (_FirstSelect.DecideState)
            {
                case CharaSelectUI.StateEnum.Directer:
                    GameData.Instance.PlayerParameter.GamepadNumber_D = (int)TitleSelectUI.PlayerType.First;
                    break;
                case CharaSelectUI.StateEnum.Melee:
                    GameData.Instance.PlayerParameter.GamepadNumber_M = (int)TitleSelectUI.PlayerType.First;
                    break;
                case CharaSelectUI.StateEnum.LongRange:
                    GameData.Instance.PlayerParameter.GamepadNumber_L = (int)TitleSelectUI.PlayerType.First;
                    break;
            }
            switch (_SecondSelect.DecideState)
            {
                case CharaSelectUI.StateEnum.Directer:
                    GameData.Instance.PlayerParameter.GamepadNumber_D = (int)TitleSelectUI.PlayerType.Second;
                    break;
                case CharaSelectUI.StateEnum.Melee:
                    GameData.Instance.PlayerParameter.GamepadNumber_M = (int)TitleSelectUI.PlayerType.Second;
                    break;
                case CharaSelectUI.StateEnum.LongRange:
                    GameData.Instance.PlayerParameter.GamepadNumber_L = (int)TitleSelectUI.PlayerType.Second;
                    break;
            }
            switch (_ThirdSelect.DecideState)
            {
                case CharaSelectUI.StateEnum.Directer:
                    GameData.Instance.PlayerParameter.GamepadNumber_D = (int)TitleSelectUI.PlayerType.Third;
                    break;
                case CharaSelectUI.StateEnum.Melee:
                    GameData.Instance.PlayerParameter.GamepadNumber_M = (int)TitleSelectUI.PlayerType.Third;
                    break;
                case CharaSelectUI.StateEnum.LongRange:
                    GameData.Instance.PlayerParameter.GamepadNumber_L = (int)TitleSelectUI.PlayerType.Third;
                    break;
            }
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
        if (isLoading)
            yield break ;
        isLoading = true;

        UICanvas.SetActive(false);
        while (CubeChange.transform.eulerAngles.y <= 90.0f)
        {
            CubeChange.transform.Rotate(0, 0.3f, 0f, Space.World);
            yield return null;
        }
        loadingUI.SetActive(true);
        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync("Stage");

        // ロードが完了するまで待機する
        yield return new WaitUntil(() => async.isDone == true);

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
        isLoading = false;
    }
}
