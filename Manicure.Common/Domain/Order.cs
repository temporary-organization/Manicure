﻿using System;
using System.Collections.Generic;

namespace Manicure.Common.Domain
{
    public class Order
    {
        public int OrderId { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public DateTime SaleDate { get; set; }

        public ICollection<OrderContent> OrderContents { get; set; }
    }
}