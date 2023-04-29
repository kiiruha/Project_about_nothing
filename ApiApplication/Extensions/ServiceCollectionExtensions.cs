using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ApiApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        #region Public methods

        public static IServiceCollection ConfigureAndValidate<T>(this IServiceCollection serviceCollection, IConfiguration config, string name = null) where T : class
        {
            var sectionName = string.IsNullOrEmpty(name) ? typeof(T).Name : name;
            return serviceCollection
                           .Configure<T>(config.GetSection(sectionName))
                           .PostConfigure<T>(settings =>
                           {
                               var configErrors = settings.GetValidationErrors().ToArray();
                               if (configErrors.Any())
                               {
                                   var aggregatedErrors = string.Join(",", configErrors);
                                   throw new ApplicationException(
                                       $"Found {configErrors.Length} configuration error(s) in {typeof(T).Name}: {aggregatedErrors}");
                               }
                           });
        }

        #endregion

        #region Private methods

        private static IEnumerable<string> GetValidationErrors(this object obj)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);
            foreach (var validationResult in results)
            {
                yield return validationResult.ErrorMessage;
            }
        }

        #endregion
    }
}