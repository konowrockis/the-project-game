﻿using System;
using System.Windows.Forms;
using TheProjectGame.Game;

namespace TheProjectGame.Display
{
    public partial class GameStateForm : Form
    {
        public GameStateForm(IGameHolder gameHolder)
        {
            InitializeComponent();

            gameStateControl.SetGameHolder(gameHolder);
            gameStateControl.DesiredGameSizeChanged += GameStateDisplayControl1_DesiredGameSizeChanged;
        }

        private void GameStateDisplayControl1_DesiredGameSizeChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateFormSize));
            }
            else
            {
                UpdateFormSize();
            }
        }

        private void UpdateFormSize()
        {
            ClientSize = gameStateControl.DesiredGameSizeInPixels;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
