using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wms.Proto.Web
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
		IEnumerable<IControl> Controls { get; }
	}

	public interface IControl
	{
		string Name { get; }
	}

	public interface IPageGenerator
	{
		
	}
}
