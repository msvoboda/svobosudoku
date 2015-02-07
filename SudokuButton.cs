using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;

namespace SudokuGrid
{
    public enum btnStyle { skNone, skRect, skRound };

    public partial class SudokuButton : System.Windows.Forms.Button
    {
        private Color m_bordercolor;
        private Color m_emptycolor;
        private Color m_disablecolor;
        private int m_borderwidth;
        private bool m_mouseover;
        private btnStyle m_style;
        private Region m_region;
        private GraphicsPath m_path;
        private SvoboSudoku.SudokuCell m_cell;
        private Color[] m_posscolor = new Color[10];
        private bool m_autofont; // automaticky resize fontu
        private bool m_empty;
        private bool m_error;

        public SudokuButton()
        {
            InitializeComponent();

            m_borderwidth = 1;
            m_bordercolor = Color.Black;
            m_emptycolor = Color.Gray;
            m_disablecolor = Color.Gray;
            m_style = btnStyle.skRect;
            m_region = new Region();
            m_path = new GraphicsPath();
            m_mouseover = false;
            m_error = false;
            m_cell = new SvoboSudoku.SudokuCell();
            m_cell.Value = 0;
            bool[] possible = new bool[SvoboSudoku.SudokuClass.MAX_COLS+1];            
            for (int i = 0; i <= SvoboSudoku.SudokuClass.MAX_COLS; i++)
                possible[i] = false;
            m_cell.Possible = possible;
            m_autofont = true;

            RefreshRegion();
        }

        public SudokuButton(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public SvoboSudoku.SudokuCell Cell
        {
            get
            {
                if (m_cell == null)
                {
                    m_cell = new SvoboSudoku.SudokuCell();
                }
                return m_cell;
            }
            set
            {
                m_cell = value;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            RefreshRegion();
            int cliw;
            if (Width < Height)
                cliw = (Width - 2 * m_borderwidth);
            else
                cliw = (Height - 2 * m_borderwidth);
            Font = new Font(Font.Name, 2*cliw / 3);
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
          m_mouseover = false;
          this.Invalidate();
          base.OnMouseLeave(e);
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
           m_mouseover = true;
           this.Invalidate();
           base.OnMouseEnter(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ///Font a = new Font("Arial", 10);
            Brush fore;
            if (Enabled == true)
                fore = new SolidBrush(ForeColor);
            else
                fore = new SolidBrush(DisableColor);

            Font fn = Font;

            SolidBrush brBrush = new SolidBrush(m_bordercolor);
            Brush br;
            if (m_error == true)
            {
              br = new SolidBrush(Color.Red);
            }
            else
            {
              br = new SolidBrush(this.BackColor);
            }

            Pen roundPen;
            int borderwidth=m_borderwidth;
            if (m_mouseover == true && Enabled == true)
                borderwidth = m_borderwidth + 1;

            roundPen= new Pen(brBrush, borderwidth);
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            if (m_style == btnStyle.skRect)
            {
                g.FillRectangle(br, this.ClientRectangle);
                g.DrawRectangle(roundPen, borderwidth/2, borderwidth/2, ClientSize.Width - borderwidth, ClientSize.Height - borderwidth);
            }
            if (m_style == btnStyle.skRound)
            {
                int width = ClientRectangle.Width;
                int height = ClientRectangle.Height;

                //Rectangle rect = new Rectangle(m_borderwidth - 1, m_borderwidth - 1, width - m_borderwidth, height - m_borderwidth);
                g.FillPath(br, m_path);
                g.DrawPath(roundPen, m_path);
                //g.FillRegion(br, m_region);
            }

            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (m_cell.Value != 0)
            {
               int tw = (int) g.MeasureString(m_cell.Value.ToString(), Font).Width;
               int th = (int) g.MeasureString(m_cell.Value.ToString(), Font).Height;
               g.DrawString(m_cell.Value.ToString(), fn, fore, ClientRectangle.Left+Width/2-tw/2, ClientRectangle.Top+Height/2-th/2);
            }
            else
            {                
                int koule = ClientRectangle.Width / 6;
                int kousek = (ClientRectangle.Width - 2 * m_borderwidth) / 3;
                bool zadny = true;

                if (m_cell.Possible[1] == true)
                {
                  Rectangle r = new Rectangle(koule/2, koule/2, koule, koule);
                  g.FillRectangle(new SolidBrush(m_posscolor[1]), r);
                  zadny = false;
                }
                if (m_cell.Possible[2] == true)
                {
                    Rectangle r = new Rectangle(Width/2-koule/2, koule/2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[2]), r);
                    zadny = false;
                }

                if (m_cell.Possible[3] == true)
                {
                    Rectangle r = new Rectangle(Width - koule - koule/2, koule/2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[3]), r);
                    zadny = false;
                }


                if (m_cell.Possible[4] == true)
                {
                    Rectangle r = new Rectangle(koule / 2, Height/2-koule / 2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[4]), r);
                    zadny = false;
                }
                if (m_cell.Possible[5] == true)
                {
                    Rectangle r = new Rectangle(Width / 2 - koule / 2, Height / 2 - koule / 2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[5]), r);
                    zadny = false;
                }

                if (m_cell.Possible[6] == true)
                {
                    Rectangle r = new Rectangle(Width - koule - koule / 2, Height / 2 - koule / 2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[6]), r);
                    zadny = false;
                }

