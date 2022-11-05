using System.Collections.Generic;
using System;
using ShowerShow.Model;

namespace ShowerShow.DTO
{
    public class CreateOrderDTO
    {
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; } = false;
        public DateTime? ShippedDate { get; set; } = null;
        public Dictionary<string, int> orderItems { get; set; }
    }
}
