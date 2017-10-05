using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public class TimeIndex
    {
        public DateTime DateTime { get; private set; }
        public int Index { get; private set; }

        private TimeIndex(DateTime dateTime, int index)
        {
            DateTime = dateTime;
            Index = index;
        }

        public TimeIndex GetCopy()
        {
            return new TimeIndex(this.DateTime, this.Index);
        }

        public override string ToString()
        {
            return DateTime.ToString("yyyy-MM-dd HH:mm:ss.fff - ") + Index;
        }


        private static readonly Dictionary<string, TimeIndex> _dicTime = new Dictionary<string, TimeIndex>();
        private static readonly object _lock = new object();

        public static TimeIndex GetUnique(string key) 
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException("'key' is empty.");
            }

            lock (_lock)
            {
                DateTime now = DateTime.Now;
                TimeIndex time;

                if (_dicTime.TryGetValue(key, out time))
                {
                    if (time.DateTime == now)
                    {
                        time.Index++;
                    }
                    else
                    {
                        time.DateTime = now;
                        time.Index = 0;
                    }

                    return time.GetCopy();
                }

                time = new TimeIndex(now, 0);
                _dicTime.Add(key, time);
                return time.GetCopy();
            }
        }
    }
}
