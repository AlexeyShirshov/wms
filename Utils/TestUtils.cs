using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MbUnit.Framework;

namespace Wms.Tests
{
	public class TestUtils
	{
		public static string TestDataDir { get { return @"..\..\Test_Data"; }}
	}
	
	public static class FileAssert
	{
		public static void Exists(string path)
		{
			Assert.IsTrue(File.Exists(path));
		}
	}
}
