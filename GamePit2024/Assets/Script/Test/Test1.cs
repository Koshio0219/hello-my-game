using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game.Test
{
    public class Test1 : MonoBehaviour
    {
        void Start()
        {
            gameObject.transform.DOMoveX(transform.position.x + 5, 1.0f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                Debug.Log($"the game object {gameObject.name} has move to its x+5 position.");
            });

        }
    }
}