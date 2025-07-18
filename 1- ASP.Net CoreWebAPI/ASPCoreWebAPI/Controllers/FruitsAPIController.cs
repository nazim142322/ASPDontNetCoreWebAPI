﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsAPIController : ControllerBase
    {
        public List<string> fruits = new List<string>()
        {
            "Mango", "Lichi", "PineApple", "Apple", "WaterMelon", "Banana", "Papaya"
        };

        [HttpGet]
        public List<string> GetFruits()
        {
            return fruits;
        }
        [HttpGet("GellAll")]
        public List<string> GetAllFruits()
        {
            return fruits;
        }

        [HttpGet("{id}")]
        public string GetFruitsByIndex([FromRoute]int id)
         {
            return fruits.ElementAt(id);
        }
    }
}
