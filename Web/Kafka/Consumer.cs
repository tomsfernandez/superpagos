using System;
using System.Collections.Generic;

namespace Web.Kafka {
    public interface Consumer<T> {

        void StartConsuming(List<string> topics, Action<T> onMessageAction, Func<bool> shutdown);
    }
}