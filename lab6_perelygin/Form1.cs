using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace lab6_perelygin
{
    public partial class Form1 : Form
    {
        private Thread[] threads;
        private MovingObject[] objects;
        private static Random globalRandom = new Random(); // Общий генератор случайных чисел

        public Form1()
        {
            // Создание трех объектов
            objects = new MovingObject[]
            {
                new MovingObject(100, 100, 30, Color.Red, 5),
                new MovingObject(200, 200, 40, Color.Blue, 4),
                new MovingObject(300, 300, 50, Color.Green, 3)
            };

            // Запуск потоков для каждого объекта
            threads = new Thread[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                int index = i;
                threads[i] = new Thread(() => MoveObject(objects[index]));
                threads[i].IsBackground = true; // Потоки завершаются с закрытием приложения
                threads[i].Start();
            }

            // Привязка метода для перерисовки окна
            this.Paint += OnPaint;
        }

        private void MoveObject(MovingObject obj)
        {
            while (true)
            {
                obj.X += globalRandom.Next(-2, 2) * obj.Speed;
                obj.Y += globalRandom.Next(-5, 5) * obj.Speed;

                // Обработка коллизии с границами окна
                if (obj.X < 0 || obj.X + obj.Size > this.ClientSize.Width)
                    obj.X = Math.Max(0, Math.Min(obj.X, this.ClientSize.Width - obj.Size));
                if (obj.Y < 0 || obj.Y + obj.Size > this.ClientSize.Height)
                    obj.Y = Math.Max(0, Math.Min(obj.Y, this.ClientSize.Height - obj.Size));

                // Перерисовка объектов в окне
                this.Invalidate();

                Thread.Sleep(500); // Задержка для плавного движения
            }
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var obj in objects)
            {
                using (Brush brush = new SolidBrush(obj.Color))
                {
                    g.FillEllipse(brush, obj.X, obj.Y, obj.Size, obj.Size);
                }
            }
        }
    }

    // Класс для отдельного движущегося объекта
    public class MovingObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; }
        public Color Color { get; }
        public int Speed { get; }

        public MovingObject(int x, int y, int size, Color color, int speed)
        {
            X = x;
            Y = y;
            Size = size;
            Color = color;
            Speed = speed;
        }
    }
}