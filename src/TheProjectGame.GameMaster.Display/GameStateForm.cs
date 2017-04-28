using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Game;

namespace TheProjectGame.GameMaster.Display
{
    public partial class GameStateForm : Form
    {
        private readonly IGameHolder gameHolder;

        public GameStateForm(IGameHolder gameHolder)
        {
            InitializeComponent();

            this.gameHolder = gameHolder;
            this.gameHolder.GameUpdated += Game_GameUpdated;
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

        private void Game_GameUpdated(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Refresh));
            }
            else
            {
                Refresh();
            }
        }

        private const int cellSize = 30;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            var game = gameHolder.Game;
            if (game != null)
            {
                var board = game.Board;
                DrawGrid(e.Graphics, board.BoardHeight, board.BoardWidth);

                foreach (var tile in board.Fields)
                {
                    if (tile.Player != null)
                    {
                        DrawPlayer(e.Graphics, tile.X, tile.Y, tile.Player.Team == TeamColor.Blue ? Brushes.Blue : Brushes.Red, tile.Player.Id);
                    }
                }

                foreach (var tile in board.Fields.OfType<TaskTile>())
                {
                    DrawDistance(e.Graphics, tile.X, tile.Y, tile.DistanceToPiece);
                }

                foreach (var tile in board.Fields.OfType<GoalTile>())
                {
                    string name = string.Empty;

                    if (tile.Discovered)
                    {
                        name = tile.Type == GoalFieldType.Goal ? "DG" : "DN";
                    }
                    else
                    {
                        name = tile.Type == GoalFieldType.Goal ? "G" : "N";
                    }

                    DrawGoal(e.Graphics, tile.X, tile.Y, name);
                }
            }

            base.OnPaint(e);
        }

        private void DrawPlayer(Graphics g, uint x, uint y, Brush b, ulong id)
        {
            g.FillRectangle(b, x * cellSize + 1, y * cellSize + 1, cellSize - 1, cellSize - 1);
        }

        private void DrawDistance(Graphics g, uint x, uint y, int distance)
        {
            string text = distance.ToString();
            Font f = new Font("Arial", 10);

            var size = g.MeasureString(text, f);

            g.DrawString(text, f, Brushes.Black, x * cellSize + cellSize / 2 - size.Width / 2, y * cellSize + cellSize / 2 - size.Height / 2);
        }

        private void DrawGoal(Graphics g, uint x, uint y, string name)
        {
            Font f = new Font("Arial", 10);

            var size = g.MeasureString(name, f);

            g.DrawString(name, f, Brushes.Black, x * cellSize + cellSize / 2 - size.Width / 2, y * cellSize + cellSize / 2 - size.Height / 2);
        }

        private void DrawGrid(Graphics g, uint rows, uint cols, int cellSize = cellSize)
        {
            for (int x = 0; x <= cols; x++)
            {
                g.DrawLine(Pens.Black, x * cellSize, 0, x * cellSize, rows * cellSize);
            }

            for (int y = 0; y <= rows; y++)
            {
                g.DrawLine(Pens.Black, 0, y * cellSize, cols * cellSize, y * cellSize);
            }
        }
    }
}
