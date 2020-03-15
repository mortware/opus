using Opus.Contracts;
using Opus.Domain.Validators;
using Moq;
using Xunit;
using FluentValidation;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Linq;

namespace Opus.Domain.Tests
{
    public class CreateJobValidatorTests
    {
        [Fact]
        public void ShouldHaveErrorWhenItemsIsEmpty()
        {
            var subject = new CreateJobRequest
            {
                Items = new List<IJobItem>()
            };
            var target = new CreateJobValidator();
            target.ShouldHaveValidationErrorFor(s => s.Items, subject);
        }

        [Fact]
        public void ShouldHaveErrorWhenNumberOfTyresExceedsMaximum()
        {

            var subject = new CreateJobRequest
            {
                Items = Enumerable.Repeat(new TyreReplacement(), 10)
            };
            var target = new CreateJobValidator();
            target.ShouldHaveValidationErrorFor(s => s.Items, subject)
                .WithErrorMessage($"{CreateJobValidator.MaximumTyreReplacementCountExceeded}4");
        }

        [Fact]
        public void ShouldHaveErrorWhenTyreDoesNotHavePair()
        {

            var subject = new CreateJobRequest
            {
                Items = new IJobItem[] { 
                    new TyreReplacement { Position = WheelPosition.NearsideFront },
                    new TyreReplacement { Position = WheelPosition.NearsideFront }
                }
            };
            var target = new CreateJobValidator();
            target.ShouldHaveValidationErrorFor(s => s.Items, subject)
                .WithErrorMessage($"{CreateJobValidator.TyreReplacementsMustBePaired}");
        }

        [Fact]
        public void ShouldNotErrorWhenTyreHasPair()
        {

            var subject = new CreateJobRequest
            {
                Items = new IJobItem[] {
                    new TyreReplacement { Position = WheelPosition.NearsideFront },
                    new TyreReplacement { Position = WheelPosition.OffsideFront }
                }
            };
            var target = new CreateJobValidator();
            target.ShouldNotHaveValidationErrorFor(s => s.Items, subject);
        }

        [Fact]
        public void ShouldHaveErrorWhenBrakeDiscAndPadsAreNotPaired()
        {

            var subject = new CreateJobRequest
            {
                Items = new IJobItem[] {
                    new BrakeDiscReplacement { Position = WheelPosition.NearsideFront },
                    new BrakePadReplacement { Position = WheelPosition.OffsideFront },
                }
            };
            var target = new CreateJobValidator();
            target.ShouldHaveValidationErrorFor(s => s.Items, subject)
                .WithErrorMessage($"{CreateJobValidator.BrakeDiscAndPadsMustBePaired}");
        }

        [Fact]
        public void ShouldNotErrorWhenBrakeDiscAndPadsArePaired()
        {

            var subject = new CreateJobRequest
            {
                Items = new IJobItem[] {
                    new BrakeDiscReplacement { Position = WheelPosition.NearsideFront },
                    new BrakePadReplacement { Position = WheelPosition.NearsideFront },
                }
            };
            var target = new CreateJobValidator();
            target.ShouldNotHaveValidationErrorFor(s => s.Items, subject);
        }
        
        [Fact]
        public void ShouldHaveErrorWhenNumberOfOilChangesExceedsMaximum()
        {

            var subject = new CreateJobRequest
            {
                Items = Enumerable.Repeat(new OilChange(), 10)
            };
            var target = new CreateJobValidator();
            target.ShouldHaveValidationErrorFor(s => s.Items, subject)
                .WithErrorMessage($"{CreateJobValidator.MaximumOilChangeCountExceeded}1");
        }

        [Fact]
        public void ShouldHaveErrorWhenNumberOfExhaustReplacementsExceedsMaximum()
        {

            var subject = new CreateJobRequest
            {
                Items = Enumerable.Repeat(new ExhaustReplacement(), 10)
            };
            var target = new CreateJobValidator();
            target.ShouldHaveValidationErrorFor(s => s.Items, subject)
                .WithErrorMessage($"{CreateJobValidator.MaximumExhaustReplacementCountExceeded}1");
        }
    }
}
