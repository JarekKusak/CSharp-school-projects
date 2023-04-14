using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    class Cashier
    {
        private bool firstBreak = true;
        private static int nextBreakLenghts = 31;
        public string Name { get; }
        public int TimeToServeCustomer;
        public bool OnBreak { get; set; }
        public int MaxQueueLength;
        public bool Busy;
        public int BreakTime;
        public int LenghtOfBreak { get; set; }
        public Queue<Customer> Queue;
        public Customer ServedCustomer { get; set; }
        public Cashier(string name, int timeToServeCustomer, int maxQueueLength, int breakTime)
        {
            Name = name;
            TimeToServeCustomer = timeToServeCustomer;
            MaxQueueLength = maxQueueLength;
            Busy = false;
            BreakTime = breakTime;
            Queue = new Queue<Customer>();
            LenghtOfBreak = 7;
            OnBreak = false;
        }
        
        public void Break(int currentTime)
        {
            Busy = true;
            BreakTime = currentTime + LenghtOfBreak + nextBreakLenghts;
        }
        public Customer ReturnServedCustomer()
        {
            ServedCustomer = Queue.Dequeue();
            return ServedCustomer;
        }
    }
}
