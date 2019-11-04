Namespace XtraReport_JsonDataSource_with_Authorization
    Partial Public Class Form1
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        #Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.button1 = New System.Windows.Forms.Button()
            Me.button2 = New System.Windows.Forms.Button()
            Me.label1 = New System.Windows.Forms.Label()
            Me.label2 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            ' 
            ' button1
            ' 
            Me.button1.Location = New System.Drawing.Point(15, 86)
            Me.button1.Name = "button1"
            Me.button1.Size = New System.Drawing.Size(271, 23)
            Me.button1.TabIndex = 0
            Me.button1.Text = "Design-Time JSON Authentication"
            Me.button1.UseVisualStyleBackColor = True
            ' 
            ' button2
            ' 
            Me.button2.Location = New System.Drawing.Point(307, 86)
            Me.button2.Name = "button2"
            Me.button2.Size = New System.Drawing.Size(235, 23)
            Me.button2.TabIndex = 1
            Me.button2.Text = "Runtime JSON Authentication"
            Me.button2.UseVisualStyleBackColor = True
            ' 
            ' label1
            ' 
            Me.label1.Location = New System.Drawing.Point(12, 26)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(286, 57)
            Me.label1.TabIndex = 2
            Me.label1.Text = "Example1:" & ControlChars.CrLf & "Show the Report Designer with a customized Specify JSON Data Location " & "wizard page. Specify the user name and password to connect to the Web Service En" & "dpoint(URI) on this page." & ControlChars.CrLf
            ' 
            ' label2
            ' 
            Me.label2.Location = New System.Drawing.Point(304, 26)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(247, 43)
            Me.label2.TabIndex = 2
            Me.label2.Text = "Example2:" & ControlChars.CrLf & "Show the Report Designer to display a report that is bound to a custom" & " JsonDataSource with the specified user name and password."
            ' 
            ' Form1
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(563, 139)
            Me.Controls.Add(Me.label2)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.button2)
            Me.Controls.Add(Me.button1)
            Me.Name = "Form1"
            Me.Text = "JsonDataSource with Authorization"
            Me.ResumeLayout(False)

        End Sub

        #End Region

        Private WithEvents button1 As System.Windows.Forms.Button
        Private WithEvents button2 As System.Windows.Forms.Button
        Private label1 As System.Windows.Forms.Label
        Private label2 As System.Windows.Forms.Label
    End Class
End Namespace

