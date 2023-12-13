using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make the obj face to camera
public class SimpleBillboard : MonoBehaviour
{
    public PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update;
    private void Awake()
    {
        var main = Camera.main;
        UniTask.Void(async (_) =>
        {
            while (this && isActiveAndEnabled && !_.IsCancellationRequested)
            {
                transform.forward = main.transform.forward;
                await UniTask.DelayFrame(1, playerLoopTiming, this.GetCancellationTokenOnDestroy());
            }
        },this.GetCancellationTokenOnDestroy());
    }
}
