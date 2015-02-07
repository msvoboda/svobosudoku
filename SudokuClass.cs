/// Name: SudokuClass
/// Desc: Trida pro vygenerovani sudoku 
/// Auth: Michal Svoboda
/// Date: (c) 2006

using System;
using System.Collections.Generic;
using System.Text;

namespace SvoboSudoku
{
    public class SudokuCell
    {
        private int m_value;
        private int cell_x;
        private int cell_y;
        private bool[] m_possible = new bool[10];        

        public SudokuCell()
        {
            m_value = 0;
            for (int i = 0; i < 10; i++)
                m_possible[i] = true;
        }

        public void ResetCell()
        {
            m_value = 0;
            for (int i = 0; i < 10; i++)
                m_possible[i] = true;
        }

        public int CellX
        {
            get
            {
                return cell_x;
            }
            set
            {
                cell_x = value;
            }
        }

        public int CellY
        {
            get
            {
                return cell_y;
            }
            set
            {
                cell_y = value;
            }
        }

        public int Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }

        public bool[] Possible
        {
            get
            {
                return m_possible;
            }
            set
            {
                for (int i = 1; i < 10 && i < value.Length; i++)
                {
                    m_possible[i] = value[i];
                }
            }
        }

        public void SetPossible(int set, bool val)
        {
            if (set == 0 || set > 9)
            {
                return;    
            }

            m_possible[set] = val;
        }

        public void SetPossible(bool set)
        {
            for (int i = 0; i < 10; i++)
                m_possible[i] = set;
        }

