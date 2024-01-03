using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Game.Loader;
using UnityEngine.InputSystem;
using Game.Data;

namespace Game.Test
{
    public class Director : MonoBehaviour
    {
        #region define
        /// <summary> 僾儗僀儎乕偺忬懺 </summary>
        private enum StateEnum
        {
            /// <summary> 壗傕側偄忬懺 </summary>
            None,
            /// <summary> 摦嶌偡傞忬懺 </summary>
            Move,
            /// <summary> 峌寕偡傞忬懺 </summary>
            Attack,
            /// <summary> 峌寕傪庴偗偨忬懺 </summary>
            Damage,
            /// <summary> 巰朣忬懺 </summary>
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
            _PlayerParameter = GameData.Instance.PlayerParameter;
            _Animator = GetComponent<Animator>();
            _Rigidbody = GetComponent<Rigidbody>();
        }
        /// <summary>
        /// 僆僽僕僃僋僩偑惗惉偝傟偨捈屻丄Unity偐傜嵟弶偵侾夞屇偽傟傞張棟
        /// </summary>
        private void Start()
        {

            ChangeState(StateEnum.Move);
        }

        /// <summary>
        /// Unity偐傜枅僼儗乕儉屇偽傟傞張棟
        /// </summary>
        private void Update()
        {
            UpdateState();
        }

        /// <summary>
        /// 僐儕僕儑儞偑摉偨偭偨弖娫枅偵侾夞偩偗屇偽傟傞張棟
        /// 揋偵徴撍偟偨傜僟儊乕僕嬺傜偆
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
            {
                _Rigidbody.velocity = Vector3.zero;
                transform.SetParent(collision.transform);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
            {
                transform.SetParent(null);
            }
        }

        ///亙!亜揋偺墦嫍棧峌寕偵懳偡傞張棟偐側丠
        private void OnTriggerEnter(Collider other)
        {

        }
        #endregion

        #region public function
        ///<summary>
        /// Lighter傪婲摦偡傞張棟
        /// GameModeController偱偙偺儊僜僢僪傪屇傃弌偡
        ///</summary>
        public void StartPlay()
        {
            // 奩摉偺僎乕儉僷僢僪偑愙懕偝傟偰偄側偄偲摦偐側偄
            //if (Gamepad.all.Count < _GameParameter.GamepadNumber_A + 1) return;
            ChangeState(StateEnum.Move);
        }
        #endregion

        #region private function
        /// <summary>
        /// 忬懺偺曄峏
        /// 仸儊儞僶乕曄悢 _State傪曄峏偡傞嵺偼偙偺娭悢傪屇傇偙偲
        /// 丂偙偺娭悢撪埲奜偱 _State傊偺捈愙戙擖傪偟偰偼偄偗側偄
        /// 丂偙偺娭悢傪屇傇偙偲偱埲壓偺棙揰偑偁傞
        /// 丂1.儘僌偑弌傞(僨僶僢僌帪偵曋棙)
        /// 丂2.忬懺曄峏帪偺張棟偑昁偢屇偽傟傞
        /// </summary>
        /// <param name="next">師偺忬懺</param>
        private void ChangeState(StateEnum next)
        {
            // 埲慜偺忬懺傪曐帩
            var prev = _State;
            // 師偺忬懺偵曄峏偡傞
            _State = next;

            // 儘僌傪弌偡
            Log.Info(GetType(), "ChangeState {0} -> {1}", prev, next);
            // 忬懺曄峏帪偵1夞偩偗屇偽傟傞張棟傪彂偔
            switch (_State)
            {
                case StateEnum.None:
                    // None曄峏帪1夞偩偗屇偽傟傞張棟
                    {
                    }
                    break;
                case StateEnum.Move:
                    // Move曄峏帪1夞偩偗屇偽傟傞張棟
                    {
                    }
                    break;
                case StateEnum.Attack:
                    // Attack曄峏帪1夞偩偗屇偽傟傞張棟
                    {
                        _Animator.SetTrigger("AttackTrigger");
                    }
                    break;
                case StateEnum.Damage:
                    {
                        // 傾僯儊乕僔儑儞嵞惗
                        _Animator.Play("Damage");
                    }
                    break;
                case StateEnum.Dead:
                    {
                        // 澦傟傞傾僯儊乕僔儑儞嵞惗
                        _Animator.Play("Dead");
                    }
                    break;
            }
        }

