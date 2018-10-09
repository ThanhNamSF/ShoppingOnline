using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PageList<T>
    {
        public List<T> DataSource { get;}
        public int TotalItems { get;}

        public PageList(List<T> dataSource, int totalItems)
        {
            this.DataSource = dataSource;
            this.TotalItems = totalItems;
        }
    }
}
