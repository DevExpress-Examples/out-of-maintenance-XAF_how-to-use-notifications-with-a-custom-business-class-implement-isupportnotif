Imports Microsoft.VisualBasic
Imports System
Imports System.Data
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
Imports System.Runtime.CompilerServices
Imports System.Collections.Generic

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
    <DefaultClassOptions>
    Public Class Task
        Implements ISupportNotifications, IXafEntityObject, INotifyPropertyChanged

        Private privateId As Integer
        Public Property Id() As Integer
            Get
                Return privateId
            End Get
            Private Set(ByVal value As Integer)
                privateId = value
            End Set
        End Property
        Private _subject As String
        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(value As String)
                SetProperty(Of String)(_subject, value)
            End Set
        End Property
        Private _dueDate As Date
        Public Property DueDate() As Date
            Get
                Return _dueDate
            End Get
            Set(value As Date)
                SetProperty(Of Date)(_dueDate, value)
            End Set
        End Property

#Region "ISupportNotifications members"
        Private _alarmTime? As Date
        <Browsable(False)>
        Public Property AlarmTime() As Date? Implements ISupportNotifications.AlarmTime
            Get
                Return _alarmTime
            End Get
            Set(ByVal value? As Date)
                SetProperty(Of Date?)(_alarmTime, value)
                If value Is Nothing Then
                    RemindIn = Nothing
                    IsPostponed = False
                End If
            End Set
        End Property
        Private _isPostponed As Boolean
        <Browsable(False)>
        Public Property IsPostponed() As Boolean Implements ISupportNotifications.IsPostponed
            Get
                Return _isPostponed
            End Get
            Set(value As Boolean)
                SetProperty(Of Boolean)(_isPostponed, value)
            End Set
        End Property
        <Browsable(False), NotMapped>
        Public ReadOnly Property NotificationMessage() As String Implements ISupportNotifications.NotificationMessage
            Get
                Return Subject
            End Get
        End Property
        Private _remindIn As TimeSpan?
        Public Property RemindIn() As TimeSpan?
            Get
                Return _remindIn
            End Get
            Set(value As TimeSpan?)
                SetProperty(Of TimeSpan?)(_remindIn, value)
            End Set
        End Property

        <Browsable(False), NotMapped>
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
        Protected Sub SetProperty(Of T)(ByRef field As T, ByVal value As T, <CallerMemberName> Optional ByVal propertyName As String = Nothing)
            If (Not EqualityComparer(Of T).[Default].Equals(field, value)) Then
                field = value
                OnPropertyChanged(propertyName)
            End If
        End Sub
#Region "INotifyPropertyChanged"
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
        Protected Sub OnPropertyChanged(ByVal propertyName As String)
            PropertyChangedEvent?.Invoke(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
#End Region
    End Class
End Namespace