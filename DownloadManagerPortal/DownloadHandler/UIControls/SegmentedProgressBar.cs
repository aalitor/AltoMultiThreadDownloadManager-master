
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using AltoMultiThreadDownloadManager;
using System.ComponentModel;

namespace DownloadManagerPortal.DownloadHandler.UIControls
{
    /// <summary>
    /// Description of SegmentedProgressBar.
    /// </summary>
    class SegmentedProgressBar : ProgressBar
    {
        public SegmentedProgressBar()
        {
            ContentLength = 100;
            bars = new Bar[] { };

            Size = new Size(100, 20);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SizeChanged += SegmentedProgressBar_SizeChanged;
        }

        void SegmentedProgressBar_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
        
        
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var grp = e.Graphics;
            step = Width * 1f / contentLength;
            foreach (var br in bars.Where(x=>x.Length > 0))
            {
                var x = br.Start * step;
                var rectF = new RectangleF(x, 0,
                                       br.Length * step, Height);
                grp.FillRectangle(Brushes.Blue, rectF);

                if (br.Status == State.Downloading)
                    using (var p = new Pen(Color.Red, 1f))
                    {
                        var barLineP1 = new PointF(x, 0);
                        var barLineP2 = new PointF(x, Height);
                        grp.DrawLine(p, barLineP1, barLineP2);

                    }
            }

            base.OnPaint(e);
        }

        private long contentLength;
        private float step;
        private Bar[] bars;

        public Bar[] Bars
        {
            get { return bars; }
            set
            {
                bars = value;
                foreach (var element in bars)
                {
                    element.PropertyChanged += element_PropertyChanged;
                }
                Invalidate();
            }
        }

        public long ContentLength
        {
            get { return contentLength; }
            set
            {
                contentLength = value;
                step = Width * 1f / contentLength;
                Invalidate();
            }
        }

        private void element_PropertyChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
    
}
