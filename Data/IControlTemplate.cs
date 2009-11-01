using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IControlTemplate : IVisual
    {

        IQueryable<IControl> Instances
        {
            get;
            set;
        }

        IController Controller
        {
            get;
            set;
        }

        string Body
        {
            get;
            set;
        }
    }
}
