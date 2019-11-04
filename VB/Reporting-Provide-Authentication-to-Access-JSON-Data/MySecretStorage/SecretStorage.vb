Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml
Imports System.Xml.Serialization

Namespace XtraReport_JsonDataSource_with_Authorization.MySecretStorage
    Public Class SecretStorage
        Private Const storageFileName As String = "data.dat"
        Private Const stringSeparator As String = "@"

        Private Shared lockObj As New Object()

        Private Shared instance_Renamed As SecretStorage

        Private storage_Renamed As Dictionary(Of String, Tuple(Of String, String))
        Private ReadOnly Property Storage() As Dictionary(Of String, Tuple(Of String, String))
            Get
                If storage_Renamed Is Nothing Then
                    storage_Renamed = New Dictionary(Of String, Tuple(Of String, String))()
                End If
                Return storage_Renamed
            End Get
        End Property

        Public Shared ReadOnly Property Instance() As SecretStorage
            Get
                If instance_Renamed Is Nothing Then
                    SyncLock lockObj
                        If instance_Renamed Is Nothing Then
                            instance_Renamed = New SecretStorage()
                        End If
                    End SyncLock
                End If
                Return instance_Renamed
            End Get
        End Property

        Private Sub New()
            Try
                LoadData()
            Catch e As Exception
                System.Diagnostics.Debug.WriteLine("SecretStorage: {0}", e)
            End Try
        End Sub

        Public Function GetCredentials(ByVal uri As String) As Tuple(Of String, String)
            Dim cred As Tuple(Of String, String) = Nothing
            If Storage.TryGetValue(uri.ToLowerInvariant(), cred) Then
                Return cred
            End If
            Return Nothing
        End Function

        Public Sub SaveCredentials(ByVal uri As String, ByVal cred As Tuple(Of String, String))
            If Not Storage.Any(Function(x) x.Key.Equals(uri, StringComparison.InvariantCultureIgnoreCase)) Then
                Storage.Add(uri, cred)
            Else
                Storage(uri.ToLowerInvariant()) = cred
            End If
        End Sub

        Private Sub LoadData()
            Dim lines = File.ReadAllLines(storageFileName)
            For Each line In lines
                Dim strs() As String = line.Split(stringSeparator.ToCharArray()(0))
                If strs.Length <> 3 Then
                    Continue For
                End If
                Dim str1 = Encoding.UTF8.GetString(Convert.FromBase64String(strs(0)))
                Dim str2 = Encoding.UTF8.GetString(Convert.FromBase64String(strs(1)))
                Dim str3 = Encoding.UTF8.GetString(Convert.FromBase64String(strs(2)))
                Storage.Add(str1, New Tuple(Of String, String)(str2, str3))
            Next line
        End Sub
        Private Sub SaveData()
            Dim strings = New List(Of String)()
            For Each item In Storage
                Dim str1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(item.Key))
                Dim str2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(item.Value.Item1))
                Dim str3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(item.Value.Item2))
                strings.Add(str1 & stringSeparator & str2 & stringSeparator & str3)
            Next item
            File.WriteAllLines(storageFileName, strings.ToArray())
        End Sub
        Protected Overrides Sub Finalize()
            Try
                SaveData()
            Catch e As Exception
                System.Diagnostics.Debug.WriteLine("~SecretStorage: {0}", e)
            End Try
        End Sub
    End Class
End Namespace
