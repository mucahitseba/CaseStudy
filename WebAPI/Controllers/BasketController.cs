using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpPost("add")]
        public IActionResult Add(AddToBasketModel addToBasketModel)
        {
            var result = _basketService.Add(addToBasketModel);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}