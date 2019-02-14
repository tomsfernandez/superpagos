

using System.Collections.Generic;
using System.Linq;
using NLog.Web.LayoutRenderers;
using Web.Model.Domain;
using Web.Service.Provider;

namespace Web.Model {
    public class MovementSaga {

        public ProviderApiFactory Factory { get; set; }
        public AppDbContext Context { get; set; }
        public string ResponseEndpoint { get; set; }

        public bool Start(List<Movement> movements) {
            var startedOperations = movements.TakeWhile(x => x.Start(Factory, 
                Context, ResponseEndpoint)).ToList();
            Context.SaveChanges();
            if (startedOperations.Count.Equals(movements.Count)) {
                return true;
            }
            startedOperations.Reverse();
            startedOperations.ForEach(x => x.Rollback(Factory, Context).Wait());
            return false;
        }
    }
}