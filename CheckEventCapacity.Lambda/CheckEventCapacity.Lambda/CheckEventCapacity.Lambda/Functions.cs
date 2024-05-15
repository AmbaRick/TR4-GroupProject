using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using CheckEventCapacity.Lambda.Interfaces;
using CheckEventCapacity.Lambda.Entities;

namespace CheckEventCapacity.Lambda
{
    





    public class Functions
    {
        private readonly IEventRepository repo;
        public Functions(IEventRepository _repo)
        {
            repo = _repo;
        }

        [LambdaFunction]
        public async Task<string>  FunctionHandler(string input, ILambdaContext context)
        {
            Event eventResult = await repo.Get("string");

            return eventResult.eventName;
        }
    }
}
