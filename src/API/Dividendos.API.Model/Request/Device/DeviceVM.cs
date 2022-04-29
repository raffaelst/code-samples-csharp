using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dividendos.API.Model.Request.Device
{
    public class DeviceVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PushToken { get; set; }

        public string AppVersion { get; set; }

        public string UniqueId { get; set; }
    }
}
