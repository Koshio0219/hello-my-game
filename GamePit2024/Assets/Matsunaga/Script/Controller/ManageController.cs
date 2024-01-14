using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageController : MonoBehaviour
{
    [SerializeField]
    private SoundManager _SoundManager = null;

    //[SerializeField]
    //private EffectManager _EffectManager = null;

    public SoundManager SoundManager { get { return _SoundManager; } }
}
