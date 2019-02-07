using System.Collections.Generic;
using System.Linq;
using Kendo.DynamicLinq;

namespace Web.Extensions {
    public static class KendoGridSearchRequestExtensions {
        public static DataSourceResult ToDataSourceResult<T>(this IQueryable<T> queryable,
            KendoGridSearchRequest request) {
            //request?.filter?.Filters?.ToList().ForEach(f => Console.WriteLine($"KendoFilter: {JsonConvert.SerializeObject(f)}"));
            return queryable.ToDataSourceResult(request.take, request.skip, request.sort, request.filter,
                request.aggregates);
        }

    }

    public class KendoGridSearchRequest {
        public int page;
        public int pageSize;
        public int take;
        public int skip;
        public IEnumerable<Sort> sort;
        public Filter filter;
        public IEnumerable<Aggregator> aggregates;
    }
}