Imports DevExpress.DataAccess.Json
Imports System
Imports System.ComponentModel
Imports System.Net
Imports System.Xml.Linq
Namespace XtraReport_JsonDataSource_with_Authorization.ReportCustomization
	Public Class MyUriJsonSource
		Inherits UriJsonSource

		Public Property Username() As String

		<PasswordPropertyText(True)>
		Public Property Password() As String
		Public Overrides Function GetJsonString() As String
			Using client = New WebClient()
				client.Credentials = New NetworkCredential(Username, Password)
				' add a header to the request
				' client.Headers.Add(UserName, Password);
				Return client.DownloadString(Uri)
			End Using
		End Function
		Protected Overrides Sub SaveToXml(ByVal connection As XElement)
			MyBase.SaveToXml(connection)
			MySecretStorage.SecretStorage.Instance.SaveCredentials(Uri.Authority, New Tuple(Of String, String)(Username, Password))
		End Sub

		Protected Overrides Sub LoadFromXml(ByVal connection As XElement)
			MyBase.LoadFromXml(connection)
			Dim cred = MySecretStorage.SecretStorage.Instance.GetCredentials(Uri.Authority)
			If cred IsNot Nothing Then
				Username = cred.Item1
				Password = cred.Item2
			End If
		End Sub
		Protected Overrides Function Clone() As JsonSourceBase
'INSTANT VB NOTE: The local variable clone was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
			Dim clone_Renamed = New MyUriJsonSource() With {
				.Uri = Uri,
				.RootElement = RootElement,
				.Username = Username,
				.Password = Password
			}
			Return clone_Renamed
		End Function
	End Class
End Namespace
