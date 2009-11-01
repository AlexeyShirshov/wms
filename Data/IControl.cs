using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IControl
    {

        string ID
        {
            get;
            set;
        }

        IControlTemplate Template
        {
            get;
            set;
        }

        IVisual Visual
        {
            get;
            set;
        }
    }
}
