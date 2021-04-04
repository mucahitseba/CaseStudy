using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class BasketManager : IBasketService
    {
        private IBasketDal _basketDal;
        private IProductService _productService;
        public BasketManager(IBasketDal basketDal, IProductService productService)
        {
            _basketDal = basketDal;
            _productService = productService;
        }

        public IResult Add(AddToBasketModel addToBasketModel)
        {
            var product = _productService.GetById(addToBasketModel.ProductId);
            if(product == null)
                return new ErrorResult(Messages.ProductNotFound);

            var shoppingBasketItem = FindShoppingBasketItem(addToBasketModel);

            int totalStockQuantity = shoppingBasketItem == null ? addToBasketModel.Quantity : addToBasketModel.Quantity + shoppingBasketItem.Quantity;

            IResult result = BusinessRules.Run(CheckIfProductPublished(product),
                CheckIfProductPriceAvailable(product),
                CheckIfProductStockQuantity(product, totalStockQuantity));

            if (result != null)
                return result;

            if (shoppingBasketItem == null)
            {
                Basket basket = new Basket
                {
                    ProductId = addToBasketModel.ProductId,
                    CustomerId = 1,
                    Quantity = totalStockQuantity
                };

                _basketDal.Add(basket);
            }
            else
            {
                shoppingBasketItem.Quantity = totalStockQuantity;
                _basketDal.Update(shoppingBasketItem);
            }
            return new SuccessResult(Messages.ProductAdded);
        }
        public Basket FindShoppingBasketItem(AddToBasketModel product)
        {
            return _basketDal.Get(p => p.ProductId == product.ProductId && p.CustomerId == 1);
        }
        private IResult CheckIfProductPublished(Product product)
        {
            if (!product.Published)
                return new ErrorResult(Messages.ProductNotFound);
            return new SuccessResult();
        }
        private IResult CheckIfProductPriceAvailable(Product product)
        {
            if (product.Price <= 0)
                return new ErrorResult(Messages.ProductPriceInNotAvailable);
            return new SuccessResult();
        }
        private IResult CheckIfProductStockQuantity(Product product, int stock)
        {
            if (product.StockQuantity < stock)
                return new ErrorResult(Messages.OutOfStock);
            return new SuccessResult();
        }
    }
}
