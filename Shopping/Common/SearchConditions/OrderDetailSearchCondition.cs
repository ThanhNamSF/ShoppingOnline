﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SearchConditions
{
    public class OrderDetailSearchCondition
    {
        public int OrderId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}