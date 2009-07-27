using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		void Generate(IPage page);
		void Generate(IControl control);
	}

	public interface IControlModel
	{
		object Data { get; set; }
	}
}
