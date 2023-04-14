using System;
using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    class Supermarket
    {
        SortedSet<Event> calendar = new SortedSet<Event>();
        List<Cashier> cashiers;
        public int CustomerCount { get; }
        public bool End { get; set; }
        public Supermarket(List<Cashier> cashiers, int customerCount)
        {
            this.cashiers = cashiers;
            CustomerCount = customerCount;
            End = false;
            for (int i = 0; i < cashiers.Count; i++)
                ScheduleEvent(new CheckoutOpenedEvent(this, cashiers[i], 0, 0));
            ScheduleEvent(new NewCustomerEvent(this, cashiers[0], 0, 1));   
        }
        /// Which cashier is either free or (if all busy) has fewest people in queue
        public Cashier? CashierQueue()
        {
            for (int i = 0; i < cashiers.Count; i++)
            {
                if (cashiers[i].Busy == false && cashiers[i].OnBreak == false)
                    return cashiers[i];
            }
            var queueLenghts = cashiers.OrderBy(x => x.Queue.Count).ToList();
            for (int i = 0; i < queueLenghts.Count; i++)
            {
                if (queueLenghts[i].Queue.Count < cashiers[i].MaxQueueLength && queueLenghts[i].OnBreak == false)
                    return queueLenghts[i];
            }
            return null;
        }
        /// core
        public void Run()
        {
            while (calendar.Count > 0)
            {
                Event currentEvent = calendar.Min!;
                calendar.Remove(currentEvent);
                currentEvent.Invoke();                          
            }       
        }
        public void CustomerGoesSomewhere(int currentTime, Cashier cashier, Customer customer)
        {
            Console.WriteLine($"<{currentTime}> [Customer{customer.Id} goes to {cashier.Name}'s queue.]");
        }
        public void AddToQueue(int currentTime, Cashier cashier, Customer customer)
        {
            cashier.Queue.Enqueue(customer);
            //CustomerGoesSomewhere(currentTime, cashier, customer);
            if (!cashier.Busy)
                NextCustomer(currentTime, cashier);       
        }
        private void NextCustomer(int currentTime, Cashier cashier)
        {
            if (cashier.Queue.Count == 0)
            {
                cashier.Busy = false;
                return;
            }
            Customer customer = cashier.ReturnServedCustomer();
            Console.WriteLine($"<{currentTime}> [Customer{customer.Id} started to be served by {cashier.Name}.]");
            ScheduleEvent(new CustomerServedEvent(this, cashier, customer.Id, currentTime + cashier.TimeToServeCustomer));
            cashier.Busy = true;
        }
        public void CustomerLeftQueue(int currentTime, Customer customer, Cashier cashier)
        {
            Console.WriteLine($"<{currentTime}> [Customer{customer.Id} left {cashier.Name}'s queue.]");
        }
        public void CustomerTransfer(int currentTime, Customer customer)
        {
            Console.WriteLine($"<{currentTime}> Customer{customer.Id}: \"Ja se z toho zblaznim. Takze musim do jine fronty.\"");
        }
        public void CashierBreak(int currentTime, Cashier cashier)
        {
            Console.WriteLine($"<{currentTime}> {cashier.Name}: \"Pane sef, ja jdu na zachod. Zaviram tuhle kasu.\"");
        }
        public void CashiersHome(int currentTime, Cashier cashier)
        {
            Console.WriteLine($"<{currentTime}> {cashier.Name}: \"Pane sef, tak ja jdu domu. Zaviram tuhle kasu.\"");
        }
        public void CheckoutOpened(int currentTime, Cashier cashier)
        {
            Console.WriteLine($"<{currentTime}> {cashier.Name}: \"Pane sef, tak ja otviram tuhle kasu. Vzhuru do prace.\"");
        }
        public void CustomerServed(int currentTime, Cashier cashier)
        {
            Console.WriteLine($"<{currentTime}> [Customer{cashier.ServedCustomer.Id} was served by {cashier.Name}.]");
            cashier.Busy = false;
            
            ScheduleEvent(new CustomerLeftEvent(this, cashier, cashier.ServedCustomer.Id, currentTime));
            if (currentTime >= cashier.BreakTime)
                ScheduleEvent(new CashierBreakEvent(this, cashier!, cashier.ServedCustomer.Id, currentTime)); 
            if (End)
                ScheduleEvent(new CashierHomeEvent(this, cashier!, cashier.ServedCustomer.Id, currentTime));
        }
        public void SettingEndsForCashiers(int time)
        {
            foreach (Cashier c in cashiers)
            {
                if (c.Busy == false)
                    ScheduleEvent(new CashierHomeEvent(this, c!, 0, time));
            }
        }
        public void CustomerHappy(int currentTime, Cashier cashier, int customerId)
        {
            Console.WriteLine($"<{currentTime}> Customer{customerId}: \"Na shledanou. Tesim se na pristi navstevu.\"");
        }
        public void CustomerUnhappy(int currentTime, Customer customer)
        {
            Console.WriteLine($"<{currentTime}> Customer{customer.Id}: \"Dyt tady neni volna zadna kasa. Kaslu na cekani, mam dulezitejsi veci na praci.\"");
            Console.WriteLine($"<{currentTime}> Customer{customer.Id}: \"Sbohem. Sem uz teda nikdy nepachnu.\"");
        }
        public void ScheduleEvent(Event e)
        {
            calendar.Add(e);
        }
    }
}