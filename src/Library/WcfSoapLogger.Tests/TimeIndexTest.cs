using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WcfSoapLogger.Tests
{
    [TestClass]
    public class TimeIndexTest
    {
        [TestMethod]
        public void MultipleTasksSameKey()
        {
            const string key = "'alpha";
            const int count = 1000;

            var taskArray = Enumerable.Range(0, count).Select(x => 
                new Task<TimeIndex>(() =>
                    {
                        var timeIndex = TimeIndex.GetUnique(key);
                        Trace.WriteLine("Result = " + timeIndex + " ThreadId = " + Thread.CurrentThread.ManagedThreadId);
                        return timeIndex;
                    }
            )).ToArray();

            Array.ForEach(taskArray, task => task.Start());
            Task.WaitAll(taskArray);

            var results = taskArray.Select(task => task.Result).OrderBy(x => x.DateTime).ThenBy(x => x.Index).ToArray();

            for (int i = 0; i < results.Length - 1; i++)
            {
                var current = results[i];
                var next = results[i + 1];

                if(next.DateTime == current.DateTime && next.Index == (current.Index + 1))
                {
                    continue;
                }

                if (next.DateTime > current.DateTime && next.Index == 0)
                {
                    continue;
                }

                Assert.Fail();
            }
        }


//        [TestMethod]
//        public void MultipleThreadsWithSameArguments() 
//        {
//            const string key = "'beta";
//            DateTime datetime = new DateTime(2017, 01, 02);
//            const int count = 100;
//
//            var results = new int[count];
//        
//            var threadArray = Enumerable.Range(0, count).Select(index => 
//                new Thread(() =>
//                {
//                    int result = ConcurrentIndex.GetUniqueIndex(key, datetime);
//                    results[index] = result;
//                    Trace.WriteLine("Result = " + result.ToString("000") + "  Index = " + index.ToString("000") + " ThreadId = " + Thread.CurrentThread.ManagedThreadId);
//                }
//            )).ToArray();
//
//            Array.ForEach(threadArray, thread => thread.Start());
//            Array.ForEach(threadArray, thread => thread.Join());
//
//            int uniqueCount = new HashSet<int>(results).Count;
//            Assert.AreEqual(count, uniqueCount);
//        }


//        [TestMethod]
//        public void OneKeyDifferentDates() 
//        {
//            const string key = "'gamma";
//            DateTime datetimeA = new DateTime(2017, 01, 03);
//            DateTime datetimeB = new DateTime(2017, 01, 04);
//
//            int indexA1 = ConcurrentIndex.GetUniqueIndex(key, datetimeA);
//            int indexA2 = ConcurrentIndex.GetUniqueIndex(key, datetimeA);
//            int indexA3 = ConcurrentIndex.GetUniqueIndex(key, datetimeA);
//
//            int indexB1 = ConcurrentIndex.GetUniqueIndex(key, datetimeB);
//            int indexB2 = ConcurrentIndex.GetUniqueIndex(key, datetimeB);
//            int indexB3 = ConcurrentIndex.GetUniqueIndex(key, datetimeB);
//
//            Assert.AreEqual(0, indexA1);
//            Assert.AreEqual(1, indexA2);
//            Assert.AreEqual(2, indexA3);
//
//            Assert.AreEqual(0, indexB1);
//            Assert.AreEqual(1, indexB2);
//            Assert.AreEqual(2, indexB3);
//        }
//
//
//        [TestMethod]
//        public void DifferentKeysOneDate() 
//        {
//            const string keyA = "'keyA";
//            const string keyB = "'keyB";
//            const string keyC = "'keyC";
//            DateTime datetime = new DateTime(2017, 01, 05);
//
//            int resultA = ConcurrentIndex.GetUniqueIndex(keyA, datetime);
//            int resultB = ConcurrentIndex.GetUniqueIndex(keyB, datetime);
//            int resultC = ConcurrentIndex.GetUniqueIndex(keyC, datetime);
//
//            Assert.AreEqual(0, resultA);
//            Assert.AreEqual(0, resultB);
//            Assert.AreEqual(0, resultC);
//        }



//        [TestMethod]
//        public void OneKey() 
//        {
//            const string key = "'delta";
//            int count = 1000;
//            var results = new int[count];
//            var datetimes = new DateTime[count];
//
//            for (int i = 0; i < count; i++)
//            {
//                DateTime dateTime = DateTime.Now;
//                datetimes[i] = dateTime;
//                results[i] = ConcurrentIndex.GetUniqueIndex(key, dateTime);
//                Thread.Sleep(TimeSpan.FromTicks(1));
//            }
//
//            int z = 1;
//        }



    }
}
