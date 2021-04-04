using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IBasketService
    {
        IResult Add(AddToBasketModel product);
        Basket FindShoppingBasketItem(AddToBasketModel product);
    }
}
