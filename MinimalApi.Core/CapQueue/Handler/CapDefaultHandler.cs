using DotNetCore.CAP;
using MinimalApi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MinimalApi.Core.CapQueue.Handler
{
    public class CapDefaultHandler : ICapSubscribe
    {
        private readonly ILogger<CapDefaultHandler> _logger;

        public CapDefaultHandler(ILogger<CapDefaultHandler> logger)
        {
            _logger = logger;
        }

        [CapSubscribe("test.show.product" , Group = "group1")]
        public void ReceiveMessage(ProductDto time)
        {
            _logger.LogInformation("Cap Queue Product:" + time);

        }
    }
}