                if (m_cell.Possible[7] == true)
                {
                    Rectangle r = new Rectangle(koule / 2, Height  - 3*koule / 2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[7]), r);
                    zadny = false;
                }
                if (m_cell.Possible[8] == true)
                {
                    Rectangle r = new Rectangle(Width / 2 - koule / 2, Height - 3*koule / 2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[8]), r);
                    zadny = false;
                }

                if (m_cell.Possible[9] == true)
                {
                    Rectangle r = new Rectangle(Width - koule - koule / 2, Height - 3*koule / 2, koule, koule);
                    g.FillRectangle(new SolidBrush(m_posscolor[9]), r);
                    zadny = false;
                }

                if (zadny == true)
                {
                    // empty staf
                    Point[] gr_points = new Point[] { new Point(ClientRectangle.Left, ClientRectangle.Top),new Point(ClientRectangle.Right, ClientRectangle.Top), new Point(ClientRectangle.Right, ClientRectangle.Bottom), new Point(ClientRectangle.Left, ClientRectangle.Bottom)};
                    using (Brush brush = new PathGradientBrush(gr_points))
                    {
                        Rectangle gr = ClientRectangle;
                        gr.X += 0; gr.Y += 0; gr.Inflate(-2*m_borderwidth-1, -2*m_borderwidth-1);
                        g.FillRectangle(brush, gr);
                    }
                }

            }
            //e.Graphics.SmoothingMode = SmoothingMode.Default;

            //base.OnPaint(e);
        }

        private GraphicsPath GetRoundPath(Rectangle rc, int r)
        {
            int x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);				//Upper left corner
            path.AddArc(x + w - r, y, r, r, 270, 90);			//Upper right corner
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);		//Lower right corner
            path.AddArc(x, y + h - r, r, r, 90, 90);			//Lower left corner
            path.CloseFigure();
            return path;
        }

        public GraphicsPath GetRoundRectPath(Rectangle rect, int radius)
        {
            int diametr = 2 * radius;

            Rectangle arcRect = new Rectangle(rect.Location, new Size(diametr, diametr));

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



        // nastaveni sirka okraje
        [
        Category("Settings"),
        Description("Automatic font size")
        ]
        public bool AutoFontSize
        {
            get
            {
                return m_autofont;
            }
            set
            {
                m_autofont = value;
                Invalidate();
            }
        }

        // nastaveni sirka okraje
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

        // nastaveni barvy okraje
        [
        Category("Settings"),
        Description("Disable color")
        ]
        public Color DisableColor
        {
            get
            {
                return m_disablecolor;
            }
            set
            {
                m_disablecolor = value;
                Invalidate();
            }
        }


        // nastaveni barvy kdys je prazdny
        [
        Category("Settings"),
        Description("Empty color")
        ]
        public Color EmptyColor
        {
            get
            {
                return m_emptycolor;
            }
            set
            {
                m_emptycolor = value;
                Invalidate();
            }
        }

        [
        Category("Settings"),
        Description("Border style")
        ]
        public btnStyle BorderStyle
        {
            get
            {
                return m_style;
            }
            set
            {
                m_style = value;
                RefreshRegion();
                Invalidate();
            }
        }

        // stav ERROR
        [
        Category("Settings"),
        Description("Error state button")
        ]
        public bool Error
        {
            get
            {
                return m_error;
            }
            set
            {
                m_error = value;
                Invalidate();
            }
        }

        private void RefreshRegion()
        {
            // predddef barvy
            m_posscolor[1] = Color.Brown;
            m_posscolor[2] = Color.Green;
            m_posscolor[3] = Color.Navy;
            m_posscolor[4] = Color.SpringGreen;
            m_posscolor[5] = Color.Black;
            m_posscolor[6] = Color.Purple;
            m_posscolor[7] = Color.LightBlue;
            m_posscolor[8] = Color.Khaki;
            m_posscolor[9] = Color.Red;

            Rectangle rect = new Rectangle(0, 0, Width, Height);
            if (m_style == btnStyle.skRound)
            {
                m_path = this.GetRoundPath(rect, 16);
                Region = new Region(m_path);
            }
            if (m_style == btnStyle.skRect)
            {

                m_path = new GraphicsPath();
                m_path.AddRectangle(rect);
                Region = new Region(m_path);
            }
        }
    }
}
