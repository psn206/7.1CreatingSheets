using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatingSheets
{
    internal class MethodRevitApp
    {

        UIApplication uiApp;
        UIDocument uiDoc;
        Document doc;

        public UIApplication UiApp { get => uiApp; }
        public UIDocument UiDoc { get => uiDoc; }
        public Document Doc { get => doc; }

        public MethodRevitApp(ExternalCommandData commandData)
        {
            uiApp = commandData.Application;
            uiDoc = UiApp.ActiveUIDocument;
            doc = UiDoc.Document;
        }

        public List<FamilySymbol> GetListTitleBlock()
        {
            ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_TitleBlocks);
            ElementClassFilter elementClassFilter = new ElementClassFilter(typeof(FamilySymbol));
            LogicalAndFilter logicalAndFilter = new LogicalAndFilter(elementCategoryFilter, elementClassFilter);
            List<FamilySymbol> familySymbol = new FilteredElementCollector(Doc)
              .WherePasses(logicalAndFilter)
              .Cast<FamilySymbol>()
              .ToList();
            return familySymbol;
        }

        public List<View> GetListViews()
        {
            var views
                = new FilteredElementCollector(doc)
                    .OfClass(typeof(View))
                    .Cast<View>()
                    .Where(p => p.ViewType == ViewType.FloorPlan)
                    .ToList();
            return views;
        }

        public void СreateSheets(string numberSheets, string developer, FamilySymbol selectedVTitleBlock, View selectedView)
        {

            using (Transaction ts = new Transaction(Doc, "Create New Sheet"))
            {
                if (selectedView == null || selectedVTitleBlock == null) return;
                int number = Convert.ToInt32(numberSheets);
                if (number <= 0) number = 1;

                ts.Start();
                for (int i = 1; i <= number; i++)
                {
                    ElementId newViewID = selectedView.Duplicate(ViewDuplicateOption.AsDependent);
                    ViewSheet sheet = ViewSheet.Create(Doc, selectedVTitleBlock.Id);
                    Viewport viewport = Viewport.Create(Doc, sheet.Id, newViewID, new XYZ(0, 0, 0));
                    sheet.Name = $"Лист{i} {selectedView.Name} ";
                    Parameter disignet = sheet.get_Parameter(BuiltInParameter.SHEET_DESIGNED_BY);
                    disignet.Set(developer);
                }
                doc.Regenerate();
                ts.Commit();

            }
        }
    }
}
