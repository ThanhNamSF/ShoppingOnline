﻿using System.Collections;

namespace Common
{
    public class DataSourceResult
    {
        public IEnumerable Data { get; set; }
        public int Total { get; set; }
        public object Errors { get; set; }
    }
}
