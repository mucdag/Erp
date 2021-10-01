using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.CrossCuttingConcerns.Validation.FluentValidation
{
    public class PolymorphicValidator<TInterface> : ChildValidatorAdaptor
    {
        Dictionary<Type, IValidator> _derivedValidators = new Dictionary<Type, IValidator>();

        // Need the base constructor call, even though we're just passing null.
        public PolymorphicValidator() : base((IValidator)null, typeof(IValidator<TInterface>))
        {
        }


        public PolymorphicValidator<TInterface> Add<TDerived>(IValidator<TDerived> derivedValidator) where TDerived : TInterface
        {
            _derivedValidators[typeof(TDerived)] = derivedValidator;
            return this;
        }

        public override IValidator GetValidator(PropertyValidatorContext context)
        {
            // bail out if the current item is null 
            if (context.PropertyValue == null) return null;

            var validationResult = new ValidationResult();
            foreach (var item in _derivedValidators)
            {
                var result = item.Value.Validate(new ValidationContext(context.PropertyValue));
                result.Errors.ToList().ForEach(x => validationResult.Errors.Add(x));
            }

            if (validationResult.Errors.Count > 0)
            {
                var value = validationResult.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.ToList().Select(y => y.ErrorMessage));

                throw new ValidationException(JsonConvert.SerializeObject(value));
            }

            if (_derivedValidators.TryGetValue(context.PropertyValue.GetType(), out var derivedValidator))
            {
                return derivedValidator;
            }

            return null;
        }
    }
}
