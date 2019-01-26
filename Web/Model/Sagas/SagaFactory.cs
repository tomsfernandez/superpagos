using System.Collections.Generic;

namespace Web.Model.Sagas {
    public interface SagaFactory {

        Saga Build();
    }
}