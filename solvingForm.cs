using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MonteKarlo
{
    public partial class solvingForm : Form
    {
        MainForm _main;
        private PointGenerator generator;

        private int RecentPoint;

        private double Area;
        private bool isDrawed = false;
        private bool isRecalculatable = false;
        private Color colorA = Color.Chartreuse;
        private Color colorB = Color.CadetBlue;

        #region Данные для заполнения в форму

        private int inA = 0;
        private int inB = 0;
        private int inAorB = 0;
        private int inAandB = 0;

        #endregion

        public solvingForm()
        {
            _main = this.Owner as MainForm;
            InitializeComponent();

            generator = new PointGenerator(MainFormData.LeftDown, MainFormData.RightUp);
            textBox18.Text = MainFormData.CountTests.ToString();

            Area = (MainFormData.RightUp.x - MainFormData.LeftDown.x)*(MainFormData.RightUp.y - MainFormData.LeftDown.y);

            if (Math.Abs(MainFormData.A) < double.Epsilon)
                textBox9.Text = @"Не определено";
            if (Math.Abs(MainFormData.B) < double.Epsilon)
                textBox10.Text = @"Не определено";
            if (Math.Abs(MainFormData.A_and_B) < double.Epsilon)
                textBox11.Text = @"Не определено";
            if (Math.Abs(MainFormData.A_or_B) < double.Epsilon)
                textBox12.Text = @"Не определено";

            timer1.Interval = 1;
        }

        void Model()
        {
            while (RecentPoint < MainFormData.CountTests)
            {
                ++RecentPoint;
                var x = generator.Generate();
                ReCalculate(x);
                x.Draw(pictureBox1);

                textBox17.Text = RecentPoint.ToString();
            }
        }

        private void ReCalculate(Point_2D x)
        {
            bool isInA = MainFormData.ShapeA.IsPointIn(x);
            bool isInB = MainFormData.ShapeB.IsPointIn(x);

            if (isInA) ++inA;
            if (isInB) ++inB;
            if (isInA && isInB) ++inAandB;
            if (isInA || isInB) ++inAorB;

            #region Вывод количества попаданий точек в различные области
            
            textBox1.Text = inA.ToString(); // A
            textBox2.Text = inB.ToString(); // B
            textBox3.Text = inAandB.ToString(); // A && B
            textBox4.Text = inAorB.ToString(); // A || B
            
            #endregion

            #region Вывод расчетной вероятности

            double PA = (double) inA / RecentPoint; // 5
            double PB = (double) inB / RecentPoint; // 6
            double PAandB = (double) inAandB / RecentPoint; // 7
            double PAorB = (double) inAorB / RecentPoint; // 8

            // P(A)
            textBox5.Text = PA.ToString(CultureInfo.InvariantCulture);
            // P(B)
            textBox6.Text = PB.ToString(CultureInfo.InvariantCulture);
            // P(A * B)
            textBox7.Text = PAandB.ToString(CultureInfo.InvariantCulture);
            // P(A + B)
            textBox8.Text = PAorB.ToString(CultureInfo.InvariantCulture);

            #endregion

            #region Вывод относительной погрешности

            if (Math.Abs(MainFormData.A) > double.Epsilon)
                textBox9.Text = Math.Abs(100 * (1 - PA / MainFormData.A)).ToString(CultureInfo.InvariantCulture);

            if (Math.Abs(MainFormData.B) > double.Epsilon)
                textBox10.Text = Math.Abs(100 * (1 - PB / MainFormData.B)).ToString(CultureInfo.InvariantCulture);

            if (Math.Abs(MainFormData.A_and_B) > double.Epsilon)
                textBox11.Text = Math.Abs(100 * (1 - PAandB / MainFormData.A_and_B)).ToString(CultureInfo.InvariantCulture);

            if (Math.Abs(MainFormData.A_or_B) > double.Epsilon)
                textBox12.Text = Math.Abs(100 * (1 - PAorB / MainFormData.A_or_B)).ToString(CultureInfo.InvariantCulture);

            #endregion

            #region Вывод расчетной площади

            textBox13.Text = (Area * PA).ToString(CultureInfo.InvariantCulture); // S(A)
            textBox14.Text = (Area * PB).ToString(CultureInfo.InvariantCulture); // S(B)
            textBox15.Text = (Area * PAandB).ToString(CultureInfo.InvariantCulture); // S(A * B)
            textBox16.Text = (Area * PAorB).ToString(CultureInfo.InvariantCulture); // S(A + B)

            #endregion
        }

        private void DrawPoint(Point_2D x)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReDraw();
            inA = 0;
            inB = 0;
            inAandB = 0;
            inAorB = 0;

            RecentPoint = 0;
            
            if(isRecalculatable)
                timer1.Start();
            else
                Model();
        }

        private void DrawFigures()
        {
            MainFormData.ShapeA.DrawAndFill(pictureBox1, colorA);
            MainFormData.ShapeB.DrawAndFill(pictureBox1, colorB);
            MainFormData.ShapeA.DrawCircuit(pictureBox1);
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ReDraw();
            isDrawed = true;
        }

        private void ReDraw()
        {
            pictureBox1.Refresh();
            DrawFigures();
        }

        void TimerModel()
        {
            ++RecentPoint;
            var x = generator.Generate();
            ReCalculate(x);
            x.Draw(pictureBox1);

            textBox17.Text = RecentPoint.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimerModel();

            if(RecentPoint == MainFormData.CountTests)
                timer1.Stop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isRecalculatable = !isRecalculatable;
        }
    }
}
