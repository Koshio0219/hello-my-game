using System.Collections.Generic;
using UnityEngine;
using animJump;
using Game.Data;

namespace Game.Test
{
    public class LongRangeJumpStateController : MonoBehaviour
    {
        //https://qiita.com/mkgask/items/fa307811da6d9d76bc97
        ///<summary>
        ///    僕儍儞僾張棟偵巊梡偡傞Rigidbody
        ///</summary>
        private Rigidbody _rigidBody;

        ///<summary>
        ///    僕儍儞僾傾僯儊乕僔儑儞傪扴摉偡傞Animator
        ///</summary>
        private Animator _animator;

        ///<summary>
        ///    僕儍儞僾偺奺忬懺傪曐帩偟偰偍偔帿彂儕僗僩
        ///</summary>
        private Dictionary<JumpState, IJumpState> _jump_state_list;

        ///<summary>
        ///    僕儍儞僾偺埲慜偺忬懺傪婰壇偟偰偍偔
        ///    偙傟偲斾妑偡傞偙偲偱忬懺偺曄峏傪幆暿偡傞
        ///</summary>
        private JumpState _state_old = JumpState.IDLE;

        ///<summary>
        ///    尰嵼偺僕儍儞僾偺忬懺傪曐帩
        ///</summary>
        private IJumpState _state_instance;

        ///<summary>
        ///    僕儍儞僾椡偵娭偡傞忣曬傪曐帩
        ///</summary>
        private JumpData _jump_data;

        ///<summary>
        ///    僾儗僀儎乕僉儍儔僋僞乕偲抧柺娫偺嫍棧偵娭偡傞忣曬傪曐帩
        ///</summary>
        private JumpDistance _jump_distance;

        ///<summary>
        ///    僉乕傪墴偟偰偄傞娫偵棴傔傞僕儍儞僾椡偺1僼儗乕儉暘
        ///</summary>
        [SerializeField] private float _jump_power_up;

        ///<summary>
        ///    僕儍儞僾椡偺忋尷
        ///</summary>
        [SerializeField] private float _jump_power_max;

        ///<summary>
        ///    忋徃拞偐傜壓崀拞偵愗傝懼傢傞曄壔偺専抦惛搙
        ///</summary>
        [SerializeField] int distance_list_limit;

        ///<summary>
        ///    壓崀拞偐傜愙抧偟偨偲敾掕偡傞嫍棧
        ///</summary>
        [SerializeField] float ground_distance_limit;

        ///<summary>
        ///    僾儗僀儎乕僉儍儔僋僞乕偲抧柺娫偺寁應嫍棧忋尷
        ///    嵟崅崅搙傛傝崅偄抣偱側偄偲丄僕儍儞僾捀揰偱偺壓崀儌乕僔儑儞傊偺愗傝懼傢傝偑弌棃傑偣傫
        ///</summary>
        [SerializeField] float raycastSearchDistance;
        private PlayerParameter _PlayerParameter;

        private async void Awake()
        {
            _PlayerParameter = await GameData.Instance.GetPlayerParameter();
        }

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
            if (_PlayerParameter == null) return;
            var state = _state_instance.stay_update();

            if (state == _state_old) return;
            _state_instance.exit();
            _state_instance = _jump_state_list[state];
            _state_instance.enter();
            _state_instance.SetGamepadNumber(_PlayerParameter.GamepadNumber_L);
            _state_old = state;
            Debug.Log("jump.power: " + _jump_data.power);
        }

        public void FixedUpdate()
        {
            _state_instance.stay_fixed_update();
        }

    }
}
