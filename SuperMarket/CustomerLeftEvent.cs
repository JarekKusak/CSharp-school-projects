using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket
{
    class CustomerLeftEvent : Event
    {
        protected override int Priority => 2;
        public CustomerLeftEvent(Supermarket supermarket, Cashier cashier, int customerId, int time) : base(supermarket, cashier, customerId, time) { }
        protected override void Action(int time)
        {
            if (cashier != null)
                Supermarket.CustomerHappy(time, cashier, CustomerId);             
        }
    }
}
