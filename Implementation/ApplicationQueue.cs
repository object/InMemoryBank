using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation
{
    public class ApplicationQueue<T> : ConcurrentQueue<T>
    {
        private Func<T,bool> messageHandler;

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

        public void SubscribeWithHandler(Func<T,bool> action)
        {
            this.messageHandler = action;
            OnEnqueued += ((sender, args) =>
            {
                T message = default(T);
                if (TryDequeue(out message))
                {
                    try
                    {
                        if (!this.messageHandler(message))
                        {
                            base.Enqueue(message);
                        }
                    }
                    catch {}
                }
            });
        }

        public void Clear()
        {
            lock (this)
            {
                T message;
                while (this.Count > 0)
                {
                    TryDequeue(out message);
                }
            }
        }
    }
}
