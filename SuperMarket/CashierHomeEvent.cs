using System;
using System.Collections.Generic;

namespace Supermarket
{
    class CashierHomeEvent : Event
    {
        protected override int Priority => 6;
        public CashierHomeEvent(Supermarket supermarket, Cashier cashier, int customerId, int time) : base(supermarket, cashier, customerId, time) { }
        protected override void Action(int time)
        {
            Supermarket.CashiersHome(time, cashier!);
        }
    }
}
