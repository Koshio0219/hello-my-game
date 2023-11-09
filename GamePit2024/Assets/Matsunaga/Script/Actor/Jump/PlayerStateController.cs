using System.Collections.Generic;
using UnityEngine;
using animJump;

namespace Game.Test
{
    public class PlayerStateController : MonoBehaviour
    {//https://qiita.com/mkgask/items/fa307811da6d9d76bc97
     ///<summary>
     ///    ジャンプ処理に使用するRigidbody
     ///</summary>
        private Rigidbody _rigidBody;

        ///<summary>
        ///    ジャンプアニメーションを担当するAnimator
        ///</summary>
        private Animator _animator;

        ///<summary>
        ///    ジャンプの各状態を保持しておく辞書リスト
        ///</summary>
        private Dictionary<JumpState, IJumpState> _jump_state_list;

        ///<summary>
        ///    ジャンプの以前の状態を記憶しておく
        ///    これと比較することで状態の変更を識別する
        ///</summary>
        private JumpState _state_old = JumpState.IDLE;

        ///<summary>
        ///    現在のジャンプの状態を保持
        ///</summary>
        private IJumpState _state_instance;

        ///<summary>
        ///    ジャンプ力に関する情報を保持
        ///</summary>
        private JumpData _jump_data;

        ///<summary>
        ///    プレイヤーキャラクターと地面間の距離に関する情報を保持
        ///</summary>
        private JumpDistance _jump_distance;

        ///<summary>
        ///    キーを押している間に溜めるジャンプ力の1フレーム分
        ///</summary>
        [SerializeField] private float _jump_power_up;

        ///<summary>
        ///    ジャンプ力の上限
        ///</summary>
        [SerializeField] private float _jump_power_max;

        ///<summary>
        ///    上昇中から下降中に切り替わる変化の検知精度
        ///</summary>
        [SerializeField] int distance_list_limit;

        ///<summary>
        ///    下降中から接地したと判定する距離
        ///</summary>
        [SerializeField] float ground_distance_limit;

        ///<summary>
        ///    プレイヤーキャラクターと地面間の計測距離上限
        ///    最高高度より高い値でないと、ジャンプ頂点での下降モーションへの切り替わりが出来ません
        ///</summary>
        [SerializeField] float raycastSearchDistance;

        public void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _jump_data = new JumpData(_jump_power_up, _jump_power_max);

            _jump_distance = new JumpDistance(
                transform,
                distance_list_limit,
                ground_distance_limit,
                raycastSearchDistance
            );

            _jump_state_list = new Dictionary<JumpState, IJumpState> {
            { JumpState.IDLE, new JumpStateIdle(_animator) },
            { JumpState.WAITING, new JumpStateWaiting(_animator, _jump_data) },
            { JumpState.RISING, new JumpStateRising(_animator, _jump_data, _jump_distance, _rigidBody) },
            { JumpState.FALLING, new JumpStateFalling(_animator, _jump_distance) },
            { JumpState.LANDING, new JumpStateLanding(_animator) },
        };

            _state_instance = _jump_state_list[JumpState.IDLE];
            _state_instance.enter();
        }

        public void Update()
        {
            var state = _state_instance.stay_update();
            if (state == _state_old) return;

            _state_instance.exit();
            _state_instance = _jump_state_list[state];
            _state_instance.enter();
            _state_old = state;
        }

        public void FixedUpdate()
        {
            _state_instance.stay_fixed_update();
        }
    }

}
