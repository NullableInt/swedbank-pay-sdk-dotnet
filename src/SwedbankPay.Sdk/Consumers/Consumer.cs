﻿using System;
using System.Threading.Tasks;

namespace SwedbankPay.Sdk.Consumers
{
    public class Consumer
    {
        private Consumer(ConsumersResponse consumersResponse)
        {
            ConsumersResponse = consumersResponse;
            var operations = new Operations();

            foreach (var httpOperation in consumersResponse.Operations)
            {
                operations.Add(httpOperation.Rel, httpOperation);

                switch (httpOperation.Rel.Value)
                {
                    case ConsumerResourceOperations.RedirectConsumerIdentification:
                        operations.RedirectConsumerIdentification = httpOperation;
                        break;
                    case ConsumerResourceOperations.ViewConsumerIdentification:
                        operations.ViewConsumerIdentification = httpOperation;
                        break;
                }

                Operations = operations;
            }
        }


        public ConsumersResponse ConsumersResponse { get; }

        public Operations Operations { get; }


        internal static async Task<Consumer> Initiate(ConsumersRequest consumersRequest, SwedbankPayHttpClient client)
        {
            var url = new Uri("/psp/consumers", UriKind.Relative);

            var consumersResponse = await client.HttpPost<ConsumersResponse>(url, consumersRequest);

            return new Consumer(consumersResponse);
        }
    }
}