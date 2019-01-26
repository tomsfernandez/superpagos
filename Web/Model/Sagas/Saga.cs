using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Model.Sagas {
    public class Saga {

        public List<SagaStage> Stages { get; }

        public Saga(List<SagaStage> stages) {
            Stages = stages;
        }

        public List<string> Start() {
            var errors = new List<string>();
            Stages.TakeWhile(x => RunSuccesfully(x.Start)).Reverse().ToList().ForEach(x => x.Rollback());
            return errors;
        }

        private bool RunSuccesfully(Action f) {
            try {
                f();
            }
            catch (Exception e) {
                return false;
            }
            return true;
        }
    }
}