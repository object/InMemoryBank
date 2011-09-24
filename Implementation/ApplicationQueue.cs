using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation
{
    public class ApplicationQueue<T> : ConcurrentQueue<T>
    {
        private Action<T> messageHandler;

        public event EventHandler<EventArgs> OnEnqueued;
        public event EventHandler<EventArgs> OnDequeued;

        public new void Enqueue(T message)
        {
            base.Enqueue(message);
            if (OnEnqueued != null)
            {
                OnEnqueued(this, new EventArgs());
            }
        }

        public new bool TryDequeue(out T message)
        {
            var result = base.TryDequeue(out message);
            if (result && OnDequeued != null)
                OnDequeued(this, new EventArgs());
            return result;
        }
    }
}
