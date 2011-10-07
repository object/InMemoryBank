using System;

namespace Implementation.Bank
{
    public class PaymentException : Exception
    {
    }

    public class PayerNotRegisteredException : PaymentException
    {
    }

    public class CollectorNotRegisteredException : PaymentException
    {
    }

    public class InsufficientFundsException : PaymentException
    {
    }
}
