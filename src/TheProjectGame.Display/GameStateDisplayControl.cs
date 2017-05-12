using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TheProjectGame.Contracts.Enums;
using TheProjectGame.Game;

namespace TheProjectGame.Display
{
    class GameStateDisplayControl : Panel
    {
        private const int cellSize = 30;

        public event EventHandler DesiredGameSizeChanged;

        public IGameHolder gameHolder { get; private set; }
        private Thread drawingThread;

        private Size desiredGameSizeInPixels;

        private static Font font = new Font("Arial", 10);

        public Size DesiredGameSizeInPixels
        {
            get
            {
                return desiredGameSizeInPixels;
            }
            set
            {
                if (desiredGameSizeInPixels.Width == value.Width && desiredGameSizeInPixels.Height == value.Height)
                {
                    return;
                }

                desiredGameSizeInPixels = value;
                DesiredGameSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public GameStateDisplayControl()
        {
            drawingThread = new Thread(new ThreadStart(new Action(() =>
            {
                while (true)
                {
                    RedrawBoard();
                    Thread.Sleep(1000 / 30);
                }
            })));

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        public void SetGameHolder(IGameHolder gameHolder)
        {
            this.gameHolder = gameHolder;
            drawingThread.Start();
        }

        private void RedrawBoard()
        {
            if (CanDrawBoard())
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(Invalidate));
                    }
                    else
                    {
                        Invalidate();
                    }
                }
                catch { }
            }
        }

        private bool CanDrawBoard() => (gameHolder?.Game?.Board ?? null) != null;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.Clear(Color.White);

            if (CanDrawBoard())
            {
                var board = gameHolder.Game.Board;

                DesiredGameSizeInPixels = new Size((int)board.BoardWidth * cellSize + 1, (int)board.BoardHeight * cellSize + 1);

                DrawGrid(e.Graphics, board.BoardHeight, board.BoardWidth);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (CanDrawBoard())
            { 
                var board = gameHolder.Game.Board;

                try
                {
                    board.Fields.Cast<Tile>()
                        .Where(t => t.Player != null)
                        .ToList().ForEach(DrawPlayer(e.Graphics));

                    board.Fields.OfType<TaskTile>()
                        .ToList().ForEach(DrawDistance(e.Graphics));

                    board.Fields.OfType<GoalTile>()
                        .ToList().ForEach(DrawGoal(e.Graphics));
                }
                catch { }
            }
        }

        private Action<Tile> DrawPlayer(Graphics g) => tile =>
        {
            var brush = tile.Player.Team == TeamColor.Blue ? Brushes.Blue : Brushes.Red;
            g.FillRectangle(brush, tile.X * cellSize + 1, tile.Y * cellSize + 1, cellSize - 1, cellSize - 1);
        };


        private Action<TaskTile> DrawDistance(Graphics g) => tile =>
        {
            string text = tile.DistanceToPiece.ToString();
            var size = g.MeasureString(text, font);

            g.DrawString(text, font, Brushes.Black, tile.X * cellSize + cellSize / 2 - size.Width / 2, tile.Y * cellSize + cellSize / 2 - size.Height / 2);
        };

        private Action<GoalTile> DrawGoal(Graphics g) => tile =>
        {
            string name = tile.Discovered ?
                (tile.Type == GoalFieldType.Goal ? "DG" : "DN") :
                (tile.Type == GoalFieldType.Goal ? "G" : "N");
            var size = g.MeasureString(name, font);

            g.DrawString(name, font, Brushes.Black, tile.X * cellSize + cellSize / 2 - size.Width / 2, tile.Y * cellSize + cellSize / 2 - size.Height / 2);
        };

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
