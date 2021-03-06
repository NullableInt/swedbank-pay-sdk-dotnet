﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using SwedbankPay.Sdk.Exceptions;
using SwedbankPay.Sdk.PaymentOrders;
using SwedbankPay.Sdk.Tests.TestBuilders;

using Xunit;

namespace SwedbankPay.Sdk.Tests
{
    public class PaymentOrderTests : ResourceTestsBase
    {
        private readonly PaymentOrderRequestBuilder paymentOrderRequestBuilder = new PaymentOrderRequestBuilder();


        [Fact]
        public async Task CreateAndAbortPaymentOrder_ShouldReturnAbortedState()
        {
            //ARRANGE

            var paymentOrderRequest = this.paymentOrderRequestBuilder.WithTestValues().Build();

            //ACT
            var paymentOrder = await this.Sut.PaymentOrder.Create(paymentOrderRequest, PaymentOrderExpand.All);
            Assert.NotNull(paymentOrder);
            Assert.NotEmpty(paymentOrder.Operations);
            Assert.NotNull(paymentOrder.Operations.Abort);

            var responseContainer = await paymentOrder.Operations.Abort();

            Assert.NotNull(responseContainer);
            Assert.NotNull(responseContainer.PaymentOrderResponseObject);
            Assert.Equal(State.Aborted, responseContainer.PaymentOrderResponseObject.State);
        }



        [Fact]
        public async Task CreateAndGetPaymentOrder_ShouldReturnPaymentOrderWithSameAmountAndMetaData()
        {
            var paymentOrderRequest = this.paymentOrderRequestBuilder.WithTestValues().Build();
            var paymentOrder = await this.Sut.PaymentOrder.Create(paymentOrderRequest, PaymentOrderExpand.All);
            Assert.NotNull(paymentOrder);
            Assert.NotNull(paymentOrder.PaymentOrderResponse);
            var amount = paymentOrder.PaymentOrderResponse.Amount;

            var paymentOrder2 = await this.Sut.PaymentOrder.Get(paymentOrder.PaymentOrderResponse.Id, PaymentOrderExpand.All);
            Assert.NotNull(paymentOrder2);
            Assert.NotNull(paymentOrder2.PaymentOrderResponse);
            Assert.Equal(amount.Value, paymentOrder2.PaymentOrderResponse.Amount.Value);
            
            Assert.Equal(paymentOrderRequest.PaymentOrder.MetaData["key1"], paymentOrder2.PaymentOrderResponse.MetaData["key1"]);
        }


        [Fact]
        public void T()
        {
            string json = "{\"id\": \"/psp/paymentorders/52bab34b-a149-4da4-8bb2-08d7942f0ac7/metadata\",\"key1\": \"value1\",\"key2\": 2,\"key3\": 3.1,\"key4\": false }";
            var deserializeObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        }



        [Fact]
        public async Task CreateAndUpdateOnlyAmountOnPaymentOrder_ShouldThrowHttpResponseException()
        {
            var paymentOrderRequest = this.paymentOrderRequestBuilder.WithTestValues().WithAmounts().Build();
            var paymentOrder = await this.Sut.PaymentOrder.Create(paymentOrderRequest, PaymentOrderExpand.All);
            Assert.NotNull(paymentOrder);
            Assert.NotNull(paymentOrder.PaymentOrderResponse);
            var amount = paymentOrder.PaymentOrderResponse.Amount;

            var newAmount = 50000;
            var updateRequest = new UpdateRequest(Amount.FromDecimal(newAmount), null);
            
            await Assert.ThrowsAsync<HttpResponseException>(() => paymentOrder.Operations.Update?.Invoke(updateRequest));
        }


