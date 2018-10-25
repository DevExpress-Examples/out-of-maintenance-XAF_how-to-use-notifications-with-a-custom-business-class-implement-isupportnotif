using System;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Base.General;
using System.ComponentModel.DataAnnotations.Schema;

using DevExpress.Persistent.Base;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace  CustomNotifications.Module.BusinessObjects {
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
    public class Task : ISupportNotifications, IXafEntityObject {
        public int Id { get; private set; }
        public string Subject { get; set; }
        public DateTime DueDate { get; set; }

        #region ISupportNotifications members
        private DateTime? alarmTime;
        [Browsable(false)]
        public DateTime? AlarmTime {
            get { return alarmTime; }
            set {
                alarmTime = value;
                if (value == null) {
                    RemindIn = null;
                    IsPostponed = false;
                }
            }
        }
        [Browsable(false)]
        public bool IsPostponed { get; set; }
        [Browsable(false), NotMapped]
        public string NotificationMessage {
            get { return Subject; }
        }
        public TimeSpan? RemindIn { get; set; }
       
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
    }
}