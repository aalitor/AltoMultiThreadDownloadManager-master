using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.Exceptions
{
    public class ReturnedContentSizeWrongException : Exception
    {
        public override string Message
        {
            get
            {
                return string.Format("Returned content size is wrong Start={0}, End={1}, Returned={2}, Shouldbe={3}",
                           Start, End, ReturnedContentSize, RealContentSize);
            }
        }

        public ReturnedContentSizeWrongException(long start, long end, long returned, long real)
        {
            Start = start;
            End = end;
            ReturnedContentSize = returned;
            RealContentSize = real;
        }

        public long ReturnedContentSize { get; set; }
        public long RealContentSize { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
    }
}
