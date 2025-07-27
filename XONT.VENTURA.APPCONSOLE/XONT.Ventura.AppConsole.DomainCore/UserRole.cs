using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XONT.Ventura.AppConsole.DomainCore
{
    [Serializable]
  public  class UserRole
    {
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
     
    }
}
