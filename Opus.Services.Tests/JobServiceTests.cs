using Microsoft.Extensions.Logging;
using Moq;
using Opus.Contracts;
using System;
using System.Linq;
using Xunit;

namespace Opus.Services.Tests
{
    public class JobServiceTests
    {
        [Fact]
        public void ShouldDeclineIfTotalPriceExceeds15PercentOfReferencePrice()
        {
            var subject = new CreateJobRequest()
            {
                ReferencePrice = 10, // Should be 20 (therefore 50% cheaper)
                ReferenceLabourInMinutes = 20,
                Items = new IJobItem[]
                {
                    new TyreReplacement { Position = WheelPosition.NearsideFront },
                    new TyreReplacement { Position = WheelPosition.OffsideFront }
                }
            };

            var target = CreateTarget();
            var result = target.CreateJob(subject);

            Assert.False(result.Success);
            Assert.Equal(result.Errors.First(), JobService.DeclinedMessage);
        }

        [Fact]
        public void ShouldReferIfTotalPriceIsBetween10And15PercentOfReferencePrice()
        {
            var subject = new CreateJobRequest()
            {
                ReferencePrice = 17.5m, // Should be 20
                ReferenceLabourInMinutes = 20,
                Items = new IJobItem[]
                {
                    new TyreReplacement { Position = WheelPosition.NearsideFront },
                    new TyreReplacement { Position = WheelPosition.OffsideFront }
                }
            };

            var target = CreateTarget();
            var result = target.CreateJob(subject);

            Assert.True(result.Success);
            Assert.True(result.IsReferred);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void ShouldApproveIfTotalPriceIsWithin10PercentOfReferencePrice()
        {
            var subject = new CreateJobRequest()
            {
                ReferencePrice = 19, // Should be 20
                ReferenceLabourInMinutes = 20,
                Items = new IJobItem[]
                {
                    new TyreReplacement { Position = WheelPosition.NearsideFront },
                    new TyreReplacement { Position = WheelPosition.OffsideFront }
                }
            };

            var target = CreateTarget();
            var result = target.CreateJob(subject);

            Assert.True(result.Success);
            Assert.Null(result.Errors);
        }

        private JobService CreateTarget()
        {
            var labourServiceMock = new Mock<ILabourService>();
            
            labourServiceMock
                .Setup(m => m.UnitLabourBrakeDiscReplacement())
                .Returns(10);

            labourServiceMock
                .Setup(m => m.UnitLabourBrakePadReplacement())
                .Returns(10);

            labourServiceMock
                .Setup(m => m.UnitLabourExhaustReplacement())
                .Returns(10);

            labourServiceMock
                .Setup(m => m.UnitLabourOilChange())
                .Returns(10);

            labourServiceMock
                .Setup(m => m.UnitLabourTyreReplacement())
                .Returns(10);

            var pricingServiceMock = new Mock<IPricingService>();

            pricingServiceMock
                .Setup(m => m.UnitCostBrakeDiscReplacement())
                .Returns(10);

            pricingServiceMock
                .Setup(m => m.UnitCostBrakePadReplacement())
                .Returns(10);

            pricingServiceMock
                .Setup(m => m.UnitCostExhaustReplacement())
                .Returns(10);

            pricingServiceMock
                .Setup(m => m.UnitCostOilChange())
                .Returns(10);

            pricingServiceMock
                .Setup(m => m.UnitCostTyreReplacement())
                .Returns(10);

            var jobService = new JobService(
                new Mock<ILogger<JobService>>().Object,
                pricingServiceMock.Object,
                labourServiceMock.Object);

            return jobService;

        }
    }
}
