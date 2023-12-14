using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using Game.Test;
using Game.Manager;

public class MeleeStateController : MonoBehaviour
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
    private PlayerState _state_old = PlayerState.IDLE;
    private IPlayerState _state_instance;
    private PlayerParameter _PlayerParameter;
    public int InsId
    {
        get => gameObject.GetInstanceID();
    }
    private async void Awake()
    {
        _PlayerParameter = await GameData.Instance.GetPlayerParameter();
        _player_state_list = new Dictionary<PlayerState, IPlayerState>();
            //GameManager.stageManager.AddOnePlayer(InsId, gameObject);
        }

    // Start is called before the first frame update
    void Start()
    {
        _player_state_list = new Dictionary<PlayerState, IPlayerState> {
            { PlayerState.IDLE, new MeleeStateIdle(_animator, _PlayerParameter.GamepadNumber_M) },
            { PlayerState.MOVE, new MeleeStateMove(_animator, _PlayerParameter.GamepadNumber_M, _rigidBody, _mainCamera, transform, 0.65f) },
            { PlayerState.ATTACK, new MeleeStateAttack(_animator, _PlayerParameter.GamepadNumber_M, _rigidBody, _mainCamera, transform, InsId, _PlayerParameter.attack_M) },
            { PlayerState.JUMP, new MeleeStateJump(_animator, _PlayerParameter.GamepadNumber_M, transform, _rigidBody, _jump_power_up, _jump_power_max, distance_list_limit, ground_distance_limit, raycastSearchDistance) },
            { PlayerState.DAMAGE, new MeleeStateDamage(_animator, _PlayerParameter.GamepadNumber_M) },
            { PlayerState.DEAD, new MeleeStateDead(_animator, _PlayerParameter.GamepadNumber_M) },
        };

        _state_instance = _player_state_list[PlayerState.IDLE];
        _state_instance.enter();
    }

    // Update is called once per frame
    void Update()
    {
        var state = _state_instance.stayUpdate();

        if (state == _state_old) return;
        _state_instance.exit();
        _state_instance = _player_state_list[state];
        _state_instance.enter();
        _state_old = state;
    }

    public void FixedUpdate()
    {
        _state_instance.stayFixedUpdate();
    }
}
