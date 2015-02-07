using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace SudokuGrid
{
    public enum cellStyle { skNone, skRect, skRound };

    public partial class SudokuPanel : System.Windows.Forms.Control
    {
        // konstanty
        public const int MAX_ROWS = 9;
        public const int MAX_COLS = 9;

        private Color m_bordercolor;
        private int m_borderwidth;
        private int m_margin;
        ///
        private int m_gridsize;
        private int m_cellsize;

        public SudokuPanel()
        {
            InitializeComponent();

            m_borderwidth = 1;
            m_bordercolor = Color.Black;
            m_margin = 5;
        }

        public SudokuPanel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            Height = Width;

            m_gridsize = Width - 2 * m_margin;
            m_cellsize = (m_gridsize - MAX_COLS * 5)/MAX_COLS;
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ///Font a = new Font("Arial", 10);
            Brush fore = new SolidBrush(ForeColor);
            SolidBrush brBrush = new SolidBrush(m_bordercolor);
            Brush br;
            br = new SolidBrush(this.BackColor);
            g.FillRectangle(br, this.ClientRectangle);


            Pen borderPen = new Pen(brBrush, 1);
            //g.DrawRectangle(borderPen, 0,0, Width-1,Height-1);

            for (int y = 0; y < MAX_ROWS; y++)
            {
                for (int x = 0; x < MAX_COLS; x++)
                {
                    Rectangle r = new Rectangle(m_margin + x * m_cellsize, m_margin + y * m_cellsize, m_cellsize, m_cellsize);
                    g.DrawRectangle(borderPen, r);
                }
            }
            /*
            Brush podklad = new SolidBrush(this.BackColor);
            Pen gridPen = new Pen(fore, 1);
            Pen roundPen = new Pen(m_bordercolor, m_borderwidth);
            */
        }

        // nastaveni barvy okraje
        [
        Category("Settings"),
        Description("Border color")
        ]
        public Color BorderColor
        {
            get
            {
                return m_bordercolor;
            }
            set
            {
                m_bordercolor = value;
                Invalidate();
            }
        }

        // nastaveni sirky okraje
        [
        Category("Settings"),
        Description("Border width")
        ]
        public int BorderWidth
        {
            get
            {
                return m_borderwidth;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                m_borderwidth = value;
                Invalidate();
            }
        }

        [
        Category("Settings"),
        Description("Margin")
        ]
        public int Margin
        {
            get
            {
                return m_margin;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                m_margin = value;
                Invalidate();
            }
        }

    }
}
