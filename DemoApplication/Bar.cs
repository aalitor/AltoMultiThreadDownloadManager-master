using System.ComponentModel;
using AltoMultiThreadDownloadManager;

namespace DemoApplication 
{
    class Bar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Bar(long length, long start, State status)
        {
            this.length = length;
            this.start = start;
            this.status = status;
        }

        private long length;
        private long start;
        private State status;

        public State Status
        {
            get { return status; }
            set
            {
                var noteq = value != status;
                status = value;

                if (noteq && PropertyChanged != null)
                    PropertyChanged(this, null);

            }
        }

        public long Start
        {
            get { return start; }

            set
            {
                var noteq = value != start;
                length = value;

                if (noteq && PropertyChanged != null)
                    PropertyChanged(this, null);

            }
        }
        public long Length
        {
            get { return length; }
            set
            {
                var noteq = value != length;
                length = value;

                if (noteq && PropertyChanged != null)
                    PropertyChanged(this, null);

            }
        }



    }
}
