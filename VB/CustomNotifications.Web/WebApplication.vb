Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Web
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.EF
Imports CustomNotifications.Module.BusinessObjects
Imports System.Data.Common

Namespace CustomNotifications.Web
	' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWebWebApplicationMembersTopicAll
	Partial Public Class CustomNotificationsAspNetApplication
		Inherits WebApplication
		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
		Private module3 As CustomNotifications.Module.CustomNotificationsModule
		Private module4 As CustomNotifications.Module.Web.CustomNotificationsAspNetModule
		Private notificationsModule As DevExpress.ExpressApp.Notifications.NotificationsModule
		Private notificationsModuleWeb As DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule

		Public Sub New()
			InitializeComponent()
		End Sub
		Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
			If args.Connection IsNot Nothing Then
				args.ObjectSpaceProvider = New EFObjectSpaceProvider(GetType(CustomNotificationsDbContext), TypesInfo, Nothing, CType(args.Connection, DbConnection))
			Else
				args.ObjectSpaceProvider = New EFObjectSpaceProvider(GetType(CustomNotificationsDbContext), TypesInfo, Nothing, args.ConnectionString)
			End If
		End Sub
		Private Sub CustomNotificationsAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
			e.Updater.Update()
			e.Handled = True
#Else
			If System.Diagnostics.Debugger.IsAttached Then
				e.Updater.Update()
				e.Handled = True
			Else
				Dim message As String = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & Constants.vbCrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & Constants.vbCrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & Constants.vbCrLf & "Anyway, refer to the following help topics for more detailed information:" & Constants.vbCrLf & "'Update Application and Database Versions' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm" & Constants.vbCrLf & "'Database Security References' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument3237.htm" & Constants.vbCrLf & "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/"

				If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
					message &= Constants.vbCrLf & Constants.vbCrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
				End If
				Throw New InvalidOperationException(message)
			End If
#End If
		End Sub
		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
			Me.module3 = New CustomNotifications.Module.CustomNotificationsModule()
			Me.module4 = New CustomNotifications.Module.Web.CustomNotificationsAspNetModule()
			Me.notificationsModule = New DevExpress.ExpressApp.Notifications.NotificationsModule()
			Me.notificationsModuleWeb = New DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' CustomNotificationsAspNetApplication
			' 
			Me.ApplicationName = "CustomNotifications"
			Me.CollectionsEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.module4)
			Me.Modules.Add(Me.notificationsModule)
			Me.Modules.Add(Me.notificationsModuleWeb)
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.CustomNotificationsAspNetApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub
	End Class
End Namespace
