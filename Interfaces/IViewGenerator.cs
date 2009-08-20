using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WXML.Model.Descriptors;

namespace Wms.Interfaces
{
	public interface IViewGenerator
	{
		void GenerateCreateView(EntityDefinition ed, TextWriter tw);
		void GenerateBrowseView(EntityDefinition ed, TextWriter tw);
		void GenerateEditView(EntityDefinition ed, TextWriter tw);
		CodeCompileUnit GenerateController(EntityDefinition ed) ;
	}
}
