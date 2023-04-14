using System;
using System.Collections.Generic;

namespace Supermarket
{
    class CheckoutOpenedEvent : Event
    {
        protected override int Priority => 1;
        public CheckoutOpenedEvent(Supermarket supermarket, Cashier cashier, int customerId, int time) : base(supermarket, cashier, customerId, time) { }
        protected override void Action(int time)
        {
            cashier!.OnBreak = false;
            cashier!.Busy = false;
            Supermarket.CheckoutOpened(time, cashier!);
            if (Supermarket.End) // until last cashier works, it's not end
                Supermarket.ScheduleEvent(new CashierHomeEvent(Supermarket, cashier, cashier.ServedCustomer.Id, time));
        }
    }
}
