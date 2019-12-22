﻿using System;

using SwedbankPay.Sdk.Payments;

namespace SwedbankPay.Sdk.PaymentOrders
{
    public class CurrentPaymentResponseContainer : PaymentResponseContainer
    {
        public CurrentPaymentResponseContainer(Uri id, string menuElementName,
                                               OperationList operations,
                                               PaymentResponse payment) : base(operations, payment)
        {
            Id = id;
            MenuElementName = menuElementName;
        }

        /// <summary>
        ///     The relative URI to the payment.
        /// </summary>
        public Uri Id { get; }

        /// <summary>
        ///     creditcard, invoice, etc. The name of the selected menu element.
        /// </summary>
        public string MenuElementName { get; }
    }
}