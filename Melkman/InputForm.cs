using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Melkman
{
    public partial class InputForm : Form
    {
        private struct point
        {
            public double x, y;
        }

        private int viraj_stanga(point a, point b, point p)
        {
            double det = (b.x - a.x) * (p.y - a.y) - (b.y - a.y) * (p.x - a.x);
            if (det == 0) return 0;///coliniare
            if (det > 0) return 1;///viraj stanga
            return -1;///viraj dreapta
        }
        public InputForm()
        {
            InitializeComponent();
        }

        ~InputForm()
        {

        }

        private int n;
        private point[] P;
        private Graphics g;
        private OutputForm f;
        private void button1_Click(object sender, EventArgs e)
        {
            string input = richTextBox1.Text;
            string[] numbers = input.Split(' ', '\n', '\t');
            int i;
            P = new point[(numbers.Length - 1) / 2];
            bool ok;
            const int prec = 10;

            if (input.Contains(','))
                ok = false;
            else
                ok = true;
            if (int.TryParse(numbers[0], out n) && ok)
            {
                if (numbers.Length != 2 * n + 1 || n < 3)
                    ok = false;
                if (ok)
                for (i = 2; i <= 2 * n; i += 2)
                {
                    if (!(double.TryParse(numbers[i-1], out P[(i / 2) - 1].x)))
                        ok = false;
                    if (!(double.TryParse(numbers[i], out P[(i / 2) - 1].y)))
                        ok = false;
                }  
            }
            else
                ok = false;

            if (!ok)
            {
                MessageBox.Show("Invalid input!");
                return;
            }


            button1.Enabled = false;
            button2.Enabled = true;
            f = new OutputForm();
            f.FormClosed += (obj, a) => button1.Enabled = true;
            f.FormClosed += (obj, a) => button2.Enabled = false;
            f.Show();

            int o = f.get_twnd_client_size()/2;
            g = f.GetGraphics();
            Brush brush = new SolidBrush(Color.White);
            Pen pen = new Pen(brush, 1);
            g.DrawLine(pen, o, 0, o, o * 2);//OY
            g.DrawLine(pen, 0, o, o * 2, o);//OX
            brush.Dispose();
            pen.Dispose();

            brush = new SolidBrush(Color.Yellow);
            pen = new Pen(brush, 1);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
            point[] rawCoords = new point[P.Length];//Coordonate netranslatate
            for (i = 0; i < n; i++)
            {
                rawCoords[i] = P[i];
                P[i].x = (int)(P[i].x * prec) + o;
                P[i].y = o  - (int)(P[i].y * prec);
            }
            for (i = 1; i < n; i++)
                g.DrawLine(pen, (int)P[i-1].x, (int)P[i-1].y, (int)P[i].x, (int)P[i].y);
            g.DrawLine(pen, (int)P[i-1].x, (int)P[i-1].y, (int)P[0].x, (int)P[0].y);

            brush.Dispose();
            pen.Dispose();

            //ALGORITMUL LUI MELKMAN
            LinkedList<point> deque = new LinkedList<point>();
            deque.AddFirst(rawCoords[2]);
            if (viraj_stanga(rawCoords[0], rawCoords[1], rawCoords[2]) == -1)///sens trigonometric daca parcurg dequeue din stanga in dreapta
            {
                deque.AddLast(rawCoords[1]);
                deque.AddLast(rawCoords[0]);
            }
            else//sens orar convertit in trigonometric
            {
                deque.AddLast(rawCoords[0]);
                deque.AddLast(rawCoords[1]);
            }
            deque.AddLast(rawCoords[2]);

            i = 3;
            while (i < n)
            {
                while (i < n && viraj_stanga(deque.First.Value, deque.First.Next.Value, rawCoords[i]) == 1 
                && viraj_stanga(deque.Last.Previous.Value, deque.Last.Value, rawCoords[i]) == 1)///rawCoords[i] e in interiorul acoperirii curente deci poate fi ignorat
                    i++;
                if (i < n)
                {
                    ///mereu voi avea cel putin 3 puncte care sa determine frontiera unei acoperiri convex
                    while (viraj_stanga(deque.First.Value, deque.First.Next.Value, rawCoords[i]) != 1)
                        deque.RemoveFirst();//deque.First e in interiorul noii acoperiri si trebuie sters

                    while (viraj_stanga(deque.Last.Previous.Value, deque.Last.Value, rawCoords[i]) != 1)
                        deque.RemoveLast();//deque.Last e in interiorul noii acoperiri si trebuie sters

                    deque.AddFirst(rawCoords[i]);
                    deque.AddLast(rawCoords[i]);
                    i++;
                }
            }

            n = 0;
            while (deque.Count != 1)
            {
                Console.WriteLine("{0} {1}",deque.First.Value.x, deque.First.Value.y);
                P[n] = deque.First.Value;
                P[n].x = (int)(P[n].x * prec) + o;
                P[n].y = o - (int)(P[n].y * prec);
                n++;
                deque.RemoveFirst();
            }
            Console.WriteLine();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Brush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(brush, 4);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

            int i;
            for (i = 1; i < n; i++)
            {
                g.DrawLine(pen, (int)P[i - 1].x, (int)P[i - 1].y, (int)P[i].x, (int)P[i].y);
                g.FillEllipse(brush, (int)P[i - 1].x - 5, (int)P[i - 1].y - 5, 10, 10);
            }
            g.DrawLine(pen, (int)P[i - 1].x, (int)P[i - 1].y, (int)P[0].x, (int)P[0].y);
            g.FillEllipse(brush, (int)P[i - 1].x - 5, (int)P[i - 1].y - 5, 10, 10);

            brush.Dispose();
            pen.Dispose();

            f.Refresh();
        }
    }
}
