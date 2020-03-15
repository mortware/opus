using Opus.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Services
{
    public interface IJobService
    {

        CreateJobResult CreateJob(CreateJobRequest request);
    }
}
