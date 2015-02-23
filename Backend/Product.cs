﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool InStock { get; set; }
    }
}