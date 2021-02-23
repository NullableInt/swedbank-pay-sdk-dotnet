﻿using SwedbankPay.Sdk.PaymentInstruments.Card;
using System.Text.Json;
using Xunit;

namespace SwedbankPay.Sdk.Tests.UnitTests
{
    public class CardPaymentTests
    {
        private const string cardPaymentVerificationList = @"{
  ""payment"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c"",
  ""verifications"": {
    ""id"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/verifications"",
    ""verificationList"": [
      {
        ""id"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/verifications/12345678-1234-1234-1234-123456789012"",
        ""cardBrand"": ""Visa"",
        ""cardType"": ""Credit"",
        ""paymentToken"": ""451f9dc8-2ab0-4901-bcc0-d2115f1c0a69"",
        ""recurrenceToken"" : ""12345678-1234-1234-1234-123456789013"",
        ""maskedPan"": ""492500******0004"",
        ""expiryDate"": ""02/2018"",
        ""panToken"": ""eb488c77-8118-4c9f-b3b3-ff134936df64"",
        ""transaction"": {
          ""id"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/transactions/12345678-1234-1234-1234-123456789012"",
          ""created"": ""2016-09-14T01:01:01.01Z"",
          ""updated"": ""2016-09-14T01:01:01.03Z"",
          ""type"": ""Verification"",
          ""state"": ""Completed"",
          ""number"": 1234567890,
          ""amount"": 1000,
          ""vatAmount"": 250,
          ""description"": ""Test transaction"",
          ""payeeReference"": ""AH123456"",
          ""failedReason"": ""ExternalResponseError"",
          ""failedActivityName"": ""Authorize"",
          ""failedErrorCode"": ""REJECTED_BY_ACQUIRER"",
          ""failedErrorDescription"": ""General decline, response-code: 05"",
          ""isOperational"": false,
          ""activities"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/transactions/12345678-1234-1234-1234-123456789012/activities"",
          ""operations"": [
          ]
        }
      },
      {
        ""id"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/verifications/12345678-1234-1234-1234-123456789012"",
        ""cardBrand"": ""Visa"",
        ""cardType"": ""Credit"",
        ""paymentToken"": ""451f9dc8-2ab0-4901-bcc0-d2115f1c0a69"",
        ""recurrenceToken"" : ""12345678-1234-1234-1234-123456789013"",
        ""maskedPan"": ""492500******0004"",
        ""expiryDate"": ""02/2018"",
        ""panToken"": ""eb488c77-8118-4c9f-b3b3-ff134936df64"",
        ""transaction"": {
          ""id"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/transactions/12345678-1234-1234-1234-123456789012"",
          ""created"": ""2016-09-14T01:01:01.01Z"",
          ""updated"": ""2016-09-14T01:01:01.03Z"",
          ""type"": ""Verification"",
          ""state"": ""Initialized"",
          ""number"": 1234567890,
          ""amount"": 1000,
          ""vatAmount"": 250,
          ""description"": ""Test transaction"",
          ""payeeReference"": ""AH123456"",
          ""failedReason"": ""ExternalResponseError"",
          ""failedActivityName"": ""Authorize"",
          ""failedErrorCode"": ""REJECTED_BY_ACQUIRER"",
          ""failedErrorDescription"": ""General decline, response-code: 05"",
          ""isOperational"": true,
          ""activities"": ""/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c/transactions/12345678-1234-1234-1234-123456789012/activities"",
          ""operations"": [
            {
              ""href"": ""https://api.payex.com/psp/creditcard/payments/5adc265f-f87f-4313-577e-08d3dca1a26c"",
              ""rel"": ""edit-verification"",
              ""method"": ""PATCH""
            }
          ]
        }
      }
    ]
  }
}";

        [Fact]
        public void Creating_CardPaymentVerifyResponse_FromDto()
        {
            var api_response = cardPaymentVerificationList;
            var dto = JsonSerializer.Deserialize<CardPaymentVerifyResponseDto>(api_response, JsonSerialization.JsonSerialization.Settings);
            _ = new CardPaymentVerifyResponse(dto);
        }
    }
}
