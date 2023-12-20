using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 時間を測るクラス
/// 使用例
/// class Test : MonoBehaviour
/// {
///		GameTimer _Timer = new GameTimer(1.0f);
///		
///		void Update()
///		{
///			_Timer.UpdateTimer();
///			if(_Timer.IsTimeup)
///			{
///				// 1秒ごとに呼ばれる処理
///				_Timer.ResetTimer();
///			}
///		}
/// }
/// </summary>
public class GameTimer
{
    #region field
    /// <summary>
    /// 設定された時間
    /// </summary>
    private float _IntervalTime = 0.0f;
    /// <summary>
    /// 経過時間
    /// </summary>
    private float _ElaspedTime = 0.0f;
    #endregion

    #region property
    /// <summary>
    /// 設定した時間を経過しているか？
    /// </summary>
    public bool IsTimeUp
    {
        get { return _IntervalTime <= _ElaspedTime; }
    }

    /// <summary>
    /// 経過時間 / 設定された時間 の割合
    /// </summary>
    public float TimeRate
    {
        get
        {
            if (IsTimeUp)
            {
                return 1.0f;
            }

            return _ElaspedTime / _IntervalTime;
        }
    }

    /// <summary>
    /// (1.0f - 経過時間 / 設定された時間)
    /// </summary>
    public float InverseTimeRate
    {
        get { return 1.0f - TimeRate; }
    }

    /// <summary>
    /// 残り時間
    /// </summary>
    public float LeftTime
    {
        get { return _IntervalTime - _ElaspedTime; }
    }

    /// <summary>
    /// 経過時間
    /// </summary>
    public float ElaspedTime
    {
        get { return _ElaspedTime; }
    }
    #endregion

    #region construct
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="interval">設定時間</param>
    public GameTimer(float interval = 1.0f)
    {
        _IntervalTime = interval;
    }
    #endregion

    #region public function
    /// <summary>
    /// 時間の更新
    /// </summary>
    /// <param name="scale">タイムスケール (1.0fで通常の時間)</param>
    /// <returns></returns>
    public bool UpdateTimer(float scale = 1.0f)
    {
        _ElaspedTime += Time.deltaTime * scale;
        return IsTimeUp;
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void ResetTimer()
    {
        _ElaspedTime = 0.0f;
    }

    /// <summary>
    /// リセット
    /// </summary>
    /// <param name="interval">設定時間</param>
    public void ResetTimer(float interval)
    {
        _IntervalTime = interval;
        _ElaspedTime = 0.0f;
    }
    #endregion
}
