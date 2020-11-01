
using System;
using System.Text.Json.Serialization;

namespace SecuringWebApiUsingJwtAuthentication.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }

        public string Currency { get; set; }
        public DateTime TS { get; set; }
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }

    }
}
