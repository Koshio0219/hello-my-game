using System.Collections.Generic;
using UnityEngine;
using animJump;

namespace Game.Test
{
    public class MeleeJumpStateController : MonoBehaviour
    {
        //https://qiita.com/mkgask/items/fa307811da6d9d76bc97
        ///<summary>
        ///    �W�����v�����Ɏg�p����Rigidbody
        ///</summary>
        private Rigidbody _rigidBody;

        ///<summary>
        ///    �W�����v�A�j���[�V������S������Animator
        ///</summary>
        private Animator _animator;

        ///<summary>
        ///    �W�����v�̊e��Ԃ�ێ����Ă����������X�g
        ///</summary>
        private Dictionary<JumpState, IJumpState> _jump_state_list;

        ///<summary>
        ///    �W�����v�̈ȑO�̏�Ԃ��L�����Ă���
        ///    ����Ɣ�r���邱�Ƃŏ�Ԃ̕ύX�����ʂ���
        ///</summary>
        private JumpState _state_old = JumpState.IDLE;

        ///<summary>
        ///    ���݂̃W�����v�̏�Ԃ�ێ�
        ///</summary>
        private IJumpState _state_instance;

        ///<summary>
        ///    �W�����v�͂Ɋւ������ێ�
        ///</summary>
        private JumpData _jump_data;

        ///<summary>
        ///    �v���C���[�L�����N�^�[�ƒn�ʊԂ̋����Ɋւ������ێ�
        ///</summary>
        private JumpDistance _jump_distance;

        ///<summary>
        ///    �L�[�������Ă���Ԃɗ��߂�W�����v�͂�1�t���[����
        ///</summary>
        [SerializeField] private float _jump_power_up;

        ///<summary>
        ///    �W�����v�͂̏��
        ///</summary>
        [SerializeField] private float _jump_power_max;

        ///<summary>
        ///    �㏸�����牺�~���ɐ؂�ւ��ω��̌��m���x
        ///</summary>
        [SerializeField] int distance_list_limit;

        ///<summary>
        ///    ���~������ڒn�����Ɣ��肷�鋗��
        ///</summary>
        [SerializeField] float ground_distance_limit;

        ///<summary>
        ///    �v���C���[�L�����N�^�[�ƒn�ʊԂ̌v���������
        ///    �ō����x��荂���l�łȂ��ƁA�W�����v���_�ł̉��~���[�V�����ւ̐؂�ւ�肪�o���܂���
        ///</summary>
        [SerializeField] float raycastSearchDistance;
        private PlayerParameter _PlayerParameter;

        private void Awake()
        {
            _PlayerParameter = PlayerParameter.Instance;
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
            var state = _state_instance.stay_update();
            
            if (state == _state_old) return;
            _state_instance.exit();
            _state_instance = _jump_state_list[state];
            _state_instance.enter();
            _state_instance.SetGamepadNumber(_PlayerParameter.GamepadNumber_M);
            _state_old = state;
            Debug.Log("jump.power: " + _jump_data.power);
        }

        public void FixedUpdate()
        {
            _state_instance.stay_fixed_update();
        }

    }
}