        [Fact]
        public async Task CreateAndUpdatePaymentOrder_ShouldReturnPaymentOrderWithNewAmounts()
        {
            var paymentOrderRequest = this.paymentOrderRequestBuilder.WithTestValues().Build();

            var paymentOrder = await this.Sut.PaymentOrder.Create(paymentOrderRequest, PaymentOrderExpand.All);
            Assert.NotNull(paymentOrder);
            Assert.NotNull(paymentOrder.PaymentOrderResponse);

            var newAmount = 50000;
            var newVatAmount = 10000;
            var updateRequest = new UpdateRequest(Amount.FromDecimal(newAmount), Amount.FromDecimal(newVatAmount));
            Assert.NotNull(paymentOrder.Operations.Update);

            var response = await paymentOrder.Operations.Update?.Invoke(updateRequest);

            Assert.Equal(updateRequest.PaymentOrder.Amount.Value, response.PaymentOrderResponseObject.Amount.Value);
            Assert.Equal(updateRequest.PaymentOrder.VatAmount.Value, response.PaymentOrderResponseObject.VatAmount.Value);
        }

        [Fact]
        public async Task CreatePaymentOrder_ShouldReturnPaymentOrderWithCorrectAmount()
        {
            var paymentOrderRequest = this.paymentOrderRequestBuilder.WithTestValues().Build();
            var paymentOrder = await this.Sut.PaymentOrder.Create(paymentOrderRequest);
            Assert.NotNull(paymentOrder.PaymentOrderResponse);
            Assert.Equal(paymentOrderRequest.PaymentOrder.Amount.Value, paymentOrder.PaymentOrderResponse.Amount.Value);
        }


        [Fact]
        public async Task CreatePaymentOrder_WithOrderItems_ShouldReturnOrderItemsIfExpanded()
        {
            //ARRANGE
            var paymentOrderRequestContainer =
                this.paymentOrderRequestBuilder.WithTestValues()
                    .WithOrderItems()
                    .Build();

            //ACT
            var paymentOrder = await this.Sut.PaymentOrder.Create(paymentOrderRequestContainer, PaymentOrderExpand.OrderItems);

            //ASSERT
            Assert.NotNull(paymentOrder.PaymentOrderResponse);
            Assert.NotNull(paymentOrder.PaymentOrderResponse.OrderItems);
            Assert.NotEmpty(paymentOrder.PaymentOrderResponse.OrderItems.OrderItemList);
        }


        [Fact]
        public async Task GetPaymentOrder_WithPayment_ShouldReturnCurrentPaymentIfExpanded()
        {
            //ACT
            var paymentOrder = await this.Sut.PaymentOrder.Get(new Uri("/psp/paymentorders/472e6f26-a9b5-4e91-1b70-08d756b9b7d8", UriKind.Relative),
                                                               PaymentOrderExpand.CurrentPayment);

            //ASSERT
            Assert.NotNull(paymentOrder);
            Assert.NotNull(paymentOrder.PaymentOrderResponse.CurrentPayment);
            Assert.NotNull(paymentOrder.PaymentOrderResponse.CurrentPayment.Payment);
        }

        [Fact]
        public async Task GetPaymentOrder_WithSwishPayment_ShouldReturnSales()
        {
            //ACT
            var paymentOrder = await this.Sut.PaymentOrder.Get(new Uri("/psp/paymentorders/472e6f26-a9b5-4e91-1b70-08d756b9b7d8", UriKind.Relative),
                                                               PaymentOrderExpand.CurrentPayment);
            //var sales = await this.Sut.Payment.GetSales(paymentOrder.PaymentOrderResponse.CurrentPayment.Payment.Sales.Id);

            //ASSERT
            //Assert.NotNull(sales);
            //Assert.NotEmpty(sales);
        }


        [Fact]
        public async Task GetUnknownPaymentOrder_ShouldThrowHttpResponseException()
        {
            var id = new Uri("/psp/paymentorders/56a45c8a-9605-437a-fb80-08d742822747", UriKind.Relative);

            await Assert.ThrowsAsync<HttpResponseException>(() => this.Sut.PaymentOrder.Get(id));
        }
    }
}