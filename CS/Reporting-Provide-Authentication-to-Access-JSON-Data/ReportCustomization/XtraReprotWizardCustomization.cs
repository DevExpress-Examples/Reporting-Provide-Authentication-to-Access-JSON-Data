using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Native.Json;
using DevExpress.DataAccess.UI.Json;
using DevExpress.DataAccess.UI.Native.Json;
using DevExpress.DataAccess.UI.Wizard;
using DevExpress.DataAccess.Wizard;
using DevExpress.DataAccess.Wizard.Model;
using DevExpress.DataAccess.Wizard.Presenters;
using DevExpress.DataAccess.Wizard.Views;
using DevExpress.XtraLayout;
using DevExpress.XtraReports.Wizards;
using System;
using System.ComponentModel.Design;

namespace XtraReport_JsonDataSource_with_Authorization.ReportCustomization
{
    public interface IMyChooseJsonSourcePageView : IChooseJsonSourcePageView {
        string UserName { get; set; }
        string Password { get; set; }
    }
    public class MyChooseJsonSourcePageView : DevExpress.DataAccess.UI.Wizard.Views.ChooseJsonSourcePageView, IMyChooseJsonSourcePageView {
        public string UserName {
            get {
                return userNameCtrl.Text;
            }
            set {
                userNameCtrl.Text = value;
            }
        }
        public string Password {
            get {
                return PasswordCtrl.Text;
            }
            set {
                PasswordCtrl.Text = value;
            }
        }

        protected readonly LayoutControlItem userNameLayoutItem;
        protected readonly LayoutControlItem passwordLayoutItem;
        protected readonly DevExpress.XtraEditors.TextEdit userNameCtrl;
        protected readonly DevExpress.XtraEditors.TextEdit PasswordCtrl;
        public MyChooseJsonSourcePageView() : base() {
            userNameLayoutItem = layoutControlContent.AddItem();
            userNameLayoutItem.Text = "User Name";
            userNameCtrl = new DevExpress.XtraEditors.TextEdit();
            userNameLayoutItem.Control = userNameCtrl;

            passwordLayoutItem = layoutControlContent.AddItem();
            passwordLayoutItem.Text = "Password";
            PasswordCtrl = new DevExpress.XtraEditors.TextEdit();
            PasswordCtrl.Properties.PasswordChar = '*';
            PasswordCtrl.Properties.UseSystemPasswordChar = true;
            passwordLayoutItem.Control = PasswordCtrl;
        }

        protected override void TypeComboOnSelectedValueChanged(object sender, EventArgs e) {
            base.TypeComboOnSelectedValueChanged(sender, e);
            if(userNameLayoutItem == null || passwordLayoutItem == null) {
                return;
            }
            if(ConnectionType != JsonConnectionType.Uri) {
                userNameLayoutItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                passwordLayoutItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            } else {
                userNameLayoutItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                passwordLayoutItem.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
        }
    }
    public class MyChooseJsonSourcePage<TModel> : ChooseJsonSourcePage<TModel> where TModel : class, IJsonDataSourceModel {
        IWizardRunnerContext Context;
        public MyChooseJsonSourcePage(IChooseJsonSourcePageView view, IWizardRunnerContext context) : base(view, context) {
            this.Context = context;
        }
        public override void Begin() {
            base.Begin();
            if(Model.JsonSource != null) {
                var myJsonSource = Model.JsonSource as MyUriJsonSource;
                if(myJsonSource != null && View is IMyChooseJsonSourcePageView) {
                    (View as IMyChooseJsonSourcePageView).Password = myJsonSource.Password;
                    (View as IMyChooseJsonSourcePageView).UserName = myJsonSource.Username;
                }
            }
        }
        public override bool Validate(out string errorMessage) {
            if (View.ConnectionType != JsonConnectionType.Uri) {
                return base.Validate(out errorMessage);
            } else {
                errorMessage = null;
                JsonDataSource source = null;
                try {
                    return AsyncHelper.DoWithWaitForm(token => {
                        source = new JsonDataSource {
                            JsonSource = new MyUriJsonSource() {
                                Uri = new Uri(View.Uri),
                                Username = (View as IMyChooseJsonSourcePageView)?.UserName,
                                Password = (View as IMyChooseJsonSourcePageView)?.Password
                            }
                        };
                        token.ThrowIfCancellationRequested();
                        source.Fill();
                        Model.DataSchema = DevExpress.DataAccess.Native.Data.DataView.ConvertToDataView(source);
                        Model.JsonSchema = source.Schema;
                    }, Context.WaitFormActivator, Context.CreateExceptionHandler(ExceptionHandlerKind.Loading));
                } catch(Exception e) {
                    errorMessage = e.Message;
                    return false;
                } finally {
                    source?.Dispose();
                }
            }
        }
        public override void Commit() {
            Model.JsonSource = new MyUriJsonSource() {
                Uri = new Uri(View.Uri),
                Username = (View as IMyChooseJsonSourcePageView)?.UserName,
                Password = (View as IMyChooseJsonSourcePageView)?.Password
            };
            View.Changed -= ViewOnChanged;
        }
    }
    public class MyWizardCustomizationService : IWizardCustomizationService, IJsonEditorsCustomizationService {
        public void CustomizeDataSourceWizard(IWizardCustomization<XtraReportModel> tool) {
            tool.RegisterPage<ChooseJsonSourcePage<XtraReportModel>, MyChooseJsonSourcePage<XtraReportModel>>();
            tool.RegisterPageView<IChooseJsonSourcePageView, MyChooseJsonSourcePageView>();
        }

        public void CustomizeReportWizard(IWizardCustomization<XtraReportModel> tool) {
            tool.RegisterPage<ChooseJsonSourcePage<XtraReportModel>, MyChooseJsonSourcePage<XtraReportModel>>();
            tool.RegisterPageView<IChooseJsonSourcePageView, MyChooseJsonSourcePageView>();
        }

        public void CustomizeWizard(JsonEditorId editor, IWizardCustomization<JsonDataSourceModel> tool) {
            tool.RegisterPage<ChooseJsonSourcePage<JsonDataSourceModel>, MyChooseJsonSourcePage<JsonDataSourceModel>>();
            tool.RegisterPageView<IChooseJsonSourcePageView, MyChooseJsonSourcePageView>();
        }

        public bool TryCreateDataSource(IDataSourceModel model, out object dataSource, out string dataMember) {
            dataSource = null;
            dataMember = model?.DataMember;
            return false;
        }

        public bool TryCreateReport(IDesignerHost designerHost, XtraReportModel model, object dataSource, string dataMember) {
            return false;
        }
    }
}
