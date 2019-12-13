﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

using SwedbankPay.Sdk.Exceptions;

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
            var url = "/psp/consumers";

            Exception OnError(ProblemsContainer m)
            {
                return new CouldNotInitiateConsumerSessionException(consumersRequest, m);
            }

            var consumersResponse = await client.HttpRequest<ConsumersResponse>(HttpMethod.Post, url, OnError, consumersRequest);

            return new Consumer(consumersResponse);
        }
    }
}