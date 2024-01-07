using Cysharp.Threading.Tasks;
using Game.Loader;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SettingToTitle : MonoBehaviour
{
    [SerializeField] private TitleSelectUI _FirstSelect;
    [SerializeField] private TitleSelectUI _SecondSelect;
    [SerializeField] private TitleSelectUI _ThirdSelect;
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject RotateSoundImage;
    private AsyncOperation async;
    private bool isDecide = false;
    private void Awake()
    {
    }

    private void Start()
    {
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
        // �ǂ��炩�����I���Ȃ珈�����Ȃ�
        if (_FirstSelect.DecideState == TitleSelectUI.StateEnum.None) return;

        isDecide = true;
        switch (_FirstSelect.DecideState)
        {
            case TitleSelectUI.StateEnum.Start:
                loadingUI.SetActive(true);
                UICanvas.SetActive(false);
                isDecide = true;
                //StartCoroutine(StageLoad());
                break;
            case TitleSelectUI.StateEnum.Setting:
                //ChangeActive((int)StateEnum.Setting);
                //StartCoroutine(SettingLoad());
                UICanvas.SetActive(false);
                isDecide = true;
                break;
            case TitleSelectUI.StateEnum.HomePage:
                Game.Manager.GameManager.Instance.OpenHomePage();
                isDecide = false;
                _FirstSelect.DeSelect();
                break;
            case TitleSelectUI.StateEnum.Exit:
                UICanvas.SetActive(false);
                isDecide = true;
                StartCoroutine(StartLoad());
                break;
        }
    }

    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1.0f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
                Application.Quit();//�Q�[���v���C�I��
#endif
    }

    private IEnumerator StartLoad()
    {

        while (RotateSoundImage.transform.eulerAngles.y <= 90.0f)
        {
            RotateSoundImage.transform.Rotate(0, 0.5f, 0f, Space.World);
            yield return null;
        }
        // �V�[����񓯊��Ń��[�h����
        async = SceneManager.LoadSceneAsync("Start");

        yield return new WaitUntil(() => async.isDone == true);

        // ���[�h��ʂ��\���ɂ���
        loadingUI.SetActive(false);
    }
}
