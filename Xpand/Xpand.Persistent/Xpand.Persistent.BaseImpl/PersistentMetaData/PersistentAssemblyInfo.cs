﻿using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Xpand.ExpressApp.Enums;
using Xpand.ExpressApp.WorldCreator.Core;
using Xpand.Persistent.BaseImpl.PersistentMetaData;
using Xpand.Persistent.BaseImpl.PersistentMetaData.PersistentAttributeInfos;
using Xpand.ExpressApp.Attributes;
using Xpand.Persistent.Base.PersistentMetaData;
using Xpand.Persistent.Base.PersistentMetaData.PersistentAttributeInfos;
using Xpand.Xpo.DB;

[assembly: DataStore(typeof(PersistentAssemblyInfo), "WorldCreator")]
namespace Xpand.Persistent.BaseImpl.PersistentMetaData {
    [DefaultClassOptions]
    [DevExpress.Persistent.Base.NavigationItem("WorldCreator")]
    [InterfaceRegistrator(typeof(IPersistentAssemblyInfo))]
    public class PersistentAssemblyInfo : BaseObject, IPersistentAssemblyInfo {
        CodeDomProvider _codeDomProvider;
        string _compileErrors;
        int _compileOrder;

        bool _doNotCompile;
        string _name;

        StrongKeyFile _strongKeyFile;

        const string Version="1.0.0.*";

        public PersistentAssemblyInfo(Session session) : base(session) {
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Attributes.Add(new PersistentAssemblyVersionAttributeInfo(Session){Version = Version});
        }

        [Index(4)]
        [FileTypeFilter("Strong Keys", 1, "*.snk")]
        [Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public StrongKeyFile StrongKeyFile {
            get { return _strongKeyFile; }
            set { SetPropertyValue("StrongKeyFile", ref _strongKeyFile, value); }
        }

        [Index(6)]
        [Custom("AllowEdit", "false")]
        [Size(SizeAttribute.Unlimited)]
        public string GeneratedCode {
            get {
                return CodeEngine.GenerateCode(this);
            }
        }

        [Association("PersistentAssemblyInfo-PersistentClassInfos")]
        [Aggregated]
        public XPCollection<PersistentClassInfo> PersistentClassInfos {
            get { return GetCollection<PersistentClassInfo>("PersistentClassInfos"); }
        }

        #region IPersistentAssemblyInfo Members
        [RuleRequiredField(null, DefaultContexts.Save)]
        [RuleUniqueValue(null, DefaultContexts.Save)]
        [Index(0)]
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }

        [Index(1)]
        public int CompileOrder {
            get { return _compileOrder; }
            set { SetPropertyValue("CompileOrder", ref _compileOrder, value); }
        }

        [Association("PersistentAssemblyInfo-Attributes")]
        public XPCollection<PersistentAssemblyAttributeInfo> Attributes {
            get { return GetCollection<PersistentAssemblyAttributeInfo>("Attributes"); }
        }
        IList<IPersistentAssemblyAttributeInfo> IPersistentAssemblyInfo.Attributes {
            get { return new ListConverter<IPersistentAssemblyAttributeInfo, PersistentAssemblyAttributeInfo>(Attributes); }
        }

        [Index(2)]
        [AllowEdit(true, AllowEditEnum.NewObject)]
        public CodeDomProvider CodeDomProvider {
            get { return _codeDomProvider; }
            set { SetPropertyValue("CodeDomProvider", ref _codeDomProvider, value); }
        }


        [Index(5)]
        public bool DoNotCompile {
            get { return _doNotCompile; }
            set { SetPropertyValue("DoNotCompile", ref _doNotCompile, value); }
        }

        IFileData IPersistentAssemblyInfo.FileData {
            get { return StrongKeyFile; }
            set { StrongKeyFile = value as StrongKeyFile; }
        }

        [Index(7)]
        [Custom("AllowEdit", "false")]
        [Size(SizeAttribute.Unlimited)]
        public string CompileErrors {
            get { return _compileErrors; }
            set { SetPropertyValue("CompileErrors", ref _compileErrors, value); }
        }

        IList<IPersistentClassInfo> IPersistentAssemblyInfo.PersistentClassInfos {
            get { return new ListConverter<IPersistentClassInfo, PersistentClassInfo>(PersistentClassInfos); }
        }
        #endregion
    }

}