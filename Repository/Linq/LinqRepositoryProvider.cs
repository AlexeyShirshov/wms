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
        private Type _mtType;

        private string _ctxName;
        private string _mtName;

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

            _ctxName = model.Namespace + "." + model.LinqSettings.ContextName;

            _repositoryType = ass.GetType(_ctxName);
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
            return Activator.CreateInstance(_mtType) as IModificationTracker;
        }

        public IEnumerable<System.CodeDom.CodeCompileUnit> CreateCompileUnits(WXMLModel model)
        {
            CodeCompileUnit modificationTracketUnit = GenerateModificationTracker(model);

            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, new WXML.CodeDom.WXMLCodeDomGeneratorSettings());

            LinqToCodedom.CodeDomGenerator.Language lang = LinqToCodedom.CodeDomGenerator.Language.CSharp;

            return new CodeCompileUnit[] { gen.GetCompileUnit(lang), modificationTracketUnit};
        }

        private CodeCompileUnit GenerateModificationTracker(WXMLModel model)
        {
            var c = new CodeDomGenerator();

            string mtName = "LinqModificationTracker";

            var cls = c.AddNamespace(model.Namespace).AddClass(mtName);

            _ctxName = model.Namespace + "." + model.LinqSettings.ContextName;
            _mtName = model.Namespace + "." + mtName;

            string conn = ConfigurationManager.ConnectionStrings["wms"].ConnectionString;

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
                .AddMethod(MemberAttributes.Public | MemberAttributes.Final, (ParamArray<object> entities) => "Add",
                    Emit.stmt((List<object> _changed, object[] entities) => _changed.AddRange(entities)))
                .Implements(typeof(IModificationTracker))
                .AddMethod(MemberAttributes.Public | MemberAttributes.Final, (ParamArray<object> entities) => "Delete",
                    Emit.stmt((List<object> _deleted, object[] entities) => _deleted.AddRange(entities)))
                .Implements(typeof(IModificationTracker))
                .AddMethod(MemberAttributes.Public | MemberAttributes.Final, () => "AcceptModifications",
                    Emit.@using(new CodeTypeReference(_ctxName), "ctx", () => CodeDom.@new(_ctxName, conn),
                        Emit.@foreach("entity", ()=>CodeDom.@this.Field<IEnumerable<object>>("_changed"),
                            Emit.stmt((object entity)=>CodeDom.@this.Call("SyncEntity")(CodeDom.VarRef("ctx"), entity, false))
                        ),
                        Emit.stmt(()=>CodeDom.@this.Field<List<object>>("_changed").Clear()),
                        Emit.@foreach("entity", () => CodeDom.@this.Field<IEnumerable<object>>("_deleted"),
                            Emit.stmt((object entity) => CodeDom.@this.Call("SyncEntity")(CodeDom.VarRef("ctx"), entity, true))
                        ),
                        Emit.stmt(() => CodeDom.@this.Field<List<object>>("_deleted").Clear()),
                        Emit.stmt(() => CodeDom.VarRef("ctx").Call("SubmitChanges"))
                    )
                ).Implements(typeof(IModificationTracker))
                .AddMethod(MemberAttributes.Private, () => "Dispose").Implements(typeof(IDisposable))
                .AddMethod(MemberAttributes.Private, (DynType ctx, object entity, bool delete) => "SyncEntity" + ctx.SetType(_ctxName),
                    Emit.@foreach("mi", () => CodeDom.@this.Call<Type>("GetType")().GetMethods(BindingFlags.NonPublic | BindingFlags.Static),
                        Emit.@if((bool delete, MethodInfo mi, object entity) =>
                            ((delete && mi.Name == "_DelEntity") || (!delete && mi.Name == "_SyncEntity")) && mi.GetParameters().Count() == 2 && mi.GetParameters().Last().ParameterType == entity.GetType(),
                            //Emit.stmt((MethodInfo mi, object entity) => mi.Invoke(null, BindingFlags.Static, null, new object[] { CodeDom.VarRef("ctx"), entity }, null)),
                            Emit.@return()
                        )
                    )
                )
            ;

            string debug = c.GenerateCode(CodeDomGenerator.Language.CSharp);

            return c.GetCompileUnit(CodeDomGenerator.Language.CSharp);
        }

        public void SetRepositoryAssembly(Assembly assembly)
        {
            _repositoryType = assembly.GetType(_ctxName);
            _mtType = assembly.GetType(_mtName);
        }
    }
}
