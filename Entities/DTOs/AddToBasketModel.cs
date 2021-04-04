using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class AddToBasketModel : IDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
