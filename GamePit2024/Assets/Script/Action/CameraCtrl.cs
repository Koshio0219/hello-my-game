using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Data;
using Game.Framework;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Action
{
    public class CameraCtrl : MonoBehaviour
    {
        public float speed = 1f;
        private int inputIdx;
        private bool inputable = false;

        async void Awake()
        {
            var data = await GameData.Instance.GetPlayerParameter();
            inputIdx = data.GamepadNumber_D;
            inputable = false;
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHandler);
        }

        private void OnDestroy()
        {
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHandler);
        }

        private void StageStatesHandler(StageStatesEvent e)
        {
            if (e.to == StageStates.BattleStarted) 
                inputable = true;
            if (e.to == StageStates.BattleClear || e.to == StageStates.GameOver)
                inputable = false;
        }

        private void LateUpdate()
        {
            if (!inputable) return;
            if (Gamepad.all.Count > 0 && inputIdx >= 0 && Gamepad.all[inputIdx] != null)
            {
                var input = Gamepad.all[inputIdx].rightStick.ReadValue().x;
                transform.Translate(new Vector3(input * speed * Time.deltaTime, 0, 0));
            }
        }
    }
}

