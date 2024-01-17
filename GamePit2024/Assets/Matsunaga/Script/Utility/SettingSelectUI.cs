using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using KanKikuchi.AudioManager;


public class SettingSelectUI : MonoBehaviour
{
    public enum PlayerType
    {
        First = 0,
        Second = 1,
        Third = 2,
    }
    public enum StateEnum
    {
        None = -1,
        BGM,
        SE,
        Exit,
    }

    [SerializeField] private PlayerType _PlayerType;
    [SerializeField] private List<GameObject> _ParList = new List<GameObject>();
    [SerializeField] private float _scale = 1.0f;

    private StateEnum _State = StateEnum.BGM;
    public StateEnum DecideState = StateEnum.None;
    void Start()
    {
        ChangeState(StateEnum.BGM);
    }

    void Update()
    {
        var playerNum = (int)_PlayerType;
        // 該当のゲームパッドが接続されていないと動かない
        if (Gamepad.all.Count < playerNum + 1)
        {
            if (_ParList == null) return;
            foreach (GameObject obj in _ParList)
            {
                obj.SetActive(false);
            }
            return;
        }
        // 何も選んでいない時
        if (DecideState == StateEnum.None)
        {
            // 上を押すとはじめるを選択
            if (Gamepad.all[playerNum].dpad.up.wasPressedThisFrame)
            {
                ChangeState((StateEnum)Enum.ToObject(typeof(StateEnum), ((int)_State + 2) % 3));
            }
            // 下を押すとチュートリアルを選択
            if (Gamepad.all[playerNum].dpad.down.wasPressedThisFrame)
            {
                ChangeState((StateEnum)Enum.ToObject(typeof(StateEnum), ((int)_State + 1) % 3));
            }

            // ○を押すと選択決定
            if (Gamepad.all[playerNum].buttonEast.wasPressedThisFrame)
            {
                SEManager.Instance.Play(SEPath.DECIDE01);
                DecideState = _State;
                var index = (int)_State;
                _ParList[index].GetComponent<RectTransform>().localScale = new Vector3(2 * _scale, 2 * _scale, 2 * _scale);
                // Debug.Log(DecideState);
            }
        }
        else
        {
            // ×を押すと選択解除
            if (Gamepad.all[playerNum].buttonSouth.wasPressedThisFrame)
            {
                SEManager.Instance.Play(SEPath.CANCEL);
                DecideState = StateEnum.None;
                var index = (int)_State;
                _ParList[index].GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, _scale);
            }
        }
    }

    private void ChangeState(StateEnum next)
    {
        if (next == StateEnum.None) return;
        var prev = _State;
        _State = next;
        // Log.Info(GetType(), "ChangeState {0} -> {1}", prev, next);
        switch (_State)
        {
            case StateEnum.BGM:
                ChangeActive((int)StateEnum.BGM);
                break;
            case StateEnum.SE:
                ChangeActive((int)StateEnum.SE);
                break;
            case StateEnum.Exit:
                ChangeActive((int)StateEnum.Exit);
                break;
        }
    }

    private void ChangeActive(int nextId)
    {
        if (_ParList == null || _ParList.Count <= nextId || nextId == -1) return;
        foreach (GameObject obj in _ParList)
        {
            obj.SetActive(false);
        }
        if (_ParList[nextId] == null) return;
        _ParList[nextId].SetActive(true);
    }

    public void DeSelect()
    {
        DecideState = StateEnum.None;
        var index = (int)_State;
        _ParList[index].GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, _scale);
    }
}
