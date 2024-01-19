using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Manager;
using Game.Data;

namespace Game.Hud
{  
    public class StagePreView : HudView
    {
        public TextMeshProUGUI guideText;

        public override void OnShow()
        {
            SetGuide();
            base.OnShow();
        }

        public void SetGuide()
        {
            string text1 = $"ステージ {GameManager.Instance.LevelIdx + 1}  開幕説明：\n\n";
            var text2 = $"目標ポイント：{GameData.Instance.LevelConfig.levelDatas[GameManager.Instance.LevelIdx].goalPoint} p\n";
            var text3 = $"限定時間：{GameData.Instance.LevelConfig.levelDatas[GameManager.Instance.LevelIdx].showTime} s\n\n";
            var text4 = "限定時間内で目標ポイントをもらって\n出口を探しましょう！";

            guideText.text = text1 + text2 + text3 + text4;
        }
    }
}

