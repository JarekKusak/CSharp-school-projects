using System;
using System.Collections.Generic;

namespace Supermarket
{
    class NewCustomerEvent : Event
    {
        protected override int Priority => 4;
        private static int currentId = 1; 
        public NewCustomerEvent(Supermarket supermarket, Cashier cashier, int customerId, int time) : base(supermarket, cashier, customerId, time) { }
        protected override void Action(int time)
        {
            Customer customer = new Customer();
            if (CustomerId == 0)
                customer.Id = currentId++;
            else
                customer.Id = CustomerId;
            Cashier cashier = Supermarket.CashierQueue()!;
            if (cashier != null)
                if (time < cashier.BreakTime || cashier.Busy == true)
                {
                    Supermarket.CustomerGoesSomewhere(time, cashier, customer);
                    Supermarket.AddToQueue(time, cashier, customer);
                }  
                else 
                {
                    //customer.NoCustomer();
                    Supermarket.ScheduleEvent(new CashierBreakEvent(Supermarket, cashier!, customer.Id, time));  
                    time--;    
                }
                    
            else
            {
                Supermarket.ScheduleEvent(new CustomerLeftEvent(Supermarket, cashier!, customer.Id, time));
                Supermarket.CustomerUnhappy(time, customer);
            }  

            if (customer.Id+1 <= Supermarket.CustomerCount)
                Supermarket.ScheduleEvent(new NewCustomerEvent(Supermarket, cashier!, customer.Id+1, time+1));   
            else 
            {
                Supermarket.End = true; 
                Supermarket.SettingEndsForCashiers(time);
            }
                     
        }
    }
}
