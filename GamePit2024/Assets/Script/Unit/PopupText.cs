using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Game.Framework;
using Unity.VisualScripting;

namespace Game.Unit
{
    public class PopupText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float lifeTime = 1f;

        public void Setup(int num ,Color color)
        {
            if (text == null) return;
            text.color = color;
            text.text = num.ToString();

            WaitRecycle().Forget();
            Move();
            Fade();
        }

        private void Move()
        {
            transform.DOMoveY(transform.position.y + 0.6f, lifeTime).SetEase(Ease.InOutQuad);
        }

        private async UniTask WaitRecycle()
        {
            await UniTask.Delay((int)(lifeTime * 1000));
            GameObjectPool.Instance.RecycleObj(gameObject);
        }

        private void Fade()
        {
            var par = text.transform.parent;
            var cg= par.gameObject.GetOrAddComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.DOFade(1f, lifeTime);
        }
    }
}

