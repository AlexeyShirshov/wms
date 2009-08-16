using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WXML.CodeDom;
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
            WXMLCodeDomGeneratorSettings settings = new WXML.CodeDom.WXMLCodeDomGeneratorSettings();

            CodeCompileUnit modificationTracketUnit = GenerateModificationTracker(model, settings);

            LinqCodeDomGenerator gen = new LinqCodeDomGenerator(model, settings);

            LinqToCodedom.CodeDomGenerator.Language lang = LinqToCodedom.CodeDomGenerator.Language.CSharp;

            return new CodeCompileUnit[] { gen.GetCompileUnit(lang), modificationTracketUnit};
        }

        private CodeCompileUnit GenerateModificationTracker(WXMLModel model, WXMLCodeDomGeneratorSettings setting)
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
            
            var tableType = new CodeTypeReference(typeof(System.Data.Linq.Table<>));
            tableType.TypeArguments.Add(new CodeTypeReference("T"));

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
                        Emit.@foreach("entity", () => CodeDom.@this.Field<IEnumerable<object>>("_deleted"),
                            Emit.stmt((object entity) => CodeDom.@this.Call("SyncEntity")(CodeDom.VarRef("ctx"), entity, true))
                        ),
                        Emit.stmt(() => CodeDom.VarRef("ctx").Call("SubmitChanges")),
                        Emit.@foreach("entity", ()=>CodeDom.@this.Field<IEnumerable<object>>("_changed"),
                            Emit.stmt((object entity)=>CodeDom.@this.Call("AcceptChanges")(entity))
                        ),
                        Emit.stmt(()=>CodeDom.@this.Field<List<object>>("_changed").Clear()),
                        Emit.@foreach("entity", () => CodeDom.@this.Field<IEnumerable<object>>("_deleted"),
                            Emit.stmt((object entity)=>CodeDom.@this.Call("AcceptChanges")(entity))
                        ),
                        Emit.stmt(() => CodeDom.@this.Field<List<object>>("_deleted").Clear())
                    )
                ).Implements(typeof(IModificationTracker))
                .AddMethod(MemberAttributes.Private, () => "Dispose").Implements(typeof(IDisposable))
                .AddMethod(MemberAttributes.Private, (object entity)=>"AcceptChanges",
                    Emit.declare("mi", (object entity)=>entity.GetType().GetMethod("AcceptChanges")),
                    Emit.@if(()=>CodeDom.VarRef("mi") != null,
                        Emit.stmt((MethodInfo mi, object entity)=>mi.Invoke(entity, null))
                    )
                )
                .AddMethod(MemberAttributes.Private, (DynType ctx, object entity, bool delete) => "SyncEntity" + ctx.SetType(_ctxName),
                    Emit.@foreach("mi", () => CodeDom.@this.Call<Type>("GetType")().GetMethods(BindingFlags.NonPublic | BindingFlags.Static),
                        Emit.@if((bool delete, MethodInfo mi, object entity) =>
                            ((delete && mi.Name == "_DelEntity") || (!delete && mi.Name == "_SyncEntity")) && mi.GetParameters().Count() == 2 && mi.GetParameters().Last().ParameterType == entity.GetType(),
                            Emit.stmt((MethodInfo mi, object entity) => mi.Invoke(null, BindingFlags.Static, null, new object[] { CodeDom.VarRef("ctx"), entity }, null)),
                            Emit.exitFor()
                        )
                    )
                )
                .AddMethod(MemberAttributes.Private | MemberAttributes.Static, (DynType p, DynType action, DynType table) => "SyncEntity" + p.SetType("T") + action.SetType("ActionEnum") + table.SetType(tableType),
                    Emit.ifelse(()=>CodeDom.VarRef("action") == CodeDom.Field(new CodeTypeReference("ActionEnum"), "Insert"),
                        CodeDom.CombineStmts(Emit.stmt(()=>CodeDom.VarRef("table").Call("InsertOnSubmit")(CodeDom.VarRef("p")))),
                        Emit.ifelse(()=>CodeDom.VarRef("action") == CodeDom.Field(new CodeTypeReference("ActionEnum"), "Delete"),
                            CodeDom.CombineStmts(
                                Emit.stmt(()=>CodeDom.VarRef("table").Call("Attach")(CodeDom.VarRef("p"))),
                                Emit.stmt(()=>CodeDom.VarRef("table").Call("DeleteOnSubmit")(CodeDom.VarRef("p")))
                            ),
                            Emit.stmt(()=>CodeDom.VarRef("table").Call("Attach")(CodeDom.VarRef("p"), true))
                        )
                    )
                ).Generic("T", typeof(object))
            ;

            WXMLCodeDomGeneratorNameHelper n = new WXMLCodeDomGeneratorNameHelper(setting);

            foreach (var entity in model.GetActiveEntities())
            {
                if (entity.HasSinglePk)
                {
                    //string entityName = entity.Name;
                    string entityProp = WXMLCodeDomGeneratorNameHelper.GetMultipleForm(entity.Name);
                    string entityType = n.GetEntityClassName(entity, true);
                    string pkName = entity.GetPkProperties().Single().Name;

                    cls.AddMethod(MemberAttributes.Static | MemberAttributes.Private,
                        (DynType ctx, DynType p) => "_DelEntity" + ctx.SetType(_ctxName) + p.SetType(entityType),
                        Emit.stmt(() => CodeDom.Call(null, "SyncEntity", new CodeTypeReference(entityType))(
                            CodeDom.VarRef("p"),
                            CodeDom.Field(new CodeTypeReference("ActionEnum"), "Delete"),
                            CodeDom.VarRef("ctx").Property(entityProp))
                        )
                    )
                    .AddMethod(MemberAttributes.Static | MemberAttributes.Private,
                        (DynType ctx, DynType p) => "_SynEntity" + ctx.SetType(_ctxName) + p.SetType(entityType),
                        Emit.stmt(() => CodeDom.Call(null, "SyncEntity", new CodeTypeReference(entityType))(
                            CodeDom.VarRef("p"),
                            CodeDom.VarRef("p").Field<int>(pkName) == 0 ? CodeDom.Field(new CodeTypeReference("ActionEnum"), "Insert") : CodeDom.Field(new CodeTypeReference("ActionEnum"), "Update"),
                            CodeDom.VarRef("ctx").Property(entityProp))
                        )
                    )
                    ;
                }
            }

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
