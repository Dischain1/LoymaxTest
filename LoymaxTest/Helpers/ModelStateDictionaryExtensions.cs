using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LoymaxTest.Helpers
{
    public static class ModelStateDictionaryExtensions
    {
        public static string JoinErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return string.Join(" | ", modelState.Values
                    .SelectMany(modelStateEntry => modelStateEntry.Errors)
                    .Select(modelError => modelError.ErrorMessage));
            }

            return string.Empty;
        }
    }
}
