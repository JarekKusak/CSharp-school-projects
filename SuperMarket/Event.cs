using System;
using System.Collections.Generic;

namespace Supermarket
{
    abstract class Event : IComparable<Event>
    {
        protected int Time { get; }
        protected abstract int Priority { get; }
        protected Cashier? cashier { get; }
        protected int CustomerId { get; }
        protected Supermarket Supermarket { get; set;}
        protected abstract void Action(int time);
        public Event(Supermarket supermarket, Cashier cashier,int customerId,int time)
        {
            this.cashier = cashier;
            Supermarket = supermarket;
            Time = time;
            CustomerId = customerId;
        }
        public void Invoke()
        {
            Action(Time);
        }
        public int CompareTo(Event other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (Time.CompareTo(other.Time) != 0)
                return Time.CompareTo(other.Time);
            if (CustomerId.CompareTo(other.CustomerId) != 0)
                return CustomerId.CompareTo(other.CustomerId);
            if (Priority.CompareTo(other.Priority) != 0)
                return Priority.CompareTo(other.Priority);
            if (cashier!.Name.CompareTo(other.cashier!.Name) != 0)
                return cashier.Name.CompareTo(other.cashier.Name);
            throw new InvalidCastException("Unable to determine order of the events");
        }
    }
}