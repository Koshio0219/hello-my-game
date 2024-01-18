using Cysharp.Threading.Tasks;
using Game.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KanKikuchi.AudioManager;

public class SettingToTitle : MonoBehaviour
{
    public enum PlayerType
    {
        First = 0,
        Second = 1,
        Third = 2,
    }
    [SerializeField] private SettingSelectUI _FirstSelect;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject RotateSoundImage;
    [SerializeField] private PlayerType _PlayerType;
    private AsyncOperation async;
    private bool isDecide = false;
    private void Awake()
    {
    }

    private void Start()
    {
        BGMSlider.value = BGMManager.Instance.GetBaseVolume() * (BGMSlider.minValue + BGMSlider.maxValue) * 0.5f;
        SESlider.value = SEManager.Instance.GetBaseVolume() * (SESlider.minValue + SESlider.maxValue) * 0.5f;
        UICanvas.SetActive(true);
        loadingUI.SetActive(false);
        isDecide = false;
    }
    void Update()
    {

        if (isDecide) return;
        ChoiceOne();
    }

    void ChoiceOne()
    {
        var playerNum = (int)_PlayerType;
        // どちらかが未選択なら処理しない
        if (_FirstSelect.DecideState == SettingSelectUI.StateEnum.None) return;

        isDecide = true;
        switch (_FirstSelect.DecideState)
        {
            case SettingSelectUI.StateEnum.BGM:
                // 上を押すとはじめるを選択
                if (Gamepad.all[playerNum].dpad.right.wasPressedThisFrame)
                {
                    BGMSlider.value = (BGMSlider.value + 1 > BGMSlider.maxValue) ? BGMSlider.maxValue : BGMSlider.value + 1;
                    BGMManager.Instance.ChangeBaseVolume((BGMSlider.value * 1.0f) / ((BGMSlider.maxValue + BGMSlider.minValue) * 0.5f));
                }
                // 下を押すとチュートリアルを選択
                if (Gamepad.all[playerNum].dpad.left.wasPressedThisFrame)
                {
                    BGMSlider.value = (BGMSlider.value - 1 < BGMSlider.minValue) ? BGMSlider.minValue : BGMSlider.value - 1;
                    BGMManager.Instance.ChangeBaseVolume((BGMSlider.value * 1.0f) / ((BGMSlider.maxValue + BGMSlider.minValue) * 0.5f));
                }
                isDecide = false;
                break;
            case SettingSelectUI.StateEnum.SE:
                // 上を押すとはじめるを選択
                if (Gamepad.all[playerNum].dpad.right.wasPressedThisFrame)
                {
                    SESlider.value = (SESlider.value + 1 > SESlider.maxValue) ? SESlider.maxValue : SESlider.value + 1;
                    SEManager.Instance.ChangeBaseVolume((SESlider.value * 1.0f) / ((SESlider.maxValue + SESlider.minValue) * 0.5f));
                    SEManager.Instance.Play(SEPath.DECIDE01);
                }
                // 下を押すとチュートリアルを選択
                if (Gamepad.all[playerNum].dpad.left.wasPressedThisFrame)
                {
                    SESlider.value = (SESlider.value - 1 < SESlider.minValue) ? SESlider.minValue : SESlider.value - 1;
                    SEManager.Instance.ChangeBaseVolume((SESlider.value * 1.0f) / ((SESlider.maxValue + SESlider.minValue) * 0.5f));
                    SEManager.Instance.Play(SEPath.DECIDE01);
                }
                isDecide = false;
                break;
            case SettingSelectUI.StateEnum.Exit:
                UICanvas.SetActive(false);
                isDecide = true;
                StartCoroutine(StartLoad());
                break;
        }
    }

    private IEnumerator StartLoad()
    {
        BGMSwitcher.FadeOutAndFadeIn(BGMPath.START);
        while (RotateSoundImage.transform.eulerAngles.y <= 90.0f)
        {
            RotateSoundImage.transform.Rotate(0, 0.5f, 0f, Space.World);
            yield return null;
        }
        // シーンを非同期でロードする
        async = SceneManager.LoadSceneAsync("Start");

        yield return new WaitUntil(() => async.isDone == true);

        // ロード画面を非表示にする
        loadingUI.SetActive(false);
    }
}
