using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;
using UnityEngine.InputSystem;

namespace Game.Test
{
    public class Director : MonoBehaviour
    {
        #region define
        /// <summary> プレイヤーの状態 </summary>
        private enum StateEnum
        {
            /// <summary> 何もない状態 </summary>
            None,
            /// <summary> 動作する状態 </summary>
            Move,
            /// <summary> 攻撃する状態 </summary>
            Attack,
            /// <summary> 攻撃を受けた状態 </summary>
            Damage,
            /// <summary> 死亡状態 </summary>
            Dead,
        }
        #endregion

        #region serialize field
        [SerializeField] private Transform _Camera;

        #endregion


        #region field
        private StateEnum _State = StateEnum.None;
        private Animator _Animator;
        private Vector3 _Velocity;
        private Rigidbody _Rigidbody;
        private float _Speed = 0.6f;
        private PlayerParameter _PlayerParameter;
        #endregion

        #region property

        #endregion

        #region Unity private function

        private void Awake()
        {
            _PlayerParameter = PlayerParameter.Instance;
        }
        /// <summary>
        /// オブジェクトが生成された直後、Unityから最初に１回呼ばれる処理
        /// </summary>
        private void Start()
        {
            _Animator = GetComponent<Animator>();
            _Rigidbody = GetComponent<Rigidbody>();
            ChangeState(StateEnum.Move);
        }

        /// <summary>
        /// Unityから毎フレーム呼ばれる処理
        /// </summary>
        private void Update()
        {
            UpdateState();
        }

        /// <summary>
        /// コリジョンが当たった瞬間毎に１回だけ呼ばれる処理
        /// 敵に衝突したらダメージ喰らう
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {

        }

        ///＜!＞敵の遠距離攻撃に対する処理かな？
        private void OnTriggerEnter(Collider other)
        {

        }
        #endregion

        #region public function
        ///<summary>
        /// Lighterを起動する処理
        /// GameModeControllerでこのメソッドを呼び出す
        ///</summary>
        public void StartPlay()
        {
            // 該当のゲームパッドが接続されていないと動かない
            //if (Gamepad.all.Count < _GameParameter.GamepadNumber_A + 1) return;
            ChangeState(StateEnum.Move);
        }
        #endregion

        #region private function
        /// <summary>
        /// 状態の変更
        /// ※メンバー変数 _Stateを変更する際はこの関数を呼ぶこと
        /// 　この関数内以外で _Stateへの直接代入をしてはいけない
        /// 　この関数を呼ぶことで以下の利点がある
        /// 　1.ログが出る(デバッグ時に便利)
        /// 　2.状態変更時の処理が必ず呼ばれる
        /// </summary>
        /// <param name="next">次の状態</param>
        private void ChangeState(StateEnum next)
        {
            // 以前の状態を保持
            var prev = _State;
            // 次の状態に変更する
            _State = next;

            // ログを出す
            Log.Info(GetType(), "ChangeState {0} -> {1}", prev, next);
            // 状態変更時に1回だけ呼ばれる処理を書く
            switch (_State)
            {
                case StateEnum.None:
                    // None変更時1回だけ呼ばれる処理
                    {
                    }
                    break;
                case StateEnum.Move:
                    // Move変更時1回だけ呼ばれる処理
                    {
                    }
                    break;
                case StateEnum.Attack:
                    // Attack変更時1回だけ呼ばれる処理
                    {
                        _Animator.SetTrigger("AttackTrigger");
                    }
                    break;
                case StateEnum.Damage:
                    {
                        // アニメーション再生
                        _Animator.Play("Damage");
                    }
                    break;
                case StateEnum.Dead:
                    {
                        // 斃れるアニメーション再生
                        _Animator.Play("Dead");
                    }
                    break;
            }
        }

        /// <summary>
        /// 状態毎の毎フレーム呼ばれる処理
        /// </summary>
        private void UpdateState()
        {
            // 状態毎の毎フレーム呼ばれる処理
            switch (_State)
            {
                case StateEnum.None:
                    // None時に毎フレーム呼ばれる処理
                    {
                    }
                    break;
                case StateEnum.Move:
                    // Move時に毎フレーム呼ばれる処理
                    {
                        // 歩く
                        if (_Rigidbody != null)
                        {
                            Walk();
                        }

                        // 攻撃する処理 ×ボタンが押されたら
                        /*if (Gamepad.all[_PlayerParameter.GamepadNumber_D].buttonEast.wasPressedThisFrame && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rising") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
                        {
                            ChangeState(StateEnum.Attack);
                        }*/
                    }
                    break;
                case StateEnum.Attack:
                    // Attack時に毎フレーム呼ばれる処理
                    {
                    }
                    break;
                case StateEnum.Damage:
                    // Damage時に毎フレーム呼ばれる処理
                    {
                    }
                    break;
                case StateEnum.Dead:
                    // Dead時に毎フレーム呼ばれる処理
                    {
                    }
                    break;
            }
        }

        /// <summary> 歩く処理 </summary>
        private void Walk()
        {
            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") || _Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing")) return;
            Vector3 cameraForward = Vector3.Scale(_Camera.forward, new Vector3(1, 0, 1)).normalized;
            //方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 vertical = cameraForward * Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue().y;
            Vector3 horizontal = _Camera.right * Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue().x;
            Vector3 moveForward = vertical + horizontal;
            //_Velocity:速度ベクトル vector3型で現在の速度ベクトルを保持 質量を加味せずに移動が可能
            //normalizedは単位ベクトルにして返す、斜め移動を可能にする
            //アニメーションに応じて移動速度も変えよう 足滑りすると違和感があるぞ
            //_GameParameter.AttackPower_A(0~1)をLerpUnclampedで最小値~最大値に押し込めるよ
            //Debug.Log("歩くよ");
            float moveSpeed = Mathf.LerpUnclamped(0f, 5f, _Speed);
            _Velocity = moveForward.normalized * moveSpeed;
            if (_Velocity.magnitude > 0.085f)
            {
                _Animator.SetFloat("Speed", _Velocity.magnitude);
                transform.LookAt(transform.position + _Velocity);
                // 移動させる
                _Rigidbody.MovePosition(_Rigidbody.position + _Velocity * _Speed * Time.deltaTime);
                //_Rigidbody.AddForce(_Velocity * _Speed * Time.deltaTime);
            }
            else
            {
                _Animator.SetFloat("Speed", 0f);
            }
        }
        #endregion

        /// <summary> アニメーションイベント Attack終了時に起動するメソッド </summary>
        private void AttackStart()
        {

        }

        /// <summary> アニメーションイベント Attack終了時に起動するメソッド </summary>
        private void AttackEnd()
        {
            ChangeState(StateEnum.Move);
        }
    }
}
