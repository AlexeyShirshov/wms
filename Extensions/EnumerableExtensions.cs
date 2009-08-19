using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms
{
	public static class EnumerableExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T enumerable in source)
			{
				action(enumerable);
			}
		}
	}
}
