using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Framework;

public class ManageController : MonoSingleton<ManageController>
{
    [SerializeField]
    private SoundManager _SoundManager = null;

    //[SerializeField]
    //private EffectManager _EffectManager = null;

    public SoundManager SoundManager { get { return _SoundManager; } }
}
