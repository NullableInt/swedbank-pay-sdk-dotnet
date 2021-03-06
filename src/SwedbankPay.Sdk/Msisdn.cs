﻿using System;

using Newtonsoft.Json;

using SwedbankPay.Sdk.JsonSerialization;

namespace SwedbankPay.Sdk
{
    public class Msisdn
    {
        public Msisdn(string msisdn)
        {
            if (!IsValidMsisdn(msisdn))
                throw new ArgumentException($"Invalid msisdn: {msisdn}", nameof(msisdn));
            Value = msisdn;
        }


        private string Value { get; }


        public static bool IsValidMsisdn(string msisdn)
        {
            if (string.IsNullOrWhiteSpace(msisdn))
                return false;
            return true;
        }


        public override string ToString()
        {
            return Value;
        }
    }
}