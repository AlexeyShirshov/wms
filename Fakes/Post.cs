using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Tests.Fakes
{
	public class Post
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
	}

	public class PostToTag
	{
		public int PostId { get; set; }
		public int TagId { get; set; }
	}
}
