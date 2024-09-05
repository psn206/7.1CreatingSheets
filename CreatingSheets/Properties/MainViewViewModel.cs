using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingSheets.Properties
{
    internal class MainViewViewModel
    {

        MethodRevitApp methodRevitApp;
        public DelegateCommand SaveCommand { get; }
        public List<FamilySymbol> ListTitleBlock { get; set; } = new List<FamilySymbol>();
        public FamilySymbol SelectedTitleBlock { get; set; }
        public List<View> ListViews { get; set; } = new List<View>();
        public View SelectedView { get; set; }
        public string NumberSheets { get; set; }
        public string Developer { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            SaveCommand = new DelegateCommand(OnSaveCommand);
            methodRevitApp = new MethodRevitApp(commandData);
            ListTitleBlock = methodRevitApp.GetListTitleBlock();
            ListViews = methodRevitApp.GetListViews();
        }

        private void OnSaveCommand()
        {
            methodRevitApp.СreateSheets(NumberSheets, Developer, SelectedTitleBlock, SelectedView);

            RaiseCloseRecuest();
        }

        public event EventHandler CloseRecuest;

        public void RaiseCloseRecuest()
        {
            CloseRecuest?.Invoke(this, EventArgs.Empty);
        }
    }
}
