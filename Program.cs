using System;
using System.Drawing;
using System.Windows.Forms;

namespace MonteKarlo
{
    static class Program  // номер 29
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class PointGenerator
    {
        /// <summary>
        /// Левая нижняя точка
        /// </summary>
        public Point_2D LeftDown { get; private set; }

        /// <summary>
        /// Правая верхняя точка
        /// </summary>
        public Point_2D RightUp { get; private set; }
        Random _rd;

        /// <summary>
        /// Создает генератор точек в указанной области
        /// </summary>
        /// <param name="leftDown">Левая нижняя точка прямоугольника</param>
        /// <param name="rightUp">Правая верхняя точка прямоугольника</param>
        public PointGenerator(Point_2D leftDown, Point_2D rightUp)
        {
            if ((rightUp.x < leftDown.x) || (leftDown.y > rightUp.y))
                throw new ArgumentException("Ошибка! Левая нижняя точка не может превосходить праую верхнюю!");

            this.LeftDown = leftDown;
            this.RightUp = rightUp;
            this._rd = new Random();
        }

        public PointGenerator(int x1, int y1, int x2, int y2)
            : this(new Point_2D(x1, y1), new Point_2D(x2, y2))
        { }

        /// <summary>
        /// Генерирует точку, попадающую в прямоугольную область
        /// </summary>
        /// <returns>Случайную точку на плоскости</returns>
        public Point_2D Generate()
        {
            double x = LeftDown.x + _rd.NextDouble() * (RightUp.x - LeftDown.x);
            double y = LeftDown.y + _rd.NextDouble() * (RightUp.y - LeftDown.y);

            return new Point_2D(x, y);
        }
    }

    public class Point_2D
    {
        public double x;
        public double y;

        public Point_2D(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", x, y); 
        }

        public static double LengthTo(Point_2D p1, Point_2D p2)
        {
            return Math.Sqrt(Sqr(p2.x - p1.x) + Sqr(p2.y - p1.y));
        }

        private static double Sqr(double x)
        {
            return x*x;
        }
        public void Draw(PictureBox g)
        {

            var rect = new RectangleF((float)(x * DrawConst), (float) (g.Height - y * DrawConst),
                    (float) 3.0, (float) 3.0);
            g.CreateGraphics()
                .DrawEllipse(new Pen(Color.DarkOrange), rect);
            g.CreateGraphics().FillEllipse(new SolidBrush(Color.DarkOrange), rect);
        }

        private readonly double DrawConst = 50;
    }

    public abstract class Shape
    {
        protected static double DrawConst = 50;

        public abstract double Area();
        public abstract bool IsPointIn(Point_2D p);

        public abstract void DrawAndFill(PictureBox g, Color color);
        public abstract void DrawCircuit(PictureBox g);
    }

    /// <summary>
    /// Класс прямоугольника
    /// </summary>
    public class Rectangle : Shape
    { 
        public Point_2D LeftDown { get; private set; }
        public Point_2D RightUp { get; private set; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Width { get { return RightUp.x - LeftDown.x; } }

        /// <summary>
        /// Высота
        /// </summary>
        public double Height { get { return RightUp.y - LeftDown.y; } }

        public Rectangle(Point_2D leftDown, Point_2D rightUp)
        {
            if ((rightUp.x <= leftDown.x) || (leftDown.y >= rightUp.y))
                throw new ArgumentException("Ошибка! Левая нижняя точка не может превосходить праую верхнюю!");

            this.LeftDown = leftDown;
            this.RightUp = rightUp;
        }

        public Rectangle(int x1, int y1, int x2, int y2)
            : this(new Point_2D(x1, y1), new Point_2D(x2, y2))
        { }

        /// <summary>
        /// Площадь прямоугольника
        /// </summary>
        /// <returns></returns>
        public override double Area()
        {
            return (RightUp.x - LeftDown.x)*(RightUp.y - LeftDown.y);
        }

        /// <summary>
        /// Проверяет попадает ли точка прямоугольник
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool IsPointIn(Point_2D p)
        {
            return (LeftDown.x <= p.x && p.x <= RightUp.x) 
                && (LeftDown.y <= p.y && p.y <= RightUp.y);
        }

        public override void DrawAndFill(PictureBox g, Color color)
        {
            DrawCircuit(g);
            g.CreateGraphics().FillRectangle(new SolidBrush(color), (int) (DrawConst * LeftDown.x), (int) (g.Height - DrawConst * (Height + LeftDown.y) - 2),
                    (int) (DrawConst * Width), (int) (DrawConst * Height));
        }

        public override void DrawCircuit(PictureBox g)
        {
            g.CreateGraphics()
               .DrawRectangle(new Pen(Color.Black, (float) 3.0), (int) (DrawConst * LeftDown.x), (int) (g.Height - DrawConst * (Height + LeftDown.y) - 2),
                   (int) (DrawConst * Width), (int) (DrawConst * Height));
        }
    }

    public class Circle : Shape
    {
        public Point_2D Center { get; }
        public double Radius { get; }

        public Circle(Point_2D center, double radius)
        {
            this.Center = center;
            this.Radius = radius > 0 ? radius : 0;
        }

        /// <summary>
        /// Площадь круга
        /// </summary>
        /// <returns></returns>
        public override double Area()
        {
            return Math.PI*Radius*Radius;
        }

        public override bool IsPointIn(Point_2D p)
        {
            return Point_2D.LengthTo(Center, p) <= Radius;
        }

        public override void DrawAndFill(PictureBox g, Color color)
        {
            float drawDiameter = (float) (2*DrawConst*Radius);

            var rect = new RectangleF((float) (DrawConst * (Center.x - Radius)),
                    (float) (g.Height - DrawConst * (Center.y + Radius) - 2),
                    drawDiameter,
                    drawDiameter);

            g.CreateGraphics()
                .DrawEllipse(new Pen(Color.Black, (float) 5.0), rect);
            g.CreateGraphics().FillEllipse(new SolidBrush(color), rect);
        }

        public override void DrawCircuit(PictureBox g)
        {
            float drawDiameter = (float) (2 * DrawConst * Radius);

            var rect = new RectangleF((float) (DrawConst * (Center.x - Radius)),
                    (float) (g.Height - DrawConst * (Center.y + Radius) - 2),
                    drawDiameter,
                    drawDiameter);

            g.CreateGraphics()
                .DrawEllipse(new Pen(Color.Black, (float) 5.0), rect);
        }
    }
}
