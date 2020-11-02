﻿using SwedbankPay.Sdk.PaymentInstruments;

using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

using Xunit;

namespace SwedbankPay.Sdk.Tests.Json
{
    public class UknownEnumTests
    {
        [Fact]
        public void DeserializingAUnknownEnum_DoesNotThrowAnError()
        {
            var paymentInstrument = JsonSerializer.Deserialize<PaymentInstrument>(InvalidStringEnum, JsonSerialization.JsonSerialization.Settings);
            Assert.Equal(PaymentInstrument.Unknown, paymentInstrument);
        }

        [Theory]
        [InlineData(typeof(PaymentInstrument), "Chair")]
        [InlineData(typeof(PaymentInstrument), "Bottle")]
        [InlineData(typeof(PriceType), "Bottle")]
        [InlineData(typeof(TransactionType), "Notebook")]
        [InlineData(typeof(PaymentIntent), "Eraser")]
        public void DeserializingUnknownEnum_GivesDefaultValue(Type enumType, string value)
        {
            var expect = Activator.CreateInstance(enumType);
            var result = JsonSerializer.Deserialize($"{{ \"propertyName\":\"{value}\" }}", enumType, JsonSerialization.JsonSerialization.Settings);
            Assert.Equal(expect, result);
        }

        [Fact]
        public void DeserializingUnknownEnum_DoesNotThrowForAnyType()
        {
            var enums = Assembly.GetAssembly(typeof(PaymentInstrument)).GetTypes().Where(t => t.IsEnum && !t.IsDefined(typeof(FlagsAttribute)));
            foreach (var enumType in enums)
            {
                var result = JsonSerializer.Deserialize(InvalidStringEnum, enumType, JsonSerialization.JsonSerialization.Settings);
                Assert.True(result.ToString().Equals("Unknown", StringComparison.InvariantCultureIgnoreCase), $"The default value for {enumType.Name} is not the string \"Unknown\"");
            }
        }

        public static string InvalidStringEnum = $"{{ \"propertyName\":\"ThisIsAVeryInvalidEnumValueForThisObject\"}}";
    }
}
