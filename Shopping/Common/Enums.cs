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
        Warning
    }

    public enum UserRole
    {
        Customer,
        Admin
    }

    public enum OrderStatus
    {
        Open,
        Closed
    }

    public enum Trend
    {
        All,
        Hot,
        New
    }
}
