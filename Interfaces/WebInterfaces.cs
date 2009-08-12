using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WXML.Model.Descriptors;

namespace Wms.Web
{
	interface IAuthenticationService
	{
	}

	interface IMembershipService
	{
	}

	interface IAuthorizationService
	{
	}

	public interface IPage
	{
		string Name { get; set; }
		string Url { get; set; }
		string Contents { get; set; }
	}

	public interface IControl
	{
		string Name { get; set;  }
		string Contents { get; set; }
	}

	public interface IPageGenerator
	{
		void Generate(IControl control, TextWriter writer);
		void Generate(IPage page, TextWriter writer);
	}

	public interface IControlModel
	{
		object Data { get; set; }
	}

	public interface IAdminPageGenerator
	{
		void GetEditPage(EntityDefinition entityDefinition, TextWriter tw);
		void GetCreatePage(EntityDefinition entityDefinition, TextWriter tw);
	}

}
