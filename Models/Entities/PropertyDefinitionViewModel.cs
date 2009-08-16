using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WXML.Model.Descriptors;

namespace Wms.Web.Models
{
    public class PropertyDefinitionViewModel
    {
        public PropertyDefinition PropertyDefinition { get; set; }
        public IEnumerable<string> AllowedTypes { get; set; }
        public EntityDefinition EntityDefinition { get; set;  }
    }
}
