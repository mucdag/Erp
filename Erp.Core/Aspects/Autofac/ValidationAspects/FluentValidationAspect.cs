using Castle.DynamicProxy;
using FluentValidation;
using FluentValidation.Resources;
using Core.CrossCuttingConcerns.Validation.FluentValidation;
using Core.Utilities.Interceptors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Core.Aspects.Autofac.ValidationAspects
{
    public class FluentValidationAspect : MethodInterception
    {
        private readonly string _ruleSet;
        private readonly Type _validatorType;

        public FluentValidationAspect(Type validatorType, string ruleSet)
        {
            _validatorType = validatorType;
            _ruleSet = ruleSet;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var validator = Activator.CreateInstance(_validatorType) as IValidator;
            var validatorEntityType = _validatorType.BaseType?.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t?.GetType() == validatorEntityType ||
                                                     t?.GetType().GetGenericTypeDefinition() == typeof(List<>) &&
                                                     t?.GetType().GenericTypeArguments[0] == validatorEntityType)
                .ToList();
            ValidatorOptions.LanguageManager.Enabled = true;
            ValidatorOptions.LanguageManager.Culture = CultureInfo.CurrentCulture;

            foreach (var paramaterEntity in entities)
            {
                if (paramaterEntity is IList)
                {
                    foreach (var entity in paramaterEntity as IList)
                    {
                        var entityType = entity.GetType().Name;
                        if (entityType == validatorEntityType.Name)
                            FluentValidationTool.FluentValidate(validator, entity, _ruleSet);
                    }
                }
                else
                {
                    var entityType = paramaterEntity.GetType().Name;
                    var validator_entityTypeName = validatorEntityType.Name;
                    if (entityType == validator_entityTypeName)
                        FluentValidationTool.FluentValidate(validator, paramaterEntity, _ruleSet);
                }
            }

            if (!entities.Any()) // parametre null gelirse kontrol
            {
                FluentValidationTool.FluentValidate(validator, null, _ruleSet);
            }
        }
    }
}
