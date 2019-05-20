using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.UI.Native.Json;
using DevExpress.DataAccess.UI.Json;
using System;
using System.Windows.Forms;
using XtraReport_JsonDataSource_with_Authorization.ReportCustomization;
using DevExpress.XtraReports.Wizards;

namespace XtraReport_JsonDataSource_with_Authorization
{
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private void DesignTimeAuthenticationButton_Click(object sender, EventArgs e) {
            var reportDesignTool = new DevExpress.XtraReports.UI.ReportDesignTool(new DevExpress.XtraReports.UI.XtraReport());
            reportDesignTool.DesignForm.DesignMdiController.AddService(typeof(IWizardCustomizationService), new MyWizardCustomizationService());
            reportDesignTool.DesignForm.DesignMdiController.AddService(typeof(IJsonEditorsCustomizationService), new MyWizardCustomizationService());
            reportDesignTool.ShowDesigner();
        }
        private void RuntimeAuthenticationButton_Click(object sender, EventArgs e) {
            var report = new MyReportHelper().CreateReport();
            report.DataSource = JsonDataSourceAuthorization_Example.CreateCustomJsonDataSource(
                @"http://northwind.servicestack.net/customers.json", "userName1", "userPassword1");
            report.DataMember = "Customers";
            new DevExpress.XtraReports.UI.ReportDesignTool(report).ShowDesigner();
        }
    }
    public static class JsonDataSourceAuthorization_Example {

        public static JsonDataSource CreateCustomJsonDataSource(string uri, string userName, string password) {
            var jsonDatasource = new JsonDataSource() {
                Name = "jsonDataSource",
                JsonSource = new MyUriJsonSource() {
                    Uri = new Uri(uri),
                    Username = userName,
                    Password = password
                }
            };
            return jsonDatasource;
        }
    }
}
