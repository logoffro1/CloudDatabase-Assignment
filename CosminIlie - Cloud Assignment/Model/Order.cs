using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShowerShow.Model
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; } = false;
        public DateTime? ShippedDate { get; set; } = null;
        public Dictionary<string,int> orderItems { get; set; }
    }
}
