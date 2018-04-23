Imports System.Linq
Imports System.Data.Entity
Imports System.Data.Common
Imports System.Data.Entity.Core.Objects
Imports System.Data.Entity.Infrastructure
Imports System.ComponentModel
Imports DevExpress.ExpressApp.EF.Updating
Imports DevExpress.Persistent.BaseImpl.EF
Imports DevExpress.Persistent.Base.General
Imports System.ComponentModel.DataAnnotations.Schema

Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Model

Namespace CustomNotifications.Module.BusinessObjects
    Public Class CustomNotificationsDbContext
        Inherits DbContext

        Public Sub New(ByVal connectionString As String)
            MyBase.New(connectionString)
        End Sub
        Public Sub New(ByVal connection As DbConnection)
            MyBase.New(connection, False)
        End Sub
        Public Property ModulesInfo() As DbSet(Of ModuleInfo)
        Public Property Tasks() As DbSet(Of Task)
    End Class
    <DefaultClassOptions> _
    Public Class Task
        Implements ISupportNotifications, IXafEntityObject

        Private privateId As Integer
        Public Property Id() As Integer
            Get
                Return privateId
            End Get
            Private Set(ByVal value As Integer)
                privateId = value
            End Set
        End Property
        Public Property Subject() As String
        Public Property DueDate() As Date

        #Region "ISupportNotifications members"
        Private _alarmTime? As Date
        <Browsable(False)> _
        Public Property AlarmTime() As Date? Implements ISupportNotifications.AlarmTime
            Get
                Return _alarmTime
            End Get
            Set(ByVal value? As Date)
                _alarmTime = value
                If value Is Nothing Then
                    RemindIn = Nothing
                    IsPostponed = False
                End If
            End Set
        End Property
        <Browsable(False)> _
        Public Property IsPostponed() As Boolean Implements ISupportNotifications.IsPostponed
        <Browsable(False), NotMapped> _
        Public ReadOnly Property NotificationMessage() As String Implements ISupportNotifications.NotificationMessage
            Get
                Return Subject
            End Get
        End Property
        Public Property RemindIn() As TimeSpan?

        <Browsable(False), NotMapped> _
        Public ReadOnly Property UniqueId() As Object Implements ISupportNotifications.UniqueId
            Get
                Return Id
            End Get
        End Property
        #End Region



        #Region "IXafEntityObject members"
        Public Sub OnCreated() Implements IXafEntityObject.OnCreated
        End Sub
        Public Sub OnLoaded() Implements IXafEntityObject.OnLoaded
        End Sub
        Public Sub OnSaving() Implements IXafEntityObject.OnSaving
	    If RemindIn.HasValue Then
		If (AlarmTime Is Nothing) OrElse (AlarmTime < DueDate - RemindIn.Value) Then
		   AlarmTime = DueDate - RemindIn.Value
		End If
		Else
		   AlarmTime = Nothing
		End If
		If AlarmTime Is Nothing Then
		   RemindIn = Nothing
		   IsPostponed = False
		End If
        End Sub
        #End Region
    End Class
End Namespace