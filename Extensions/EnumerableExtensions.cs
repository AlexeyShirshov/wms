using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace System.Linq
{
	public static class EnumerableExtensions
	{
		public static SelectList ToSelectList<T>(this IEnumerable<T> items, Func<T, string> valueSelector,
			Func<T, string> textSelector, Func<T,bool> selectedItemPredicate)
		{
			return new SelectList(items.Select(i => new SelectListItem() { Value = valueSelector(i), Text = textSelector(i), Selected = selectedItemPredicate(i) }));
		}

	}
}
