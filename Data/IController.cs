using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IController
    {
        string Name
        {
            get;
            set;
        }

        string ModelName
        {
            get;
            set;
        }

        object LocalParameters
        {
            get;
            set;
        }

        object GlobalParameters
        {
            get;
            set;
        }

        string Code
        {
            get;
            set;
        }

        object Language
        {
            get;
            set;
        }
    }
}
