using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace LoymaxTest.Controllers
{
    public static class ModelStateDictionaryExtensions
    {
        public static string JoinErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return string.Join(" | ", modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            return string.Empty;
        }
    }
}