        /// <summary>
        /// 忬懺枅偺枅僼儗乕儉屇偽傟傞張棟
        /// </summary>
        private void UpdateState()
        {
            // 忬懺枅偺枅僼儗乕儉屇偽傟傞張棟
            switch (_State)
            {
                case StateEnum.None:
                    // None帪偵枅僼儗乕儉屇偽傟傞張棟
                    {
                    }
                    break;
                case StateEnum.Move:
                    // Move帪偵枅僼儗乕儉屇偽傟傞張棟
                    {
                        // 曕偔
                        if (_Rigidbody != null)
                        {
                            Walk();
                        }

                        // 峌寕偡傞張棟 亊儃僞儞偑墴偝傟偨傜
                        /*if (Gamepad.all[_PlayerParameter.GamepadNumber_D].buttonEast.wasPressedThisFrame && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Rising") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") && !_Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
                        {
                            ChangeState(StateEnum.Attack);
                        }*/
                    }
                    break;
                case StateEnum.Attack:
                    // Attack帪偵枅僼儗乕儉屇偽傟傞張棟
                    {
                    }
                    break;
                case StateEnum.Damage:
                    // Damage帪偵枅僼儗乕儉屇偽傟傞張棟
                    {
                    }
                    break;
                case StateEnum.Dead:
                    // Dead帪偵枅僼儗乕儉屇偽傟傞張棟
                    {
                    }
                    break;
            }
        }

        /// <summary> 曕偔張棟 </summary>
        private void Walk()
        {
            if (_PlayerParameter ==null)
            {
                return;
            }
            if (_Animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") || _Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing")) return;
            Vector3 cameraForward = Vector3.Scale(_Camera.forward, new Vector3(1, 0, 1)).normalized;
            //曽岦僉乕偺擖椡抣偲僇儊儔偺岦偒偐傜丄堏摦曽岦傪寛掕
            Vector3 vertical = cameraForward * Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue().y;
            Vector3 horizontal = _Camera.right * Gamepad.all[_PlayerParameter.GamepadNumber_D].leftStick.ReadValue().x;
            Vector3 moveForward = vertical + horizontal;
            //_Velocity:懍搙儀僋僩儖 vector3宆偱尰嵼偺懍搙儀僋僩儖傪曐帩 幙検傪壛枴偣偢偵堏摦偑壜擻
            //normalized偼扨埵儀僋僩儖偵偟偰曉偡丄幬傔堏摦傪壜擻偵偡傞
            //傾僯儊乕僔儑儞偵墳偠偰堏摦懍搙傕曄偊傛偆 懌妸傝偡傞偲堘榓姶偑偁傞偧
            //_GameParameter.AttackPower_A(0~1)傪LerpUnclamped偱嵟彫抣~嵟戝抣偵墴偟崬傔傞傛
            //Debug.Log("曕偔傛");
            float moveSpeed = Mathf.LerpUnclamped(0f, 5f, _Speed);
            _Velocity = moveForward.normalized * moveSpeed;
            if (_Velocity.magnitude > 0.085f)
            {
                _Animator.SetFloat("Speed", _Velocity.magnitude);
                transform.LookAt(transform.position + _Velocity);
                // 堏摦偝偣傞
                _Rigidbody.MovePosition(_Rigidbody.position + _Velocity * _Speed * Time.deltaTime);
                //_Rigidbody.AddForce(_Velocity * _Speed * Time.deltaTime);
            }
            else
            {
                _Animator.SetFloat("Speed", 0f);
            }
        }
        #endregion

        /// <summary> 傾僯儊乕僔儑儞僀儀儞僩 Attack廔椆帪偵婲摦偡傞儊僜僢僪 </summary>
        private void AttackStart()
        {

        }

        /// <summary> 傾僯儊乕僔儑儞僀儀儞僩 Attack廔椆帪偵婲摦偡傞儊僜僢僪 </summary>
        private void AttackEnd()
        {
            ChangeState(StateEnum.Move);
        }


    }
}
