using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Framework;

public class SoundManager : MonoSingleton<SoundManager>
{
    /// <summary>
    /// エフェクトID ( 手動で追加する)
    /// </summary>
    public enum SoundID
    {
        None = -1,
        MeleeAttack,
        MeleeDefense,
        LongRangeShot,
        LighterRangeShotCharge,
        ToPlayerDamage,
        Dead,
        EnemyAttack,
        EnemyBulletAttack,
        ToEnemyDamage,
        EnemyDead,
        Decide01,
        Decide02,
        Decide03,
        Cancel,
        StageClear,
    }

    /// <summary>
    /// エフェクトリスト
    /// </summary>
    [SerializeField]
    List<AudioClip> _SoundList = new List<AudioClip>();

    private AudioSource _AudioSource;
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// エフェクト再生
    /// </summary>
    /// <param name="id">エフェクトID</param>
    /// <returns></returns>
    public void Play(SoundID id, float volume)
    {
        if (id == SoundID.None) return;
        var index = (int)id;
        var clip = _SoundList[index];
        if (index < 0 || _SoundList.Count <= index) return;
        if (clip == null) return;
        _AudioSource.volume = volume;
        _AudioSource.PlayOneShot(clip);
    }

    public void Stop()
    {
        if (_AudioSource != null) _AudioSource.Stop();
    }

#if UNITY_EDITOR
    [SerializeField]
    private SoundID _TestSoundID = SoundID.None;

    [ContextMenu("TestPlay")]
    private void TestPlay()
    {
        Play(_TestSoundID, 1f);
    }
#endif
}
