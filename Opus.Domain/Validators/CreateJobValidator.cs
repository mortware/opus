using FluentValidation;
using Opus.Contracts;
using Opus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Opus.Domain.Validators
{
    public class CreateJobValidator : AbstractValidator<CreateJobRequest> 
    {
        public const string MaximumExhaustReplacementCountExceeded = "Maximum number of exhaust replacements exceeded. Value can be no greater than: ";
        public const string MaximumTyreReplacementCountExceeded = "Maximum number of tyre replacements exceeded. Count can be no greater than: ";
        public const string TyreReplacementsMustBePaired = "Tyre replacements must be submitted in pairs";
        public const string MaximumOilChangeCountExceeded = "Maximum number of oil changes exceeded. Count can be no greater than: ";
        public const string BrakeDiscAndPadsMustBePaired = "Brake disc and pad replacements must be submitted in pairs";
        public const string ItemsMustNotBeEmpty = "Items must not be empty";

        private readonly int MaximumExhaustReplacementCount = 1;
        private readonly int MaximumOilChangeCount = 1;
        private readonly int MaximumTyreReplacementCount = 4;

        // TODO: 
        // Split these validation steps into separate validators - open/close principle
        // Fail if more than 1 tyre replacement exists for a given position
        // And these...
        // private readonly int MaximumBrakeDiscReplacementCount = 4;
        // private readonly int MaximumBrakePadReplacementCount = 4;

        public CreateJobValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage(ItemsMustNotBeEmpty);

            RuleFor(x => x.Items)
                .Must(NotExceedMaximumTyreReplacementCount)
                .WithMessage($"{MaximumTyreReplacementCountExceeded}{MaximumTyreReplacementCount}");

            RuleFor(x => x.Items)
                .Must(HavePairedTyreReplacements)
                .WithMessage($"{TyreReplacementsMustBePaired}");

            RuleFor(x => x.Items)
                .Must(HavePairedBrakeDiscAndPadReplacements)
                .WithMessage($"{BrakeDiscAndPadsMustBePaired}");

            RuleFor(x => x.Items)
                .Must(NotExceedMaximumOilChangeCount)
                .WithMessage($"{MaximumOilChangeCountExceeded}{MaximumOilChangeCount}");

            RuleFor(x => x.Items)
                .Must(NotExceedMaximumExhaustReplacementCount)
                .WithMessage($"{MaximumExhaustReplacementCountExceeded}{MaximumExhaustReplacementCount}");

        }

        private bool HavePairedBrakeDiscAndPadReplacements(IEnumerable<IJobItem> items)
        {
            foreach (WheelPosition wheelPosition in Enum.GetValues(typeof(WheelPosition)))
            {
                var discReplacements = items.OfType<BrakeDiscReplacement>().Where(x => x.Position == wheelPosition);
                var padReplacements = items.OfType<BrakePadReplacement>().Where(x => x.Position == wheelPosition);
                return discReplacements.Count() == padReplacements.Count();
            }
            return true;
        }

        private bool HavePairedTyreReplacements(IEnumerable<IJobItem> items)
        {
            if (items.OfType<TyreReplacement>().Any(x => x.Position == WheelPosition.NearsideFront))
                return items.OfType<TyreReplacement>().Any(x => x.Position == WheelPosition.OffsideFront);

            if (items.OfType<TyreReplacement>().Any(x => x.Position == WheelPosition.NearsideRear))
                return items.OfType<TyreReplacement>().Any(x => x.Position == WheelPosition.OffsideRear);

            return true;
        }

        private bool NotExceedMaximumTyreReplacementCount(IEnumerable<IJobItem> items)
        {
            return items.OfType<TyreReplacement>().Count() <= MaximumTyreReplacementCount;
        }

        private bool NotExceedMaximumExhaustReplacementCount(IEnumerable<IJobItem> items)
        {
            return items.OfType<ExhaustReplacement>().Count() <= MaximumExhaustReplacementCount;
        }

        private bool NotExceedMaximumOilChangeCount(IEnumerable<IJobItem> items)
        {
            return items.OfType<OilChange>().Count() <= MaximumOilChangeCount;
        }
    }
}
