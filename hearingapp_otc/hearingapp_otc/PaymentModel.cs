using System;
using System.Collections.Generic;
using System.Text;

namespace hearingapp_otc
{
    class PaymentModel
    {
        public string sourceStripeToken { get; set; }

        // Customer info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string SessionGuid { get; set; }

        // Shipping info
        public string ShipLine1 { get; set; }
        public string ShipLine2 { get; set; }
        public string ShipCity { get; set; }
        public string ShipState { get; set; }
        public string ShipZip { get; set; }

        // Billing info
        public string BillLine1 { get; set; }
        public string BillLine2 { get; set; }
        public string BillCity { get; set; }
        public string BillState { get; set; }
        public string BillZip { get; set; }

        // Item info
        public int chargeAmount { get; set; }
        public string SKU { get; set; }
    }
}
