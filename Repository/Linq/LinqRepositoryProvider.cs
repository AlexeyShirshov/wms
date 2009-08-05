using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.Model;
using LinqCodeGenerator;
using System.Reflection;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Compilation;
using System.CodeDom;
using LinqToCodedom;
using LinqToCodedom.Extensions;
using LinqToCodedom.Generator;

namespace Wms.Repository
{
    public class LinqRepositoryProvider : IRepositoryProvider
    {
        private Type _repositoryType;

        //public LinqRepositoryProvider(Type t)
        //{
        //    _repositoryType = t;
        //}

        public LinqRepositoryProvider()
        {
        }

        public void Init(string tempPath, WXMLModel model)
        {
            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            Assembly ass = gen.Compile(/*tempPath + "\\Wms.Data.dll", */LinqToCodedom.CodeDomGenerator.Language.CSharp);

            _repositoryType = ass.GetType("Wms.Data.WmsRepository");
        }

        public object CreateRepository()
        {
            return Activator.CreateInstance(_repositoryType, (object)ConfigurationManager.ConnectionStrings["wms"].ConnectionString);
        }

        public Type RepositoryType
        {
            get
            {
                return _repositoryType;
            }
        }

        public IModificationTracker CreateTracker()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<System.CodeDom.CodeCompileUnit> CreateCompileUnits(WXMLModel model)
        {
            CodeCompileUnit modificationTracketUnit = GenerateModificationTracker(model);

            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            LinqToCodedom.CodeDomGenerator.Language lang = LinqToCodedom.CodeDomGenerator.Language.CSharp;

            return new CodeCompileUnit[] { gen.GetCompileUnit(lang)/*, modificationTracketUnit */};
        }

        private CodeCompileUnit GenerateModificationTracker(WXMLModel model)
        {
            var c = new CodeDomGenerator();

            var cls = c.AddNamespace(model.Namespace).AddClass("LinqModificationTracker");

            string ctxName = model.Namespace + "." + model.LinqSettings.ContextName;

            cls.Implements(typeof(IModificationTracker))
                .AddEnum("ActionEnum")
                    .AddFields(
                        Define.StructField("Update"),
                        Define.StructField("Insert"),
                        Define.StructField("Delete")
            );

            cls.AddField("_changed", () => new List<object>());
            cls.AddField("_deleted", () => new List<object>());
            
            cls
                .AddMethod(MemberAttributes.Public, (ParamArray<object> entities) => "Add",
                    Emit.stmt((List<object> _changed, object[] entities) => _changed.AddRange(entities)))
                .Implements(typeof(IModificationTracker))
                .AddMethod(MemberAttributes.Public, (ParamArray<object> entities) => "Delete",
                    Emit.stmt((List<object> _deleted, object[] entities) => _deleted.AddRange(entities)))
                .Implements(typeof(IModificationTracker))
                .AddMethod(MemberAttributes.Public, () => "AcceptModifications",
                    Emit.@using(new CodeTypeReference(ctxName), "ctx", () => CodeDom.@new(ctxName, ConfigurationManager.ConnectionStrings["wms"].ConnectionString),
                        Emit.@foreach("entity", ()=>CodeDom.@this.Field<IEnumerable<object>>("_changed"),
                            Emit.stmt((object entity)=>CodeDom.@this.Call("SyncEntity")(CodeDom.VarRef("ctx"), entity, false))
                        ),
                        Emit.stmt(()=>CodeDom.@this.Field<List<object>>("_changed").Clear())
                    )
                ).Implements(typeof(IModificationTracker));

            string debug = c.GenerateCode(CodeDomGenerator.Language.CSharp);

            return c.GetCompileUnit(CodeDomGenerator.Language.CSharp);
        }

        public void SetRepositoryAssembly(Assembly assembly)
        {
            _repositoryType = assembly.GetType("Wms.Data.WmsRepository");
        }
    }
}
