using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace CustomNotifications.Module.BusinessObjects {
    public class CustomNotificationsDbContext : DbContext {
		public CustomNotificationsDbContext(String connectionString)
			: base(connectionString) {
		}
		public CustomNotificationsDbContext(DbConnection connection)
			: base(connection, false) {
		}
		public DbSet<ModuleInfo> ModulesInfo { get; set; }
        public DbSet<Task> Tasks { get; set; }
	}
    [DefaultClassOptions]
    public class Task : ISupportNotifications, IXafEntityObject, INotifyPropertyChanged {
        public int Id { get; private set; }
        private string subject;
        public string Subject {
            get { return subject; }
            set { SetProperty<string>(ref subject, value); }
        }
        private DateTime dueDate;
        public DateTime DueDate {
            get { return dueDate; }
            set { SetProperty<DateTime>(ref dueDate, value);}
        }

        #region ISupportNotifications members
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if(!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        private DateTime? alarmTime;
        [Browsable(false)]
        public DateTime? AlarmTime {
            get { return alarmTime; }
            set {
                SetProperty<DateTime?>(ref alarmTime, value);
                if (value == null) {
                    RemindIn = null;
                    IsPostponed = false;
                }
            }
        }
        private bool isPostponed;
        [Browsable(false)]
        public bool IsPostponed {
            get { return isPostponed;}
            set { SetProperty<bool>(ref isPostponed, value);}
        }
        [Browsable(false), NotMapped]
        public string NotificationMessage {
            get { return Subject; }
        }
        private TimeSpan? remindIn;
        public TimeSpan? RemindIn {
            get { return remindIn; }
            set { SetProperty<TimeSpan?>(ref remindIn, value); }
        }
       
        [Browsable(false), NotMapped]
        public object UniqueId {
            get { return Id; }
        }
        #endregion
        
        #region IXafEntityObject members
        public void OnCreated() { }
        public void OnLoaded() { }
        public void OnSaving() {
            if(RemindIn.HasValue) {
                if((AlarmTime == null) || (AlarmTime < DueDate - RemindIn.Value)) {
                    AlarmTime = DueDate - RemindIn.Value;
                }
            }
            else {
                AlarmTime = null;
            }
            if (AlarmTime == null) {
                RemindIn = null;
                IsPostponed = false;
            }
        }
        #endregion

        #region INotifyPropertyChanged members
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}