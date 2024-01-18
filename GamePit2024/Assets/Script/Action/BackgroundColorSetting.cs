using DG.Tweening;
using Game.Data;
using Game.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{
    public class BackgroundColorSetting : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer.color = GameData.Instance.LevelConfig.levelDatas[GameManager.Instance.LevelIdx].backgroundColor;
            spriteRenderer.DOFade(.5f, 1).SetLoops(-1, LoopType.Yoyo);
        }
    }
}

