using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Test;
using Game.Manager;
using Game.Base;
using Cysharp.Threading.Tasks;
using Game.Framework;

public class MeleeStateController : Player
{
    [SerializeField] Rigidbody _rigidBody;
    [SerializeField] Animator _animator;
    [SerializeField] Transform _mainCamera;
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
    private Dictionary<PlayerState, IPlayerState> _player_state_list;
    private int GamePadNumber_M = 0;
    private PlayerState _state_old = PlayerState.IDLE;
    private IPlayerState _state_instance;

    private float _hp;
    public override float Hp { get => _hp; set => _hp = value; }
    public override PlayerType PlayerType => PlayerType.Swordsman;
    //private PlayerParameter _PlayerParameter;
    //public int InsId
    //{
    //    get => gameObject.GetInstanceID();
    //}
    //private async void Awake()
    //{
    //    _PlayerParameter = await GameData.Instance.GetPlayerParameter();

    //    //GameManager.stageManager.AddOnePlayer(InsId, gameObject);
    //}
    //protected async override void Awake()
    //{
    //    base.Awake();
    //    _hp = _PlayerParameter.hp_M;
    //}


    //private async void Awake()
    //{
    //    await base.Init();
    //    _hp = _PlayerParameter.hp_L;
    //}

    // Start is called before the first frame update
     void Start()
    {
         base.Init();
        _hp = _PlayerParameter.hp_M;
        _player_state_list = new Dictionary<PlayerState, IPlayerState>();
        _player_state_list = new Dictionary<PlayerState, IPlayerState> {
            { PlayerState.IDLE, new MeleeStateIdle(_animator, GamePadNumber_M) },
            { PlayerState.MOVE, new MeleeStateMove(_animator, GamePadNumber_M, _rigidBody, _mainCamera, transform, 0.65f) },
            { PlayerState.ATTACK, new MeleeStateAttack(_animator, GamePadNumber_M, _rigidBody, _mainCamera, transform, InsId, 1.0f,_PlayerParameter.attack_M) },
            { PlayerState.JUMP, new MeleeStateJump(_animator, GamePadNumber_M, transform, _rigidBody, _jump_power_up, _jump_power_max, distance_list_limit, ground_distance_limit, raycastSearchDistance) },
            { PlayerState.DAMAGE, new MeleeStateDamage(_animator, GamePadNumber_M) },
            { PlayerState.DEAD, new MeleeStateDead(_animator, GamePadNumber_M) },
        };

        _state_instance = _player_state_list[PlayerState.IDLE];
        _state_instance.enter();
    }

    // Update is called once per frame
    void Update()
    {
        if (_state_instance == null) return;
        //Debug.Log("PlayerStateLength: " + _player_state_list.Count);
        PlayerState state = _state_instance.stayUpdate();
        //Debug.Log(state.ToString());
        if (state == _state_old)
        {
            /*foreach (var keyValuePair in _player_state_list)
            {
                PlayerState key = keyValuePair.Key;
                Debug.Log($"key:{key}");
            }*/
            return;
        }
        Debug.Log(state.ToString());
        if (_player_state_list.ContainsKey(state))
        {
            Debug.Log("State: (" + _state_old.ToString() + ") -> (" + state.ToString() + ")");
            _state_instance.exit();
            _state_instance = _player_state_list[state];
            _state_instance.enter();
            _state_old = state;
        }

    }

    public void FixedUpdate()
    {
        if (_state_instance == null) return;
        _state_instance.stayFixedUpdate();
    }
}
