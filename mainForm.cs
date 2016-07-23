using System;
using System.Windows.Forms;

namespace MonteKarlo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Set_A(MainFormData.Shape.Rectangle);
            Set_B(MainFormData.Shape.Rectangle);

            MainFormData.CountTests = 1000;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void area_b_type_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = comboBox1.Text == @"Окружность" ? MainFormData.Shape.Circle : MainFormData.Shape.Rectangle;

            Set_B(type);
        }

        private void area_a_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = area_a_type.Text == @"Окружность" ? MainFormData.Shape.Circle : MainFormData.Shape.Rectangle;

            Set_A(type);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            MainFormData.B = (double) numericUpDown7.Value;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void Set_A(MainFormData.Shape s)
        {
            switch (s)
            {
                case MainFormData.Shape.Circle:
                    pictureBox1.ImageLocation = @"pictures\circle_a.png";

                    numericUpDown14.Enabled = false;
                    numericUpDown15.Enabled = false;
                    numericUpDown18.Enabled = true;
                    break;

                case MainFormData.Shape.Rectangle:
                    pictureBox1.ImageLocation = @"pictures\rectangle_a.png";

                    numericUpDown14.Enabled = true;
                    numericUpDown15.Enabled = true;
                    numericUpDown18.Enabled = false;
                    break;
            }

            MainFormData.RecentShapeA = s;
        }

        private void Set_B(MainFormData.Shape s)
        {
            switch (s)
            {
                case MainFormData.Shape.Circle:
                    pictureBox2.ImageLocation = @"pictures\circle_b.png";

                    numericUpDown16.Enabled = false;
                    numericUpDown17.Enabled = false;
                    numericUpDown19.Enabled = true;
                    break;

                case MainFormData.Shape.Rectangle:
                    pictureBox2.ImageLocation = @"pictures\rectangle_b.png";

                    numericUpDown16.Enabled = true;
                    numericUpDown17.Enabled = true;
                    numericUpDown19.Enabled = false;
                    break;
            }

            MainFormData.RecentShapeB = s;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            MainFormData.CountTests = (int) numericUpDown5.Value;
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            MainFormData.A = (double) numericUpDown6.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            MainFormData.A_and_B = (double) numericUpDown8.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            MainFormData.A_or_B = (double) numericUpDown9.Value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MainFormData.LeftDown = new Point_2D((double) I_Xn.Value, (double) I_Yn.Value);
            MainFormData.RightUp  = new Point_2D((double) I_Xk.Value, (double) I_Yk.Value);

            if (MainFormData.RecentShapeA == MainFormData.Shape.Rectangle)
            {
                Point_2D leftDown = new Point_2D((double) numericUpDown10.Value, (double) numericUpDown11.Value);
                Point_2D rightUp = new Point_2D((double) numericUpDown14.Value, (double) numericUpDown15.Value);

                MainFormData.ShapeA = new Rectangle(leftDown, rightUp);
            }
            else
            {
                Point_2D center = new Point_2D((double) numericUpDown10.Value, (double) numericUpDown11.Value);
                MainFormData.ShapeA = new Circle(center, (double) numericUpDown18.Value);
            }

            if (MainFormData.RecentShapeB == MainFormData.Shape.Rectangle)
            {
                Point_2D leftDown = new Point_2D((double) numericUpDown12.Value, (double) numericUpDown13.Value);
                Point_2D rightUp = new Point_2D((double) numericUpDown16.Value, (double) numericUpDown17.Value);

                MainFormData.ShapeB = new Rectangle(leftDown, rightUp);
            }
            else
            {
                Point_2D center = new Point_2D((double) numericUpDown12.Value, (double) numericUpDown13.Value);
                MainFormData.ShapeB = new Circle(center, (double) numericUpDown19.Value);
            }

            solvingForm f = new solvingForm { Owner = this };
            f.ShowDialog();
        }
    }

    public static class MainFormData
    {
        public enum Shape { Circle, Rectangle }

        public static Shape RecentShapeA { get; set; }
        public static Shape RecentShapeB { get; set; }

        public static MonteKarlo.Shape ShapeA { get; set; }
        public static MonteKarlo.Shape ShapeB { get; set; }

        public static int CountTests { get; set; }

        public static Point_2D LeftDown { get; set; }
        public static Point_2D RightUp { get; set; }

        public static double A { get; set; }
        public static double B { get; set; }
        public static double A_and_B { get; set; }
        public static double A_or_B { get; set; } 
    }
}
