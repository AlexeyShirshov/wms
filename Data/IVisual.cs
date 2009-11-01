using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IVisual
    {

        int Name
        {
            get;
            set;
        }

        IQueryable<IControl> Controls
        {
            get;
            set;
        }

        string Description
        {
            get;
            set;
        }

        DateTime CreateDate
        {
            get;
            set;
        }

        string Creator
        {
            get;
            set;
        }

        bool Disabled
        {
            get;
            set;
        }
    }
}
