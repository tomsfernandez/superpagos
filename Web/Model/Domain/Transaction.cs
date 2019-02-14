using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Model.Domain {
    public class Transaction {

        public long Id { get; set; }
        public DateTime Timestamp { get; } = DateTime.Now;
        public List<Movement> Movements { get; set; }

        public bool IsSuccesfull() => Movements
                .Select(x => x.IsSuccesfull)
                .Aggregate(true, (acc, curr) => acc & curr);
        public bool InProcess() => Movements
            .Select(x => x.InProcess)
            .Aggregate(false, (acc, curr) => acc | curr);
        public bool IsRollback() => Movements
            .Select(x => x.IsRollback)
            .Aggregate(false, (acc, curr) => acc | curr);
        public bool Started() => Movements
            .Select(x => x.Started)
            .Aggregate(false, (acc, curr) => acc | curr);

    }
}