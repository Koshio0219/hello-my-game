using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CharaSelectUI : MonoBehaviour
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
        Directer,
        Melee,
        LongRange,
    }
    [SerializeField] private PlayerType _PlayerType;
    [SerializeField] private List<GameObject> _ParList = new List<GameObject>();
    [SerializeField] private float _scale = 1.0f;

    private StateEnum _State = StateEnum.Directer;
    public StateEnum DecideState = StateEnum.None;
    void Start()
    {
        ChangeState(StateEnum.Directer);
    }
    void Update()
    {
        var playerNum = (int)_PlayerType;
        // 奩摉偺僎乕儉僷僢僪偑愙懕偝傟偰偄側偄偲摦偐側偄
        if (Gamepad.all.Count < playerNum + 1)
        {
            if (_ParList == null) return;
            foreach (GameObject obj in _ParList)
            {
                obj.SetActive(false);
            }
            return;
        }
        // 壗傕慖傫偱偄側偄帪
        if (DecideState == StateEnum.None)
        {
            // 忋傪墴偡偲偼偠傔傞傪慖戰
            if (Gamepad.all[playerNum].dpad.up.wasPressedThisFrame)
            {
                ChangeState((StateEnum)Enum.ToObject(typeof(StateEnum), ((int)_State + 2) % 3));
            }
            // 壓傪墴偡偲僠儏乕僩儕傾儖傪慖戰
            if (Gamepad.all[playerNum].dpad.down.wasPressedThisFrame)
            {
                ChangeState((StateEnum)Enum.ToObject(typeof(StateEnum), ((int)_State + 1) % 3));
            }
            /*// 忋傪墴偡偲偼偠傔傞傪慖戰
            if (Gamepad.all[playerNum].dpad.up.wasPressedThisFrame)
            {
                ChangeState(StateEnum.Start);
            }
            // 壓傪墴偡偲僠儏乕僩儕傾儖傪慖戰
            if (Gamepad.all[playerNum].dpad.down.wasPressedThisFrame)
            {
                ChangeState(StateEnum.Tutorial);
            }*/
            // 仜傪墴偡偲慖戰寛掕
            if (Gamepad.all[playerNum].buttonEast.wasPressedThisFrame)
            {
                //SoundManager.Instance.Play(SoundManager.SoundID.Decide01, 0.8f);
                DecideState = _State;
                var index = (int)_State;
                _ParList[index].GetComponent<RectTransform>().localScale = new Vector3(2 * _scale, 2 * _scale, 2 * _scale);
                // Debug.Log(DecideState);
            }
        }
        else
        {
            // 亊傪墴偡偲慖戰夝彍
            if (Gamepad.all[playerNum].buttonSouth.wasPressedThisFrame)
            {
                //SoundManager.Instance.Play(SoundManager.SoundID.Cancel, 0.5f);
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
            case StateEnum.Directer:
                ChangeActive((int)StateEnum.Directer);
                break;
            case StateEnum.Melee:
                ChangeActive((int)StateEnum.Melee);
                break;
            case StateEnum.LongRange:
                ChangeActive((int)StateEnum.LongRange);
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
