using System.IO;

namespace Xpand.ExpressApp.WorldCreator.Win {
    public sealed partial class WorldCreatorWinModule : WorldCreatorModuleBase {
        public WorldCreatorWinModule() {
            InitializeComponent();
        }

        public override string GetPath() {
            return Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        }
    }
}