using FluentValidation;
using System.Collections.Generic;

namespace Opus.Validation
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, IList<TElement>> MaxItems<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int max)
        {
            return ruleBuilder.Must((rootObject, list, context) =>
            {
                context.MessageFormatter
                    .AppendArgument("MaxElements", max)
                    .AppendArgument("TotalElements", list.Count);

                return list.Count <= max;
            })
                .WithMessage("{PropertyName} must not contain more than {MaxElements} items. The list contains {TotalElements} items.");
        }

        public static IRuleBuilderOptions<T, bool?> BoolMustBeValue<T>(this IRuleBuilder<T, bool?> ruleBuilder, bool value)
        {
            return ruleBuilder.Must((rootObject, current, context) => current.HasValue && current.Value == value);
        }
    }
}
