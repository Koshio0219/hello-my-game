using Game.Base;
using Game.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hud
{
    public class StageEndView : HudView
    {
        public GameObject battleClearPage;
        public GameObject gameOverPage;

        public void Win()
        {
            battleClearPage.Show();
            gameOverPage.Hide();
        }

        public void Lose()
        {
            battleClearPage.Hide();
            gameOverPage.Show();
        }
    }
}

