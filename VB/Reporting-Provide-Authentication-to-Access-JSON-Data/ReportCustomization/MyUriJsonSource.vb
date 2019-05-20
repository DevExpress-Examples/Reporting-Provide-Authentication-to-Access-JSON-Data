Imports DevExpress.DataAccess.Json
Imports System
Imports System.ComponentModel
Imports System.Net
Imports System.Xml.Linq
Namespace XtraReport_JsonDataSource_with_Authorization.ReportCustomization
    Public Class MyUriJsonSource
        Inherits UriJsonSource

        Public Property Username() As String

        <PasswordPropertyText(True)> _
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

            Dim clone_Renamed = New MyUriJsonSource() With { _
                .Uri = Uri, _
                .RootElement = RootElement, _
                .Username = Username, _
                .Password = Password _
            }
            Return clone_Renamed
        End Function
    End Class
End Namespace
