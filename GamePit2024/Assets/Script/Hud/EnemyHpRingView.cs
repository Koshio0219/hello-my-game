using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Hud
{
    public class EnemyHpRingView : MonoBehaviour
    {
        [SerializeField] private Image ring_mod;
        [SerializeField] private Image ring_hp;
        [SerializeField] private TextMeshProUGUI text_hp;

        private float maxHp;

        public void InitHpView(float hp)
        {
            text_hp.text = hp.ToString();
            ring_hp.fillAmount = 1f;
            ring_mod.fillAmount = 1f;
            maxHp = hp;
        }

        public void UpdateHpView(float lastHp, float nowHp, float changeTime = .3f)
        {
            if (lastHp > nowHp)
                HpDown(nowHp,changeTime);
            else if (lastHp < nowHp)
                HpUp(nowHp);
        }

        private void HpUp(float nowHp)
        {
            text_hp.text = nowHp.ToString();
            ring_hp.fillAmount = nowHp * 1.0f / maxHp;
            ring_mod.fillAmount = ring_hp.fillAmount;
        }

        private void HpDown(float nowHp, float changeTime)
        {
            text_hp.text = nowHp.ToString();
            ring_hp.fillAmount = nowHp * 1.0f / maxHp;
            DOTween.To(() => ring_mod.fillAmount, (x) => ring_mod.fillAmount = x, ring_hp.fillAmount, changeTime);
        }
    }
}