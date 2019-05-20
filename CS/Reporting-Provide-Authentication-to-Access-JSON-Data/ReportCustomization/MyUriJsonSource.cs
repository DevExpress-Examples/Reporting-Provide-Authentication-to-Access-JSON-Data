using DevExpress.DataAccess.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
namespace XtraReport_JsonDataSource_with_Authorization.ReportCustomization
{
    public class MyUriJsonSource : UriJsonSource {
        public string Username { get; set; }

        [PasswordPropertyText(true)]
        public string Password { get; set; }
        public override string GetJsonString() {
            using(var client = new WebClient()) {
                client.Credentials = new NetworkCredential(Username, Password);
                // add a header to the request
                // client.Headers.Add(UserName, Password);
                return client.DownloadString(Uri);
            }
        }
        protected override void SaveToXml(XElement connection) {
            base.SaveToXml(connection);
            MySecretStorage.SecretStorage.Instance.SaveCredentials(Uri.Authority, new Tuple<string, string>(Username, Password));
        }

        protected override void LoadFromXml(XElement connection) {
            base.LoadFromXml(connection);
            var cred = MySecretStorage.SecretStorage.Instance.GetCredentials(Uri.Authority);
            if(cred != null) {
                Username = cred.Item1;
                Password = cred.Item2;
            }
        }
        protected override JsonSourceBase Clone() {
            var clone = new MyUriJsonSource() {
                Uri = Uri,
                RootElement = RootElement,
                Username = Username,
                Password = Password
            };
            return clone;
        }
    }
}
