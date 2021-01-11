using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManagerPortal
{
    class TextedProgressBar : Control
    {
         int value;
        int maxValue;
        int minValue;
        Color prColor;

        public TextedProgressBar()
        {
            prColor = Color.LightBlue;
            maxValue = 100;
            minValue = 0;
            value = 0;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Width = 100;
            Height = 20;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            int length = Width * value / maxValue;
            e.Graphics.DrawRectangle(Pens.Silver, 0, 0, Width - 1, Height - 1);
            using (SolidBrush brush = new SolidBrush(prColor))
                e.Graphics.FillRectangle(brush, 1, 1, length - 2, Height - 2);

            Size textSize = TextRenderer.MeasureText(value + "", this.Font);
            int x = (Width - textSize.Width) / 2;
            int y = (Height - textSize.Height) / 2;
            using (SolidBrush textBrush = new SolidBrush(this.ForeColor))
                e.Graphics.DrawString("%" + (value * 100d / maxValue).ToString("0.00"), this.Font, textBrush, x, y);
            base.OnPaint(e);
        }
        
        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value > maxValue)
                    throw new Exception("The value is greater than the maximum value of progress");
                if (value < minValue)
                    throw new Exception("The value is lower than the maximum value of progress");
                this.value = value;
                Invalidate();
            }
        }
        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                Invalidate();
            }
        }
        public int MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
                Invalidate();
            }
        }
        public Color ProgressColor
        {
            get
            {
                return prColor;
            }
            set
            {
                prColor = value;
                Invalidate();
            }
        }
    }
}
