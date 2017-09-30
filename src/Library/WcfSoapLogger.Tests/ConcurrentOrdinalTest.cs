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
    public class ConcurrentOrdinalTest
    {
        [TestMethod]
        public void MultipleTasks()
        {
            const string key = "'alpha";
            DateTime datetime = DateTime.Now;
            const int count = 100;

            var taskArray = Enumerable.Range(0, count).Select(x => 
                new Task<int>(() =>
                {
                    Trace.WriteLine("ThreadId = " + Thread.CurrentThread.ManagedThreadId);
                    return ConcurrentIndex.GetUniqueIndex(key, datetime);
                }
            )).ToArray();

            Array.ForEach(taskArray, task => task.Start());
            Task.WaitAll(taskArray);

            int uniqueCount = new HashSet<int>(taskArray.Select(task => task.Result)).Count;
            Assert.AreEqual(count, uniqueCount);
        }


        [TestMethod]
        public void MultipleThreads() {
            const string key = "'alpha";
            DateTime datetime = DateTime.Now;
            const int count = 100;

            var results = new int[count];
        
            var threadArray = Enumerable.Range(0, count).Select(x => 
                new Thread(() =>
                {
                    Trace.WriteLine("ThreadId = " + Thread.CurrentThread.ManagedThreadId);
                    int threadIndex = x;
                    int index = ConcurrentIndex.GetUniqueIndex(key, datetime);
                    results[threadIndex] = index;
                }
            )).ToArray();

            Array.ForEach(threadArray, thread => { thread.Start(); } );
            Array.ForEach(threadArray, thread => { thread.Join(); } );

            int uniqueCount = new HashSet<int>(results).Count;
            Assert.AreEqual(count, uniqueCount);
        }



    }
}
