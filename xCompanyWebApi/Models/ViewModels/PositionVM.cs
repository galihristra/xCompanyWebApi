using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace xCompanyWebApi.Models.ViewModels
{
    [KnownTypeAttribute(typeof(PositionVM))]
    public class PositionVM
    {
        public Guid posId { get; set; }
        public string posName { get; set; }
    }
}