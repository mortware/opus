using Microsoft.Extensions.Logging;
using NSubstitute;
using Opus.Contracts;
using System.Linq;
using Xunit;

namespace Opus.Services.Tests
{
    public class JobServiceTests
    {
        private readonly ILabourService _labourService = Substitute.For<ILabourService>();
        private readonly IPricingService _pricingService = Substitute.For<IPricingService>();
        private readonly ILogger<JobService> _logger = Substitute.For<ILogger<JobService>>();

        [Fact]
        public void CreateJob_ShouldDecline_WhenTotalPriceExceeds15PercentOfReferencePrice()
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

            _pricingService.UnitCostTyreReplacement().Returns(10);

            var target = CreateTarget();
            var result = target.CreateJob(subject);

            Assert.False(result.Success);
            Assert.Equal(result.Errors.First(), JobService.DeclinedMessage);
        }

        [Fact]
        public void CreateJob_ShouldRefer_WhenTotalPriceIsBetween10And15PercentOfReferencePrice()
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

            _pricingService.UnitCostTyreReplacement().Returns(10);

            var target = CreateTarget();
            var result = target.CreateJob(subject);

            Assert.True(result.Success);
            Assert.True(result.IsReferred);
            Assert.Null(result.Errors);
        }

        [Fact]
        public void CreateJob_ShouldApprove_WhenTotalPriceIsWithin10PercentOfReferencePrice()
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

            _pricingService.UnitCostTyreReplacement().Returns(10);

            var target = CreateTarget();
            var result = target.CreateJob(subject);

            Assert.True(result.Success);
            Assert.Null(result.Errors);
        }

        private JobService CreateTarget()
        {
            var jobService = new JobService(_logger, _pricingService, _labourService);
            return jobService;
        }
    }
}
