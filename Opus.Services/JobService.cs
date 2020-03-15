using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Opus.Contracts;
using Opus.Domain.Validators;

namespace Opus.Services
{
    public class JobService : IJobService
    {
        public const string ReferredMessage = "Job has been referred due to the calculated total price difference";
        public const string DeclinedMessage = "Job has been declined due to exceeding calculated total price difference";
        public const string UnknownErrorMessage = "Unknown error";

        private readonly ILogger<JobService> _logger;
        private readonly IPricingService _pricingService;
        private readonly ILabourService _labourService;

        private readonly decimal CostApprovalThreshold = 0.1m;
        private readonly decimal CostReferralThresholdMin = 0.1m;
        private readonly decimal CostReferralThresholdMax = 0.15m;
        private readonly decimal CostDeclineThreshold = 0.15m;

        public JobService(ILogger<JobService> logger, IPricingService pricingService, ILabourService labourService)
        {
            _logger = logger;
            _pricingService = pricingService;
            _labourService = labourService;
        }

        public CreateJobResult CreateJob(CreateJobRequest request)
        {
            _logger.LogInformation("Validating CreateJobRequest...");
            var requestValidator = new CreateJobValidator();
            var validationResult = requestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed", validationResult.Errors);
                return CreateJobResult.Failed(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }
                
            // Labour
            var labourInMinutes = CalculateTotalLabour(request.Items);
            if (labourInMinutes > request.ReferenceLabourInMinutes)
            {
                _logger.LogWarning($"CreateJob declined. Calculated labour of '{labourInMinutes}' exceeded given value of '{request.ReferenceLabourInMinutes}'");
                return CreateJobResult.Declined("Calculated total labour exceeds given value");
            }

            // Costs
            // TODO: This logic could use be put into dedicated validators - but that would move this business logic away
            var cost = CalculateTotalCost(request.Items);

            // TODO: Possible divide by zero exception here - add logic to decline if so?
            var difference = Math.Abs(request.ReferencePrice / cost - 1);
            if (difference >= CostReferralThresholdMin && Math.Abs(difference) < CostReferralThresholdMax)
            {
                // TODO: Do something here for referral...
                _logger.LogInformation($"CreateJob referred. Total job cost '{cost}' versus '{request.ReferencePrice}' (Difference: {difference * 100:N2}% between {CostReferralThresholdMin * 100:N2}% and {CostReferralThresholdMax * 100:N2}%)");
                return CreateJobResult.Referred();
            }
            
            if (difference >= CostDeclineThreshold)
            {
                // TODO: Do something here for declining...
                _logger.LogWarning($"CreateJob declined. Total job cost '{cost}' versus '{request.ReferencePrice}' (Difference: {difference * 100:N2}% >= {CostDeclineThreshold * 100:N2}%)");
                return CreateJobResult.Declined(DeclinedMessage);
            }

            if (difference <= CostApprovalThreshold)
            {
                // TODO: Do something here for approval...
                _logger.LogInformation($"CreateJob approved. Total job cost '{cost}' versus '{request.ReferencePrice}' (Difference: {difference * 100:N2}% <= {CostApprovalThreshold * 100:N2}%)");
                return CreateJobResult.Approved();
            }
            else
            {
                // Not enough rules - are the thresholds on a sliding scale? If so, we can fix this with a collection of thresholds instead
                return CreateJobResult.Declined(UnknownErrorMessage);
            }
        }

        private decimal CalculateTotalCost(IEnumerable<IJobItem> items)
        {
            decimal cost = 0;

            // TODO: Run over these using checking class type rather than the value
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case Constants.JobItems.TyreReplacementTypeName:
                        cost += _pricingService.UnitCostTyreReplacement();
                        break;
                    case Constants.JobItems.BrakeDiscReplacementTypeName:
                        cost += _pricingService.UnitCostBrakeDiscReplacement();
                        break;
                    case Constants.JobItems.BrakePadReplacementTypeName:
                        cost += _pricingService.UnitCostBrakePadReplacement();
                        break;
                    case Constants.JobItems.OilChangeTypeName:
                        cost += _pricingService.UnitCostOilChange();
                        break;
                    case Constants.JobItems.ExhaustTypeName:
                        cost += _pricingService.UnitCostExhaustReplacement();
                        break;
                    default:
                        break;
                }
            }
            return cost;
        }

        private decimal CalculateTotalLabour(IEnumerable<IJobItem> items)
        {
            decimal labour = 0;
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case Constants.JobItems.TyreReplacementTypeName:
                        labour += _labourService.UnitLabourTyreReplacement();
                        break;
                    case Constants.JobItems.BrakeDiscReplacementTypeName:
                        labour += _labourService.UnitLabourBrakeDiscReplacement();
                        break;
                    case Constants.JobItems.BrakePadReplacementTypeName:
                        labour += _labourService.UnitLabourBrakePadReplacement();
                        break;
                    case Constants.JobItems.OilChangeTypeName:
                        labour += _labourService.UnitLabourOilChange();
                        break;
                    case Constants.JobItems.ExhaustTypeName:
                        labour += _labourService.UnitLabourExhaustReplacement();
                        break;
                    default:
                        break;
                }
            }
            return labour;
        }
    }
}
