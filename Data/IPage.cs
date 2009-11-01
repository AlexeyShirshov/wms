using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public interface IPage : IVisual
    {

        string Url
        {
            get;
            set;
        }

        string Title
        {
            get;
            set;
        }

        string Keywords
        {
            get;
            set;
        }

        IPage Parent
        {
            get;
            set;
        }

        IQueryable<IPage> Childs
        {
            get;
            set;
        }

        string HtmlDescription
        {
            get;
            set;
        }
    }
}
