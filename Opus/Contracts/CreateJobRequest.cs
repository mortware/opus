using System.Collections.Generic;

namespace Opus.Contracts
{
    public class CreateJobRequest
    {
        public decimal ReferenceLabourInMinutes { get; set; }
        public decimal ReferencePrice { get; set; }

        public IEnumerable<IJobItem> Items { get; set; }
    }
}