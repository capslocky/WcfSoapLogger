using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WcfSoapLogger
{
    public static class ConcurrentIndex
    {
        private static readonly Dictionary<string, DateTime> _dicDate = new Dictionary<string, DateTime>();
        private static readonly Dictionary<string, int> _dicIndex = new Dictionary<string, int>();
        private static readonly object _lock = new object();

        public static int GetUniqueIndex(string key, DateTime dateTime) {
            lock (_lock)
            {
                DateTime previousDate;

                if (_dicDate.TryGetValue(key, out previousDate))
                {
                    if (previousDate == dateTime)
                    {
                        int index = _dicIndex[key] + 1;
                        _dicIndex[key] = index ;
                        return index;
                    }

                    _dicIndex[key] = 0;
                    _dicDate[key] = dateTime;
                    return 0;
                }

                _dicDate.Add(key, dateTime);
                _dicIndex.Add(key, 0);
                return 0;
            }
        }

    }
}
