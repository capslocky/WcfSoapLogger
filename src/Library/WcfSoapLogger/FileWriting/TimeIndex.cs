// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;

namespace WcfSoapLogger
{
    /* Problem: 
     * There is always a probability to have 2 similar requests at the same moment of time.
     * Which results in file name collisions. Let's take as example '2017-10-06 at 23-04-41-410 @ FindSimilar.xml'
     * 
     * Solution 1:
     * Always add unique part to every file name, in this case it will look like this:
     * '2017-10-06 at 23-04-41-410 @ c2c14a7f-8adb-4a78-8723-d3e0da98036a @ FindSimilar.xml'
     * '2017-10-06 at 23-04-41-410 @ b3ba2f75-0784-4791-8266-df00efe0d678 @ FindSimilar.xml'
     *  
     * Solution 2:
     * Add unique part only if needed. TimeIndex facilitates this approach.
     * '2017-10-06 at 23-04-41-410 @ FindSimilar.xml'
     * '2017-10-06 at 23-04-41-410 @ FindSimilar_1.xml'
     *  
     * Note:
     * By default filenames are generated using only full datetime and soap operation name with solution 2 for collisions.
     * But we can easily implement any custom logging logic. 
     */

    public class TimeIndex
    {
        public const string DateTimeMask = "yyyy-MM-dd HH:mm:ss.fff";

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
            return DateTime.ToString(DateTimeMask) + Index.ToString(" / 00");
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
                    else if (time.DateTime < now)
                    {
                        time.DateTime = now;
                        time.Index = 0;
                    }
                    else
                    {
                        //very rare and strange case (time.DateTime > now)
                        time.Index++;
#if DEBUG
                        throw new InvalidOperationException(string.Format("Debug-only exception. 'time.DateTime > now' '{0}' > '{1}'", 
                            time.DateTime.ToString(DateTimeMask), now.ToString(DateTimeMask)));
#endif
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
