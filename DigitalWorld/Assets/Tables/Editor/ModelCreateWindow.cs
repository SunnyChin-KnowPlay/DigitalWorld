using UnityEditor;

namespace DigitalWorld.Table.Editor
{
    internal class ModelCreateWindow : ScriptableWizard
    {
        #region Event
        public delegate void OnCreateModelHandle(NodeModel mode);
        public event OnCreateModelHandle OnCreateModel;
        #endregion

        #region Params
        public string modelName;
        #endregion
        #region Window
        public static ModelCreateWindow DisplayWizard()
        {
            ModelCreateWindow window = ScriptableWizard.DisplayWizard<ModelCreateWindow>("Create Model");
            window?.Show();
            return window;
        }
        #endregion
        #region Common

        #endregion
        #region OnGUI
        private void OnWizardCreate()
        {

            NodeModel model = new NodeModel
            {
                Name = this.modelName
            };

            OnCreateModel?.Invoke(model);

        }
        #endregion
    }
}
