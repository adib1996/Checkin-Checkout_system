#ExternalChecksum ("..\..\..\Form\Login.xaml", "{ff1816ec-aa5e-4d10-87f7-6f4963833460}", "8102378EE0AB0F84AB366958E4C8293EF8F0E46C")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports EasyKeyLibrarySystem
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''Login
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Login
    Inherits System.Windows.Controls.Page
    Implements System.Windows.Markup.IComponentConnector

#End ExternalSource


#ExternalSource ("..\..\..\Form\Login.xaml", 20)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents Password As System.Windows.Controls.PasswordBox

#End ExternalSource


#ExternalSource ("..\..\..\Form\Login.xaml", 21)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")> _
    Friend WithEvents LoginButton As System.Windows.Controls.Button

#End ExternalSource

    Private _contentLoaded As Boolean

    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")> _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/EasyKeyLibrarySystem;component/form/login.xaml", System.UriKind.Relative)

#ExternalSource ("..\..\..\Form\Login.xaml", 1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)

#End ExternalSource
    End Sub

    <System.Diagnostics.DebuggerNonUserCodeAttribute(), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"), _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never), _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"), _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")> _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.TPNumber = CType(target, System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 2) Then
            Me.Password = CType(target, System.Windows.Controls.PasswordBox)
            Return
        End If
        If (connectionId = 3) Then
            Me.LoginButton = CType(target, System.Windows.Controls.Button)
            Return
        End If
        Me._contentLoaded = true
    End Sub

    Friend WithEvents TPNumber As System.Windows.Controls.TextBox
End Class

