using System.Collections.Generic;
using System.Linq;

namespace Web.Dto
{
    public class PaymentMethodPayload
    {
        public string ProviderCode { get; set; }
        public string OperationTokenFromProvider { get; set; }

        public List<string> Validate(AppDbContext context)
        {
            var errors = new List<string>();
            if (!context.Providers.Any(x => x.Code.Equals(ProviderCode)))
                errors.Add($"Provider with code {ProviderCode} doesnt exist");
            if (string.IsNullOrEmpty(OperationTokenFromProvider))
                errors.Add("The Operation Token is null or empty");
            return errors;
        }
    }
}