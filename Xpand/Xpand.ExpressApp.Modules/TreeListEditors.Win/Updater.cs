using System;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;

namespace Xpand.ExpressApp.TreeListEditors.Win
{
    public class Updater : ModuleUpdater
    {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion)
        {
        }
    }
}