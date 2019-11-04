Imports DevExpress.XtraReports.UI
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace XtraReport_JsonDataSource_with_Authorization
    Public Class MyReportHelper
        Public Function CreateReport() As XtraReport
            Dim report As New XtraReport()
            report.Bands.AddRange(GetReportBands(report.PageWidth - report.Margins.Left - report.Margins.Right))
            CreateStyles(CType(report.Bands(BandKind.Detail), DetailBand))
            Return report
        End Function

        Private Function GetReportBands(ByVal pageWidth As Single) As Band()
            Dim reportBands As New List(Of Band)()
            Dim indent As Single = 0
            reportBands.Add(GetGroupHeader("Country", pageWidth, indent, 1))
            indent += 25
            reportBands.Add(GetGroupHeader("City", pageWidth, indent, 0))
            reportBands.Add(GetDetailBand(pageWidth, indent))

            Return reportBands.ToArray()
        End Function

        Private Function GetDetailBand(ByVal pageWidth As Single, ByVal indent As Single) As Band
            Dim detailBand As New DetailBand()
            detailBand.HeightF = 25
            detailBand.Controls.Add(GetDetailsTable(pageWidth, indent))
            Return detailBand
        End Function

        Private Function GetDetailsTable(ByVal pageWidth As Single, ByVal indent As Single) As XRControl
            Dim detailsTable As XRTable = XRTable.CreateTable(New RectangleF(indent, 0, pageWidth - indent, 25), 1, 4)
            CreateBindings(detailsTable)
            Return detailsTable
        End Function

        Private Sub CreateStyles(ByVal detailBand As DetailBand)
            Dim evenStyle = New XRControlStyle(Color.LightGray, Color.White, DevExpress.XtraPrinting.BorderSide.None, 0, Nothing, Color.Black, DevExpress.XtraPrinting.TextAlignment.MiddleLeft) With {.Name = "evenStyle"}
            Dim oddStyle = New XRControlStyle(Color.White, Color.White, DevExpress.XtraPrinting.BorderSide.None, 0, Nothing, Color.Black, DevExpress.XtraPrinting.TextAlignment.MiddleLeft) With {.Name = "oddStyle"}
            detailBand.RootReport.StyleSheet.AddRange(New XRControlStyle() { evenStyle, oddStyle })
            detailBand.OddStyleName = oddStyle.Name
            detailBand.EvenStyleName = evenStyle.Name
        End Sub

        Private Sub CreateBindings(ByVal detailsTable As XRTable)
            detailsTable.Rows(0).Cells(0).ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[CompanyName]"))
            detailsTable.Rows(0).Cells(1).ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[Address]"))
            detailsTable.Rows(0).Cells(2).ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[Phone]"))
            detailsTable.Rows(0).Cells(3).ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "FormatString('{0}, {1}', [ContactName], [ContactTitle])"))
        End Sub

        Private Function GetGroupHeader(ByVal groupFieldName As String, ByVal pageWidth As Single, ByVal indent As Single, ByVal level As Integer) As GroupHeaderBand
            Dim groupBand As New GroupHeaderBand() With { _
                .HeightF = 35, _
                .Level = level _
            }
            groupBand.GroupFields.Add(New GroupField(groupFieldName))
            groupBand.Controls.Add(GetGroupHeaderLabel(groupFieldName, indent, pageWidth))
            Return groupBand
        End Function

        Private Function GetGroupHeaderLabel(ByVal groupFieldName As String, ByVal indent As Single, ByVal pageWidth As Single) As XRControl
            Dim groupLabel As New XRLabel() With { _
                .SizeF = New SizeF(pageWidth - indent, 25), _
                .LocationF = New PointF(indent, 10), _
                .Font = New Font("Arial", 12, FontStyle.Bold) _
            }
            groupLabel.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", String.Format("[{0}]", groupFieldName)))
            Return groupLabel
        End Function
    End Class
End Namespace
