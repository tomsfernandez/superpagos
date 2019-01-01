using System.Collections.Generic;

namespace Web.Kafka {
    public interface Producer {

        bool Publish<T>(T t);
        bool BulkPublish<T>(List<T> list);
    }
}