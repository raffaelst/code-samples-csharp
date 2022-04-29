using Dividendos.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dividendos.Entity.Views
{
    public class PushNotificationView
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Device Device { get; set; }
    }
}
