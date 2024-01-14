using animJump;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeStateJump : IPlayerState
{
    private Animator _animator;
    private int _GamePadNumber;
    private Transform _player;
    private Rigidbody _rigidBody;
    private Dictionary<JumpState, IJumpState> _jump_state_list;
    private JumpState _state_old = JumpState.IDLE;
    private IJumpState _state_instance;
    private JumpData _jump_data;
    private JumpDistance _jump_distance;

    private float _jump_power_up;

    private float _jump_power_max;

    int distance_list_limit;

    float ground_distance_limit;

    float raycastSearchDistance;

    private float _speed;

    public MeleeStateJump(Animator animator, int GampePadNumber, Transform Player, Rigidbody rigidbody, float JumpPowerUp, float JumpPowerMax, int DistanceListLimit, float GroundDistanceLimit, float RayCastSearchDistance)
    {
        _GamePadNumber = GampePadNumber;
        _animator = animator;
        _player = Player;
        _rigidBody = rigidbody;
        _jump_power_up = JumpPowerUp;
        _jump_power_max = JumpPowerMax;
        distance_list_limit = DistanceListLimit;
        ground_distance_limit = GroundDistanceLimit;
        raycastSearchDistance = RayCastSearchDistance;
        _jump_data = new JumpData(_jump_power_up, _jump_power_max);

        _jump_distance = new JumpDistance(
            _player,
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

    public PlayerState stayUpdate()
    {

        var state = _state_instance.stay_update();

        if (state == _state_old)
        {
            return PlayerState.JUMP;
        }
        _state_instance.exit();
        _state_instance = _jump_state_list[state];
        _state_instance.enter();
        _state_instance.SetGamepadNumber(_GamePadNumber);
        _state_old = state;
        if(state == JumpState.IDLE)
        {
            return PlayerState.IDLE;
        }
        return PlayerState.JUMP;
    }

    public void enter()
    {
    }

    public void stayFixedUpdate() {
        _state_instance.stay_fixed_update();
    }

    public void enterDamage()
    {
        _state_old = JumpState.IDLE;
        _jump_state_list = new Dictionary<JumpState, IJumpState> {
            { JumpState.IDLE, new JumpStateIdle(_animator) },
            { JumpState.WAITING, new JumpStateWaiting(_animator, _jump_data) },
            { JumpState.RISING, new JumpStateRising(_animator, _jump_data, _jump_distance, _rigidBody) },
            { JumpState.FALLING, new JumpStateFalling(_animator, _jump_distance) },
            { JumpState.LANDING, new JumpStateLanding(_animator) },
        };

        _state_instance = _jump_state_list[JumpState.IDLE];
    }
    public void exit() { }
}
