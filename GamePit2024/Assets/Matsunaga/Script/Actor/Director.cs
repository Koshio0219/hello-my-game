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
        /// <summary> �v���C���[�̏�� </summary>
        private enum StateEnum
        {
            /// <summary> �����Ȃ���� </summary>
            None,
            /// <summary> ���삷���� </summary>
            Move,
            /// <summary> �U�������� </summary>
            Attack,
            /// <summary> �U�����󂯂���� </summary>
            Damage,
            /// <summary> ���S��� </summary>
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
        /// �I�u�W�F�N�g���������ꂽ����AUnity����ŏ��ɂP��Ă΂�鏈��
        /// </summary>
        private void Start()
        {
            _Animator = GetComponent<Animator>();
            _Rigidbody = GetComponent<Rigidbody>();
            ChangeState(StateEnum.Move);
        }

        /// <summary>
        /// Unity���疈�t���[���Ă΂�鏈��
        /// </summary>
        private void Update()
        {
            UpdateState();
        }

        /// <summary>
        /// �R���W���������������u�Ԗ��ɂP�񂾂��Ă΂�鏈��
        /// �G�ɏՓ˂�����_���[�W��炤
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {

        }

        ///��!���G�̉������U���ɑ΂��鏈�����ȁH
        private void OnTriggerEnter(Collider other)
        {

        }
        #endregion

        #region public function
        ///<summary>
        /// Lighter���N�����鏈��
        /// GameModeController�ł��̃��\�b�h���Ăяo��
        ///</summary>
        public void StartPlay()
        {
            // �Y���̃Q�[���p�b�h���ڑ�����Ă��Ȃ��Ɠ����Ȃ�
            //if (Gamepad.all.Count < _GameParameter.GamepadNumber_A + 1) return;
            ChangeState(StateEnum.Move);
        }
        #endregion

        #region private function
        /// <summary>
        /// ��Ԃ̕ύX
        /// �������o�[�ϐ� _State��ύX����ۂ͂��̊֐����ĂԂ���
        /// �@���̊֐����ȊO�� _State�ւ̒��ڑ�������Ă͂����Ȃ�
        /// �@���̊֐����ĂԂ��Ƃňȉ��̗��_������
        /// �@1.���O���o��(�f�o�b�O���ɕ֗�)
        /// �@2.��ԕύX���̏������K���Ă΂��
        /// </summary>
        /// <param name="next">���̏��</param>
        private void ChangeState(StateEnum next)
        {
            // �ȑO�̏�Ԃ�ێ�
            var prev = _State;
            // ���̏�ԂɕύX����
            _State = next;

            // ���O���o��
            Log.Info(GetType(), "ChangeState {0} -> {1}", prev, next);
            // ��ԕύX����1�񂾂��Ă΂�鏈��������
            switch (_State)
            {
                case StateEnum.None:
                    // None�ύX��1�񂾂��Ă΂�鏈��
                    {
                    }
                    break;
                case StateEnum.Move:
                    // Move�ύX��1�񂾂��Ă΂�鏈��
                    {
                    }
                    break;
                case StateEnum.Attack:
                    // Attack�ύX��1�񂾂��Ă΂�鏈��
                    {
                        _Animator.SetTrigger("AttackTrigger");
                    }
                    break;
                case StateEnum.Damage:
                    {
                        // �A�j���[�V�����Đ�
                        _Animator.Play("Damage");
                    }
                    break;
                case StateEnum.Dead:
                    {
                        // �˂��A�j���[�V�����Đ�
                        _Animator.Play("Dead");
                    }
                    break;
            }
        }

        /// <summary>
        /// ��Ԗ��̖��t���[���Ă΂�鏈��
        /// </summary>
        private void UpdateState()
        {
            // ��Ԗ��̖��t���[���Ă΂�鏈��
            switch (_State)
            {
                case StateEnum.None:
                    // None���ɖ��t���[���Ă΂�鏈��
                    {
                    }
                    break;
                case StateEnum.Move:
                    // Move���ɖ��t���[���Ă΂�鏈��
                    {
                        // ����
                        if (_Rigidbody != null)
                        {
                            Walk();
                        }

                        // �U�����鏈�� �~�{�^���������ꂽ��
                        /*if (Gamepad.all[_PlayerParameter.GamepadNumber_D].buttonEast.wasPressedThisFrame && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rising") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
                        {
                            ChangeState(StateEnum.Attack);
                        }*/
                    }
                    break;
                case StateEnum.Attack:
                    // Attack���ɖ��t���[���Ă΂�鏈��
                    {
                    }
                    break;
                case StateEnum.Damage:
                    // Damage���ɖ��t���[���Ă΂�鏈��
                    {
                    }
                    break;
                case StateEnum.Dead:
                    // Dead���ɖ��t���[���Ă΂�鏈��
                    {
                    }
                    break;
            }
        }

        /// <summary> �������� </summary>
        private void Walk()
        {
            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") || _Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing")) return;
            Vector3 cameraForward = Vector3.Scale(_Camera.forward, new Vector3(1, 0, 1)).normalized;
            //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3 vertical = cameraForward * Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue().y;
            Vector3 horizontal = _Camera.right * Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue().x;
            Vector3 moveForward = vertical + horizontal;
            //_Velocity:���x�x�N�g�� vector3�^�Ō��݂̑��x�x�N�g����ێ� ���ʂ����������Ɉړ����\
            //normalized�͒P�ʃx�N�g���ɂ��ĕԂ��A�΂߈ړ����\�ɂ���
            //�A�j���[�V�����ɉ����Ĉړ����x���ς��悤 �����肷��ƈ�a�������邼
            //_GameParameter.AttackPower_A(0~1)��LerpUnclamped�ōŏ��l~�ő�l�ɉ������߂��
            //Debug.Log("������");
            float moveSpeed = Mathf.LerpUnclamped(0f, 5f, _Speed);
            _Velocity = moveForward.normalized * moveSpeed;
            if (_Velocity.magnitude > 0.085f)
            {
                _Animator.SetFloat("Speed", _Velocity.magnitude);
                transform.LookAt(transform.position + _Velocity);
                // �ړ�������
                _Rigidbody.MovePosition(_Rigidbody.position + _Velocity * _Speed * Time.deltaTime);
                //_Rigidbody.AddForce(_Velocity * _Speed * Time.deltaTime);
            }
            else
            {
                _Animator.SetFloat("Speed", 0f);
            }
        }
        #endregion

        /// <summary> �A�j���[�V�����C�x���g Attack�I�����ɋN�����郁�\�b�h </summary>
        private void AttackStart()
        {

        }

        /// <summary> �A�j���[�V�����C�x���g Attack�I�����ɋN�����郁�\�b�h </summary>
        private void AttackEnd()
        {
            ChangeState(StateEnum.Move);
        }
    }
}
