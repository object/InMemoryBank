using System;

namespace Implementation.Bank
{
    public enum NotificationTopic
    {
        None,
        PaymentSent,
        PaymentReceived,
        PayerNotRegistered,
        CollectorNotRegistered,
        InsufficientFunds
    }
}