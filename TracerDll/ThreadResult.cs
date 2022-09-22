﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TracerDll
{
    public class ThreadResult
    {
        public int threadId;
        public long time;
        public ConcurrentQueue<MethodResult> childMethods;
        public ThreadResult(int threadId)
        {
            this.threadId = threadId;
            this.childMethods = new ConcurrentQueue<MethodResult>();
        }
        public void CountTime()
        {
            MethodResult[] methodResults = new MethodResult[childMethods.Count];
            childMethods.CopyTo(methodResults, 0);
            this.time = 0;
            foreach(MethodResult methodResult in methodResults)
            {
                this.time += methodResult.time;
            }
        }
    }
}
