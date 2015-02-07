using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace SvoboSudoku
{
    public enum sudokuStyle { skNone, skRect, skRound};
    
    public partial class SudokuGridView : System.Windows.Forms.Control
    {
        private string m_title;
        private sudokuStyle m_style;
        private Color m_bordercolor;
        private Color m_possibcolor;
        private Color m_fontcolor;
        private int m_borderwidth;
        private bool m_gridok = false;
        private Font m_fontSmall;
        SvoboSudoku.SudokuCell[,] m_sudokuGrid;


        public SudokuGridView()
        {
            InitializeComponent();

            m_title = "Sudoku";
            m_style = sudokuStyle.skRect;
            m_bordercolor = Color.Goldenrod;
            m_borderwidth = 3;
            m_fontSmall = new Font(Font.Name, 8);
        }

        public SudokuGridView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            m_title = "Sudoku";
            m_style = sudokuStyle.skRect;
            m_bordercolor = Color.Goldenrod;
            m_borderwidth = 3;
            m_fontSmall = new Font(Font.Name, 8);
        }

        public void SetGrid(SvoboSudoku.SudokuCell[,] sudokuGrid)
        {
            if (sudokuGrid.Length == 81)
                m_gridok = true;
            else
                m_gridok = false;

            m_sudokuGrid = sudokuGrid;
        }

        protected override void OnResize(EventArgs e)
        {
            int cliw = (Width - 2 * m_borderwidth) / 9;
            int rozdil = (Width - 2 * m_borderwidth) - cliw * 9;
            if (rozdil != 0)
            {
              this.Size = new Size(Width + (9 - rozdil), Width + (9 - rozdil));
            }
            else
            {
              Height = Width;
            }


            //Width = Width + (9 - rozdil);            
            
            Font = new Font(Font.Name, cliw/2);
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ///Font a = new Font("Arial", 10);
            Brush fore = new SolidBrush(ForeColor);
            Brush br;
            br = new SolidBrush(this.BackColor);
            g.FillRectangle(br, this.ClientRectangle);

            Brush podklad = new SolidBrush(this.BackColor);
            Pen gridPen = new Pen(fore, 1);
            Pen roundPen = new Pen(m_bordercolor, m_borderwidth);

            if (m_style == sudokuStyle.skRect)
            {
                g.DrawRectangle(roundPen, m_borderwidth / 2, m_borderwidth/2, ClientSize.Width - m_borderwidth, ClientSize.Height - m_borderwidth);
            }
            if (m_style == sudokuStyle.skRound)
            {
                int width = ClientRectangle.Width;
                int height = ClientRectangle.Height;

                Rectangle rect = new Rectangle(m_borderwidth - 1, m_borderwidth-1, width - m_borderwidth, height - m_borderwidth);
                using (GraphicsPath path = GetRoundRectPath(rect, 10))
                {
                    g.FillPath(br, path);
                    g.DrawPath(roundPen, path);
                }
            }

            // nakreslit mriz
            int cliw = (Width - 2 * m_borderwidth) / 9;
            for (int i = 1; i < 9; i++)
            {
                g.DrawLine(gridPen, m_borderwidth + i * cliw, m_borderwidth, m_borderwidth+i * cliw, Height - m_borderwidth-1);
                g.DrawLine(gridPen, m_borderwidth, m_borderwidth + i * cliw, Width - m_borderwidth-1, m_borderwidth + i * cliw);
            }

            int climw = (Width - 2 * m_borderwidth) / 3;
            for (int i = 1; i < 3; i++)
            {
                g.DrawLine(roundPen, m_borderwidth + i * climw, m_borderwidth, m_borderwidth + i * climw, Height - m_borderwidth);
                g.DrawLine(roundPen, m_borderwidth, m_borderwidth + i * climw, Width - m_borderwidth, m_borderwidth + i * climw);
            }

            // nakreslit grid                        

            if (this.m_gridok == true && this.m_sudokuGrid != null)
            {
                for (int i = 0; i < SvoboSudoku.SudokuClass.MAX_ROWS; i++)
                {
                    for (int j = 0; j < SvoboSudoku.SudokuClass.MAX_COLS; j++)
                    {
                        Rectangle rect = new Rectangle(m_borderwidth + j * cliw, m_borderwidth + i * cliw, cliw, cliw);
                        if (m_sudokuGrid[i, j].Value != 0)
                        {
                            int text_w = g.MeasureString(m_sudokuGrid[i, j].Value.ToString(), Font).ToSize().Width;
                            int text_h = g.MeasureString(m_sudokuGrid[i, j].Value.ToString(), Font).ToSize().Height;
                            g.DrawString(m_sudokuGrid[i, j].Value.ToString(), Font, fore, rect.Left+cliw/2-text_w/2, rect.Top+cliw/2-text_h/2);
                            //g.DrawString(m_sudokuGrid[i, j].Value.ToString(), Font, SolidBrush(FontColor), 1, 1);
                        }
                        else
                        {
                            int pulka = cliw / 2;

                            if (m_sudokuGrid[i, j].Possible[1]== true)
                            {
                              g.DrawString("1", m_fontSmall, fore, rect.Left, rect.Top+1);
                            }
                            if (m_sudokuGrid[i, j].Possible[2] == true)
                            {
                                int w = (int)g.MeasureString("2", m_fontSmall).Width;
                                g.DrawString("2", m_fontSmall, fore, rect.Left + pulka - w/2, rect.Top + 1);
                            }
                            if (m_sudokuGrid[i, j].Possible[3] == true)
                            {
                                int w = (int) g.MeasureString("3", m_fontSmall).Width;
                                g.DrawString("3", m_fontSmall, fore, rect.Right - w, rect.Top + 1);
                            }
                            if (m_sudokuGrid[i, j].Possible[4] == true)
                            {
                                int h = (int)g.MeasureString("4", m_fontSmall).Height;
                                g.DrawString("4", m_fontSmall, fore, rect.Left, rect.Top + cliw/2 - h/2 + 1);
                            }
                            if (m_sudokuGrid[i, j].Possible[5] == true)
                            {
                                int h = (int)g.MeasureString("5", m_fontSmall).Height;
                                int w = (int)g.MeasureString("5", m_fontSmall).Width;
                                g.DrawString("5", m_fontSmall, fore, rect.Left + pulka - w / 2, rect.Top + cliw/2 - h/2 + 1);
                            }
                            if (m_sudokuGrid[i, j].Possible[6] == true)
                            {
                                int h = (int)g.MeasureString("6", m_fontSmall).Height;
                                int w = (int)g.MeasureString("6", m_fontSmall).Width;
                                g.DrawString("6", m_fontSmall, fore, rect.Right - w, rect.Top + cliw/2 - h/2 + 1);
                            }
                            if (m_sudokuGrid[i, j].Possible[7] == true)
                            {
                                int h = (int)g.MeasureString("7", m_fontSmall).Height;
                                g.DrawString("7", m_fontSmall, fore, rect.Left, rect.Bottom -h);
                            }
                            if (m_sudokuGrid[i, j].Possible[8] == true)
                            {
                                int h = (int)g.MeasureString("8", m_fontSmall).Height;
                                int w = (int)g.MeasureString("8", m_fontSmall).Width;
                                g.DrawString("8", m_fontSmall, fore, rect.Left + pulka - w / 2, rect.Bottom - h);
                            }
                            if (m_sudokuGrid[i, j].Possible[9] == true)
                            {
                                int h = (int)g.MeasureString("9", m_fontSmall).Height;
                                int w = (int)g.MeasureString("9", m_fontSmall).Width;
                                g.DrawString("9", m_fontSmall, fore, rect.Right - w, rect.Bottom - h);
                            }
                        }
                    }
                }
            }
        }

        [
        Category("Settings"),
        Description("Border style")
        ]
        public sudokuStyle BorderStyle
        {
            get
            {
                return m_style;
            }
            set
            {
                m_style = value;
                Invalidate();
            }
        }

        public GraphicsPath GetRoundRectPath(Rectangle rect, int radius)
        {
            int diametr = 2 * radius;

            Rectangle arcRect = new Rectangle(rect.Location, new Size(diametr,diametr));

            GraphicsPath path = new GraphicsPath();

            path.AddArc(arcRect, 180, 90);
            arcRect.X = rect.Right - diametr;
            path.AddArc(arcRect, 270, 90);
            arcRect.Y = rect.Bottom - diametr;
            path.AddArc(arcRect, 0, 90);
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();

            return path;
        }


        //////////////////////////////////////////////
        [
        Category("GausOptions"),
        Description("Graph title")
        ]
        /// <summary>
        /// Titulek grafu
        /// pro titulek se pouziva font Font
        /// </summary>
        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                m_title = value;
                Invalidate();
            }
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
        // nastaveni barvy possible
        [
        Category("Settings"),
        Description("Possible color")
        ]
        public Color PossibleColor
        {
            get
            {
                return m_possibcolor;
            }
            set
            {
                m_possibcolor = value;
                Invalidate();
            }
        }

        // nastaveni barvy cisel ve hre
        [
        Category("Settings"),
        Description("Border color")
        ]
        public Color FontColor
        {
            get
            {
                return m_fontcolor;
            }
            set
            {
                m_fontcolor = value;
                Invalidate();
            }
        }

        // nastaveni barvy okraje
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

    }
}
