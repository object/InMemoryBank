using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Implementation.Bank
{
    public class CommandProcessor
    {
        private ApplicationQueue<PaymentCommand> commandQueue;
        private Repository repository;

        public CommandProcessor(
            ApplicationQueue<PaymentCommand> commandQueue,
            Repository repository)
        {
            this.commandQueue = commandQueue;
            this.repository = repository;

            this.commandQueue.SubscribeWithHandler(ProcessCommand);
        }

        public void ProcessCommand(PaymentCommand command)
        {
            throw new PayerNotRegisteredException();
        }
    }
}
