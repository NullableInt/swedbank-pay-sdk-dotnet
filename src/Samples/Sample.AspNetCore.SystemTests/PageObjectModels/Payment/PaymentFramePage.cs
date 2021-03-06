﻿using Atata;

using Sample.AspNetCore.SystemTests.PageObjectModels.Base;

namespace Sample.AspNetCore.SystemTests.PageObjectModels
{
    using _ = PaymentFramePage;

    [WaitForLoadingIndicator]
    public class PaymentFramePage : Page<_>
    {
        [Wait(1, TriggerEvents.BeforeClick)]
        [FindById("paymentmenu-container")]
        public ControlList<PayexItem, _> PaymentMethods { get; set; }

        [ControlDefinition("div", ContainingClass = "custom-menu-card")]
        public class PayexItem : Control<_>
        {
            [FindByClass("menu-card-title")] public Text<_> Name { get; private set; }

            [WaitFor(Until.Visible, TriggerEvents.BeforeAccess)]
            public Frame<_> PaymentFrame { get; private set; }
        }
    }
}