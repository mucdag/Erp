using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Core.CrossCuttingConcerns.Validation.FluentValidation
{
    public class FluentValidationTool
    {
        public static void FluentValidate(IValidator validator, object entity, string? ruleSet = null)
        {
            if (entity == null) throw new ValidationException(nameof(entity));

            var list = ((IEnumerable<IValidationRule>)validator).ToList();

            var validationResult = new ValidationResult();

            if (list.Any(x => x.RuleSets.Any(y => string.IsNullOrWhiteSpace(y))))
            {
                var result = validator.Validate(entity);
                result.Errors.ToList().ForEach(x => validationResult.Errors.Add(x));
            }

            if (list.Any(x => !x.RuleSets.Any(y => string.IsNullOrWhiteSpace(y))))
            {
                var result = validator.Validate(new ValidationContext(entity, new PropertyChain(),
                    new RulesetValidatorSelector(ruleSet, "Common")));

                result.Errors.ToList().ForEach(x => validationResult.Errors.Add(x));
            }

            if (validationResult.Errors.Count <= 0) return;

            var value = validationResult.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.ToList().Select(y => y.ErrorMessage));

            throw new ValidationException(JsonConvert.SerializeObject(value));
        }
    }
}