using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    class CashierBreakEvent : Event
    {
        protected override int Priority => 3;
        public CashierBreakEvent(Supermarket supermarket, Cashier cashier, int customerId, int time) : base(supermarket, cashier, customerId, time) { }
        protected override void Action(int time)
        {
            cashier!.OnBreak = true;
            Supermarket.ScheduleEvent(new CheckoutOpenedEvent(Supermarket, cashier, 0, time+7));
            Supermarket.CashierBreak(time, cashier!);
            Stack<Customer> customersToTransfer = new Stack<Customer>(); 
            while (cashier!.Queue.Count != 0)
            {
                Customer customer = cashier.Queue.Dequeue();
                customersToTransfer.Push(customer);
            }
            cashier.Break(time);
            if (customersToTransfer.Count != 0)
            {
                while (customersToTransfer.Count != 0)
                {
                    Customer customer = customersToTransfer.Pop();
                    Supermarket.CustomerTransfer(time, customer);
                    Supermarket.CustomerLeftQueue(time, customer, cashier);
                    Cashier newCashier = Supermarket.CashierQueue();
                    if (newCashier != null)
                    {
                        Supermarket.ScheduleEvent(new NewCustomerEvent(Supermarket, newCashier, customer.Id, time));
                        
                        //Supermarket.AddToQueue(time, newCashier,customer);
                    }
                        
                    else
                        Supermarket.CustomerUnhappy(time, customer);
                }
            }
                
        }
    }
}