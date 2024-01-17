using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "HudConfig", menuName = "GamePit2024/HudConfig")]
    public class HudConfig : ScriptableObject
    {
        public GameObject popupTextPrefab;
        public Sprite lrMark;
        public Sprite udMark;
        public Sprite allMoveMark;

        public Color blockMoveable;
        public Color blockStatic;
    }
}