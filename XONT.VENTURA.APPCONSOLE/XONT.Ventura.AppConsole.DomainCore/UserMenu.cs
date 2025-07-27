using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XONT.Ventura.AppConsole.DomainCore
{
    [Serializable]
  public  class UserMenu
    {
        public string MenuCode { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
 
    }
}
