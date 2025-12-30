using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envios.Application.DTOs.Carnet
{
    public class CarnetWebhookDto
    {
        public string Event { get; set; }                // payment.success
        public string Subscription_Id { get; set; }      // sub_987
        public decimal Amount { get; set; }
        public DateTime Paid_At { get; set; }

        public string SubscriptionId => Subscription_Id;

    }
}
