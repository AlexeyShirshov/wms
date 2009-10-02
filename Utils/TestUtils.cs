using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Microsoft.Practices.Unity;
using Wms.Data;
using Wms.Tests.Fakes;

namespace Wms.Tests
{
	public static class TestUtils
	{
		public static string TestDataDir { get { return @"..\..\Test_Data"; }}

        public static void PrintDictionary(this IDictionary d)
        {
            if(d == null)
                return;

            foreach (var key in d.Keys)
            {
                Console.WriteLine("[{0},{1}", key, d[key]);
            }
        }

		public static IUnityContainer FakeContainer
		{
			get
			{
				var container = new UnityContainer();
				container.RegisterType<IDefinitionManager, FakeDefinitionManager>()
					.RegisterInstance(typeof(IRepositoryManager), new FakeRepositoryManager());
				return container;
			}
		}
	}
	
	public static class FileAssert
	{
		public static void Exists(string path)
		{
			Assert.IsTrue(File.Exists(path));
		}
	}

	public static class StringAssert
	{
		public static void EqualToWhiteSpace(string expected, string actual)
		{
			Assert.AreEqual(expected.Replace(" ", "").Replace("\t", ""), actual.Replace(" ", "").Replace("\t", ""));
		}
	}

	




}
