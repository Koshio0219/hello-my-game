using Game.Action;
using Game.Base;
using Game.Data;
using Game.Framework;
using Game.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Unit
{
    public class PlayerCreateBlock : BlockBase
    {
        public Transform player1Point;
        public Transform player2Point;

        private void Awake()
        {
            EventQueueSystem.AddListener<StageStatesEvent>(StageStatesHnadler);
        }

        private void StageStatesHnadler(StageStatesEvent e)
        {
            if (e.to != StageStates.CurtainInputEnd) return;

            var tempMap = new Dictionary<CharacterType, GameObject>();
            GameData.Instance.SelectedCharacters.ForEach(one =>
            {
                switch (one)
                {
                    case CharacterType.Melee:
                        var temp1 = Instantiate(GameData.Instance.PlayerParameter.prefab_M);
                        temp1.transform.position = player1Point.position;
                        tempMap.Add(one, temp1);
                        break;
                    case CharacterType.LongRange:
                        var temp2 = Instantiate(GameData.Instance.PlayerParameter.prefab_L);
                        temp2.transform.position = player1Point.position;
                        tempMap.Add(one, temp2);
                        break;
                }
            });
            EventQueueSystem.RemoveListener<StageStatesEvent>(StageStatesHnadler);

            if (tempMap.Count == 0) return;
            var temp3 = tempMap.Values.ToList()[0];
            var temp4 = temp3.AddComponent<ScrollerSender>();
            temp4.speed = .5f;
        }
    }
}