        public int GetPossibleCount(bool arg)
        {
            int count = 0;

            for (int i = 1; i < m_possible.Length; i++)
            {
                if (m_possible[i] == arg)
                    count++;
            }

            return count;
        }

    }

    public class SudokuClass
    {
        // konstanty
        public const int MAX_ROWS = 9;
        public const int MAX_COLS = 9;
        //
        //private int[,] gameGrid = new int[MAX_ROWS, MAX_COLS];
        //private bool[,] showGrid = new bool[MAX_ROWS, MAX_COLS];
        private SudokuCell[,] sudokuGrid = new SudokuCell[MAX_ROWS, MAX_COLS];
        private int m_free_cels = MAX_ROWS*MAX_COLS;
        private Random rand;

        public SudokuClass()
        {
            int seed = DateTime.Now.Millisecond;
            rand = new Random(seed);

            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    sudokuGrid[i, j] = new SudokuCell();
                    sudokuGrid[i, j].CellX = j;
                    sudokuGrid[i, j].CellY = i;
                }
            }
        }

        public void ClearGrids()
        {
            // vycistit grid
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    sudokuGrid[j, i].ResetCell();
                }
            }
        }

        // pomocne funkce
        private void GenRow(int row)
        {            

            for (int i = 0; i < MAX_COLS; i++)
            {
                sudokuGrid[row, i].Value = 0;
            }

            int count = 0;
            for (int x = 0; x < MAX_COLS; x++)
            {
                bool nalezen = false;
                int r = 0;
                do
                {
                    r=rand.Next(1, 10);
                    nalezen = false;
                    for (int c = 0; c < MAX_COLS; c++)
                    {
                        if (sudokuGrid[row, c].Value == r)
                        {
                            nalezen = true;
                            break;
                        }
                    }
                } while(nalezen == true);

                if (nalezen == false)
                {
                    sudokuGrid[row, x].Value = r;
                }
            }
            RefreshGrid();
        }

        // pomocne funkce
        public int[] GenRow()
        {
            int[] pole = new int[MAX_COLS];

            for (int i = 0; i < MAX_COLS; i++)
            {
                pole[i] = 0;
            }

            int count = 0;
            for (int x = 0; x < MAX_COLS; x++)
            {
                bool nalezen = false;
                int r = 0;
                do
                {
                    r = rand.Next(1, 10);
                    nalezen = false;
                    for (int c = 0; c < MAX_COLS && pole[c] != 0 ; c++)
                    {
                        if (pole[c] == r)
                        {
                            nalezen = true;
                            break;
                        }
                    }
                } while (nalezen == true);

                if (nalezen == false)
                {
                    pole[x] = r;
                }
            }

            return pole;
        }


        // pomocne funkce
        // latain square ... non-canonical
        private void GenTriplet()
        {
            for (int i = 0; i < MAX_COLS; i++)
            {
                sudokuGrid[0, i].Value = 0;
                sudokuGrid[1, i].Value = 0;
                sudokuGrid[2, i].Value = 0;
            }

            int count = 0;
            
            for (int x = 0; x < MAX_COLS; x++)
            {
                bool nalezen = false;
                int r = 0;
                do
                {
                    r = rand.Next(1, 10);
                    nalezen = false;
                    for (int c = 0; c < MAX_COLS; c++)
                    {
                        if (sudokuGrid[0, c].Value == r)
                        {
                            nalezen = true;
                            break;
                        }
                    }
                } while (nalezen == true);

                if (nalezen == false)
                {
                    sudokuGrid[0, x].Value = r;
                }
            }
            
            // canonical
            /*
            for (int c = 0; c < MAX_COLS; c++)
            {
                sudokuGrid[0, c].Value = c+1;
            }*/

            sudokuGrid[1, 6].Value = sudokuGrid[2, 3].Value = sudokuGrid[0, 0].Value;
            sudokuGrid[1, 7].Value = sudokuGrid[2, 4].Value = sudokuGrid[0, 1].Value;
            sudokuGrid[1, 8].Value = sudokuGrid[2, 5].Value = sudokuGrid[0, 2].Value;

            sudokuGrid[1, 0].Value = sudokuGrid[2, 6].Value = sudokuGrid[0, 3].Value;
            sudokuGrid[1, 1].Value = sudokuGrid[2, 7].Value = sudokuGrid[0, 4].Value;
            sudokuGrid[1, 2].Value = sudokuGrid[2, 8].Value = sudokuGrid[0, 5].Value;

            sudokuGrid[2, 0].Value = sudokuGrid[1, 3].Value = sudokuGrid[0, 6].Value;
            sudokuGrid[2, 1].Value = sudokuGrid[1, 4].Value = sudokuGrid[0, 7].Value;
            sudokuGrid[2, 2].Value = sudokuGrid[1, 5].Value = sudokuGrid[0, 8].Value;

            RefreshGrid();
        }

        private void GenBox(int x, int y)
        {
            int[] pole = GenRow();
            int count = 0;

            for (int i = y * 3; i < y * 3 + 3; i++)
            {
                for (int j = x * 3; j < x * 3 + 3; j++)
                {
                    sudokuGrid[i, j].Value = pole[count];
                    count++;
                }
            }

        }



        /// <summary>
        /// Generuje SUDOKU hru
        /// </summary>
        /// <returns>bool</returns>
        public bool GenerateGame()
        {
            this.ClearGrids();

            GenBox(0, 0);
            GenBox(1, 1);
            GenBox(2, 2);
            
            RefreshGrid();
            Console.Write(ToString());
            
            int c = 0;

            while (m_free_cels > 0)
            {
                SudokuCell sudoku = GetMinPossible();
                if (sudoku == null)
                    return false;
                this.GenRandomValue(sudoku);
                RefreshGrid();
                Console.WriteLine("Reseni:");
                Console.Write(ToString());
               //System.Threading.Thread.Sleep(300);
            }
            return true; 
        }

        // zkontroluje hru
        public bool CheckGame()
        {
            int[] cisel = new int[MAX_COLS+1];

            // vynuluj 
            for (int c = 0; c < MAX_COLS; c++)
            {
                cisel[c] = 0;
            }

            // kontroluj radky
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (sudokuGrid[i, j].Value != 0)
                    {
                        //SetPossible(j, i, sudokuGrid[i, j].Value);
                        cisel[sudokuGrid[i, j].Value]++;
                    }
                }
            }

            for (int c = 0; c < MAX_COLS; c++)
            {
                if (cisel[c] > 1)
                    return false;
            }

            // vynuluj 
            for (int c = 0; c < MAX_COLS; c++)
            {
                cisel[c] = 0;
            }

            // kontroluj sloupce
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (sudokuGrid[j, i].Value != 0)
                    {
                        //SetPossible(j, i, sudokuGrid[i, j].Value);
                        cisel[sudokuGrid[j, i].Value]++;
                    }
                }
            }

            for (int c = 0; c < MAX_COLS; c++)
            {
                if (cisel[c] > 1)
                    return false;
            }

            // vynuluj 
            for (int c = 0; c < MAX_COLS; c++)
            {
                cisel[c] = 0;
            }

            // kontrola kvadrantu
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (CheckBox(j, i) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool CheckBox(int x, int y)
        {
            int[] cisel = new int[MAX_COLS];

            for (int i = 0; i < MAX_COLS; i++)
            {
                cisel[i] = 0;
            }

            for (int i = x * 3; i < x * 3 + 3; i++)
            {
                for (int j = y * 3; j < y * 3 + 3; j++)
                {
                    cisel[sudokuGrid[j, i].Value]++;
                }
            }

            for (int c = 0; c < MAX_COLS; c++)
            {
                if (cisel[c] > 1)
                    return false;
            }

            return true;
        }

        // zkontroluje pocet volnych poli
        public int GetFreeCells()
        {
            m_free_cels = MAX_COLS * MAX_ROWS;
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (sudokuGrid[i, j].Value != 0)
                    {
                        //SetPossible(j, i, sudokuGrid[i, j].Value);
                        m_free_cels--;
                    }
                }
            }

            return m_free_cels;
        }


        // najdi nejmensi pocet posible 
        // bereme prvni maly nalezeny
        private SudokuCell GetMinPossible()
        {
            int min_x = MAX_COLS+1, min_y = MAX_ROWS+1, min_val = MAX_ROWS+1;
            SudokuCell min_cel = null;

            for (int y = 0; y < MAX_ROWS; y++)
            {
                for (int x = 0; x < MAX_COLS; x++)
                {
                    SudokuCell cel = sudokuGrid[y, x];

                    if (cel.Value != 0)
                        continue;

                    int pocet = cel.GetPossibleCount(true);
                    if (pocet == 0)
                        return null;
                    if (min_val > pocet)
                    {
                        min_x = x;
                        min_y = y;
                        min_val = pocet;
                        min_cel = cel;
                    }                    
                }
            }

            return min_cel;
        }

        // generuj nahodne cislo do bunky
        private void GenRandomValue(SudokuCell cell)
        {
            for (int c = 1; c <= MAX_COLS; c++)
            {
                if (cell.Possible[c] == true)
                {
                    cell.Value = c;
                    break;
                }
            }
        }

        public SudokuCell[,] GetGrid()
        {
            return sudokuGrid;
        }   

        public bool SetCellValue(int x,int y, int value)
        {
            //zrusit starou hodnotu
            if (CheckCellValue(x, y, value) == false)
            {
                return false;
            }
            if (sudokuGrid[y, x].Value != 0)
                return false;

            sudokuGrid[y, x].Value = value;
            RefreshGrid();

            return true;
        }

        public bool CheckCellValue(int x, int y, int value)
        {
            return sudokuGrid[y, x].Possible[value];
        }

        public bool CheckCellInGrid(SudokuCell cell)
        {
            bool res;
            int x = cell.CellX;
            int y = cell.CellY;
            int value = cell.Value;

            // kontrola v radku
            for (int i = 0; i < 9; i++)
            {
                if (i == x)
                    continue;

                if (sudokuGrid[y, i].Value == value)
                    return false;
            }
            // kontrola ve sloupci
            for (int i = 0; i < 9; i++)
            {
                if (i == y)
                    continue;

                if (sudokuGrid[i, x].Value == value)
                    return false;
                
            }
            // kontrola kvadrantu
            int kv_x = (x) / 3;
            int kv_y = (y) / 3;

            for (int i = kv_y * 3; i < kv_y * 3 + 3; i++)
            {
                for (int j = kv_x * 3; j < kv_x * 3 + 3; j++)
                {
                    if (x == j && y == i)
                        continue;
                    if (sudokuGrid[i, j].Value == value)
                        return false;
                }
            }

            return true;
        }

        public void RefreshGrid()
        {
            // vycistit grid
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    for (int k = 1; k < 10; k++)
                        sudokuGrid[i, j].SetPossible(k, true);
                }
            }

            m_free_cels = MAX_COLS * MAX_ROWS;
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    if (sudokuGrid[i, j].Value != 0)
                    {
                        SetPossible(j, i, sudokuGrid[i, j].Value);
                        m_free_cels--;
                    }
                }
            }
        }

        public void SetPossible(int x, int y, int value)
        {
            // prepracovat pole
            // kontrola v radku
            for (int i = 0; i < 9; i++)
            {
                sudokuGrid[y, i].SetPossible(value, false);
            }
            // kontrola ve sloupci
            for (int i = 0; i < 9; i++)
            {
                sudokuGrid[i, x].SetPossible(value, false);
            }
            // kontrola kvadrantu
            int kv_x = (x) / 3;
            int kv_y = (y) / 3;

            for (int i = kv_y * 3; i < kv_y * 3 + 3; i++)
            {
                for (int j = kv_x * 3; j < kv_x * 3 + 3; j++)
                {
                    sudokuGrid[i, j].SetPossible(value, false);
                }
            }

            sudokuGrid[y, x].Value = value;        
        }




        public void CopyGridValue(SudokuCell[,] grid, int value, int visible, bool up)
        {
            int invisible = MAX_COLS - visible;
            Random r = new Random();

            if (up == true)
            {
                for (int y = 0; y < MAX_ROWS; y++)
                {
                    for (int x = 0; x < MAX_COLS; x++)
                    {
                        SudokuCell sc = grid[y, x];

                        if (sc.Value == value)
                        {
                            if (visible-- <= 0)
                            {
                                return;
                            }
                            this.SetCellValue(x, y, value);
                        }
                    }
                }
            }
            else
            {
                for (int y = MAX_ROWS-1; y >= 0; y--)
                {
                    for (int x = MAX_COLS-1; x >= 0; x--)
                    {
                        SudokuCell sc = grid[y, x];

                        if (sc.Value == value)
                        {
                            if (visible-- <= 0)
                            {
                                return;
                            }
                            this.SetCellValue(x, y, value);
                        }
                    }
                }
            }
        }

        public void CopyRowValue(SudokuCell[,] grid, int row, int visible)
        {
            int[] rownum = GenRow();

            for (int i = 0; i < visible; i++)
            {
                this.SetCellValue(rownum[i]-1, row, grid[row, rownum[i]-1].Value);                
            }            
        }

        public void ResetPossible()
        {
            for (int y = 0; y < MAX_ROWS; y++)
            {
                for (int x = 0; x < MAX_COLS; x++)
                {
                    SudokuCell sc = sudokuGrid[y, x];
                    if (sc.Value == 0)
                        sc.SetPossible(false);
                }
            }
        }

        public string ToString()
        {
            string res = "";

            // vycistit grid
            for (int i = 0; i < MAX_ROWS; i++)
            {
                for (int j = 0; j < MAX_COLS; j++)
                {
                    res += sudokuGrid[i, j].Value + " ";
                }
                res += "\n";
            }

            return res;
        }
    }
}