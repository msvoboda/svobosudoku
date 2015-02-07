using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SudokuGrid
{
    public class SudokuClickEventArgs : EventArgs
    {
        public SudokuGrid.SudokuButton button;
        public MouseButtons mouseButton;
        public int cellX;
        public int cellY;
        public SudokuClickEventArgs(SudokuGrid.SudokuButton btn)
        {
            button = btn;             
        }
    }

    public delegate void SudokuClickEventHandler(object sender, SudokuGrid.SudokuClickEventArgs e);

    public partial class SudokuGridControl : UserControl
    {
        private SudokuGrid.SudokuButton[] m_buttons = new SudokuButton[SvoboSudoku.SudokuClass.MAX_COLS*SvoboSudoku.SudokuClass.MAX_ROWS];
        public event SudokuGrid.SudokuClickEventHandler SudokuClick;
        public event SudokuGrid.SudokuClickEventHandler SudokuCellDown;
        public event SudokuGrid.SudokuClickEventHandler SudokuCellUp; 

        public SudokuGridControl()
        {
            InitializeComponent();
            int velikost = 32;

            for (int i = 0; i < SvoboSudoku.SudokuClass.MAX_ROWS * SvoboSudoku.SudokuClass.MAX_COLS; i++)
            {
                int bx = i % SvoboSudoku.SudokuClass.MAX_ROWS;
                int by = i / SvoboSudoku.SudokuClass.MAX_ROWS;
                int mezx = 0;
                int mezy = 0;

                mezx = bx / 3;
                mezy = by / 3;

                m_buttons[i] = new SudokuButton();
                m_buttons[i].Left = (bx) * (velikost + 5) + mezx * 5;
                m_buttons[i].Top = (by) * (velikost + 5) + mezy * 5;
                m_buttons[i].Width = velikost;
                m_buttons[i].Height = velikost;
                m_buttons[i].Visible = true;
                m_buttons[i].Cell = new SvoboSudoku.SudokuCell();
                m_buttons[i].Cell.CellX = bx;
                m_buttons[i].Cell.CellY = by;
                m_buttons[i].Cell.Value = 0;
                for (int c = 1; c < m_buttons[i].Cell.Possible.Length; c++)
                    m_buttons[i].Cell.SetPossible(c, false);
                m_buttons[i].Click += new System.EventHandler(this.SudokuButtonClick);
                m_buttons[i].MouseDown += new MouseEventHandler(this.SudokuButtonMouseDown);
                m_buttons[i].MouseUp += new MouseEventHandler(this.SudokuButtonMouseUp);

                this.Controls.Add(m_buttons[i]);
            }
        }

        public void SetGrid(SvoboSudoku.SudokuCell[,] sudokuGrid)
        {
            // KOD
            for (int i = 0; i < SvoboSudoku.SudokuClass.MAX_ROWS; i++)
            {
                for (int j = 0; j < SvoboSudoku.SudokuClass.MAX_COLS; j++)
                {
                    m_buttons[i * SvoboSudoku.SudokuClass.MAX_ROWS + j].Cell = sudokuGrid[i, j];
                }
            }
        }

        public void SetFixedButtons()
        {
            for (int i = 0; i < SvoboSudoku.SudokuClass.MAX_ROWS * SvoboSudoku.SudokuClass.MAX_COLS; i++)
            {
                if (m_buttons[i].Cell.Value != 0)
                {
                    m_buttons[i].Enabled = false;
                }
            }
        }

        public void RefreshButtons()
        {
            for (int i = 0; i < SvoboSudoku.SudokuClass.MAX_ROWS * SvoboSudoku.SudokuClass.MAX_COLS; i++)
            {
                m_buttons[i].Invalidate();               
            }
        }

        private void SudokuGridControl_Load(object sender, EventArgs e)
        {

        }

        private void SudokuButtonClick(object sender, EventArgs e)
        {
            if (SudokuClick != null)
            {
                SudokuGrid.SudokuClickEventArgs evt = new SudokuClickEventArgs((SudokuButton)sender);
                evt.button = (SudokuButton)sender;
                evt.mouseButton = MouseButtons.None;

                SudokuClick(sender, evt);
            }
        }

        private void SudokuButtonMouseDown(object sender, MouseEventArgs e)
        {

            if (SudokuCellDown != null)
            {
                SudokuGrid.SudokuClickEventArgs evt = new SudokuClickEventArgs((SudokuButton)sender);
                evt.button = (SudokuButton)sender;
                evt.mouseButton = e.Button;
                SudokuCellDown(sender, evt);
            }
        }

        private void SudokuButtonMouseUp(object sender, MouseEventArgs e)
        {

            if (SudokuCellUp != null)
            {
                SudokuGrid.SudokuClickEventArgs evt = new SudokuClickEventArgs((SudokuButton)sender);
                evt.button = (SudokuButton)sender;
                evt.mouseButton = e.Button;
                SudokuCellUp(sender, evt);
            }
        }

    }
}
