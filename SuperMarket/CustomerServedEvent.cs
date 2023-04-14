using System;
using System.Collections.Generic;

namespace Supermarket
{
    class CustomerServedEvent : Event
    {
        protected override int Priority => 5;
        public CustomerServedEvent(Supermarket supermarket, Cashier cashier, int customerId, int time) : base(supermarket, cashier, customerId, time) { }
        protected override void Action(int time)
        {
            Supermarket.CustomerServed(time, cashier!);
        }
    }
}
