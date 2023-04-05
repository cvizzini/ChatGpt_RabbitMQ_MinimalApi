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
    public class CapProductHandler : ICapSubscribe
    {
        private readonly ILogger<CapProductHandler> _logger;

        public CapProductHandler(ILogger<CapProductHandler> logger)
        {
            _logger = logger;
        }

        [CapSubscribe("test.show.product" , Group = "group2")]
        public void ReceiveMessage(ProductDto time)
        {
            _logger.LogInformation("Cap Queue Product:" + time);

        }

        [CapSubscribe("test.show.time")]
        public void ReceiveMessage(DateTime time)
        {
            _logger.LogInformation("message time is:" + time);
        }
    }
}
