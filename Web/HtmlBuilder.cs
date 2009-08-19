using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace Wms.Web
{
	public class HtmlBuilder
	{
		private readonly HtmlTextWriter _hw;

		public HtmlBuilder()
		{
			_hw = new HtmlTextWriter(new StringWriter());
		}

		public HtmlBuilder(TextWriter tw)
		{
			_hw = new HtmlTextWriter(tw);
		}

		public HtmlBuilder Begin(string tag)
		{
			return Begin(tag, new object());
		}

		public HtmlBuilder Begin(string tag, object attributes)
		{
			return Begin(tag, ObjectToDictionary(attributes));
		}

		public HtmlBuilder Begin(string tag, IDictionary<string, string> attributes)
		{
			foreach (KeyValuePair<string, string> p in attributes)
			{
				_hw.AddAttribute(p.Key, p.Value);
			}
			_hw.RenderBeginTag(tag);
			return this;
		}

		public HtmlBuilder End()
		{
			_hw.RenderEndTag();
			return this;
		}

		public HtmlBuilder Tag(string tag)
		{
			return Tag(tag, new object());
		}

		public HtmlBuilder Tag(string tag, object attributes)
		{
			return Tag(tag, attributes, delegate { });
		}

		public HtmlBuilder Tag(string tag, object attributes, Action<HtmlBuilder> innerAction)
		{
			Begin(tag, attributes);
			innerAction(this);
			End();
			return this;

		}

		public HtmlBuilder Tag(string tag, Action<HtmlBuilder> innerAction)
		{
			return Tag(tag, new object(), innerAction);
		}

		public override string ToString()
		{
			return _hw.ToString();
		}

		public TextWriter GetWriter()
		{
			return _hw;
		}

		public HtmlBuilder Tag(string tag, Func<HtmlBuilder, HtmlBuilder> f)
		{
			Begin(tag);
			f(this);
			End();
			return this;
		}

		private static IDictionary<string, string> ObjectToDictionary(object o)
		{
			var d = new Dictionary<string, string>();
			foreach (PropertyInfo pi in o.GetType().GetProperties())
			{
				d.Add(pi.Name, pi.GetValue(o, new object[] { }).ToString());
			}
			return d;
		}

		public HtmlBuilder Text(string text)
		{
			_hw.WriteLine(text);
			return this;
		}
	}
}
