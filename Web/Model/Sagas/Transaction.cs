using System;
using System.Collections.Generic;
using System.Linq;
using Web.Model.Domain;

namespace Web.Model.Sagas {
    public class Transaction {
        
        public string Description { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public List<SagaStage> Stages { get; set; }

        public Transaction(string description, IEnumerable<ConcreteSagaStage> stages) {
            Description = description;
            Stages = new List<SagaStage>(stages);
        }

        public bool Started {
            get { return Stages.Select(x => x.IsStarted()).Aggregate(false, (acc, curr) => acc | curr); }
        }

        public bool Success {
            get { return Stages.Select(x => x.IsSuccess()).Aggregate(false, (acc, curr) => acc | curr); }
        }
    }
}