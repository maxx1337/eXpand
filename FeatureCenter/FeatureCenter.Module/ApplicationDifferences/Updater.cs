﻿using System;
using System.IO;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;
using Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects;
using Xpand.ExpressApp.ModelDifference.DataStore.Queries;
using Xpand.ExpressApp.ModelDifference.Security;
using Xpand.Xpo.Collections;

namespace FeatureCenter.Module.ApplicationDifferences {

    public class Updater : Xpand.Persistent.BaseImpl.Updater {
        private const string ModelCombine = "ModelCombine";

        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            var session = ((ObjectSpace)ObjectSpace).Session;
            if (new QueryModelDifferenceObject(session).GetActiveModelDifference(ModelCombine, FeatureCenterModule.Application) == null) {
                new ModelDifferenceObject(session).InitializeMembers(ModelCombine, FeatureCenterModule.Application);
                ICustomizableRole role = EnsureRoleExists(ModelCombine, GetPermissions);
                IUserWithRoles user = EnsureUserExists(ModelCombine, ModelCombine, role);
                role.AddPermission(new ModelCombinePermission(ApplicationModelCombineModifier.Allow) { Difference = ModelCombine });
                role.Users.Add(user);
                ObjectSpace.CommitChanges();
            }
            var modelDifferenceObjects = new XpandXPCollection<ModelDifferenceObject>(session, o => o.PersistentApplication.Name == "FeatureCenter");
            foreach (var modelDifferenceObject in modelDifferenceObjects) {
                modelDifferenceObject.PersistentApplication.Name =
                    Path.GetFileNameWithoutExtension(modelDifferenceObject.PersistentApplication.ExecutableName);
            }
            ObjectSpace.CommitChanges();
        }
        protected override System.Collections.Generic.List<System.Security.IPermission> GetPermissions(ICustomizableRole customizableRole) {
            var permissions = base.GetPermissions(customizableRole);
            if (customizableRole.Name == ModelCombine)
                permissions.Add(new EditModelPermission(ModelAccessModifier.Allow));
            return permissions;
        }
    }
}
