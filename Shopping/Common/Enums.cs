using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum NotificationType
    {
        Success,
        Error,
        Empty,
        Warning
    }

    public enum UserRole
    {
        Admin = 1,
        Salesman = 2,
        Shipper = 3
    }

    public enum OrderStatus
    {
        Cancelled,
        Approved,
        Assigned,
        Closed,
        Open
    }

    public enum Trend
    {
        All,
        Hot,
        New
    }
}
