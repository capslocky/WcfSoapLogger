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
        public void OneKeyMultipleTasks()
        {
            const string key = "'alpha";
            const int count = 1000;

            var taskArray = Enumerable.Range(0, count).Select(index => 
                new Task<TimeIndex>(() =>
                    {
                        var timeIndex = TimeIndex.GetUnique(key);
                        Trace.WriteLine("Result = " + timeIndex + " Index = " + index.ToString("000") + " ThreadId = " + Thread.CurrentThread.ManagedThreadId);
                        return timeIndex;
                    }
            )).ToArray();

            Array.ForEach(taskArray, task => task.Start());
            Task.WaitAll(taskArray);

            var results = taskArray.Select(task => task.Result).ToArray();

            CheckOneKeyResults(results);
        }


        [TestMethod]
        public void OneKeyMultipleThreads() 
        {
            const string key = "'beta";
            const int count = 1000;

            var results = new TimeIndex[count];
        
            var threadArray = Enumerable.Range(0, count).Select(index => 
                new Thread(() =>
                {
                    var timeIndex = TimeIndex.GetUnique(key);
                    Trace.WriteLine("Result = " + timeIndex + " Index = " + index.ToString("000") + " ThreadId = " + Thread.CurrentThread.ManagedThreadId);
                    results[index] = timeIndex;
                }
            )).ToArray();


            Array.ForEach(threadArray, thread => thread.Start());
            Array.ForEach(threadArray, thread => thread.Join());

            CheckOneKeyResults(results);
        }



        /* should look like this
        2017-10-06 21:20:57.766 / 00
        2017-10-06 21:20:57.767 / 00
        2017-10-06 21:20:57.770 / 00
        2017-10-06 21:20:57.793 / 00
        2017-10-06 21:20:57.814 / 00
        2017-10-06 21:20:57.814 / 01
        2017-10-06 21:20:57.814 / 02
        2017-10-06 21:20:57.814 / 03
        2017-10-06 21:20:57.814 / 04
        2017-10-06 21:20:57.814 / 05
        2017-10-06 21:20:57.815 / 00
        2017-10-06 21:20:57.815 / 01
        2017-10-06 21:20:57.815 / 02
        2017-10-06 21:20:57.818 / 00
        2017-10-06 21:20:57.822 / 00
        2017-10-06 21:20:57.826 / 00
        2017-10-06 21:20:57.832 / 00
        */
        private void CheckOneKeyResults(TimeIndex[] results)
        {
            results = results.OrderBy(x => x.DateTime).ThenBy(x => x.Index).ToArray();

            for (int i = 0; i < results.Length - 1; i++)
            {
                var current = results[i];
                var next = results[i + 1];

                if (next.DateTime == current.DateTime && next.Index == (current.Index + 1))
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


        [TestMethod]
        public void OneKey() 
        {
            const string key = "'gamma";
            const int count = 10000;

            var results = Enumerable.Range(0, count).Select(index => TimeIndex.GetUnique(key)).ToArray();

            CheckOneKeyResults(results);
        }


        [TestMethod]
        public void DifferentKeys() 
        {
            const string keyA = "'keyA";
            const string keyB = "'keyB";
            const string keyC = "'keyC";

            var resultA = TimeIndex.GetUnique(keyA);
            var resultB = TimeIndex.GetUnique(keyB);
            var resultC = TimeIndex.GetUnique(keyC);

            Assert.AreEqual(0, resultA.Index);
            Assert.AreEqual(0, resultB.Index);
            Assert.AreEqual(0, resultC.Index);
        }


    }
}
