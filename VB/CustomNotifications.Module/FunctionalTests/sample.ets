;#DropDB CustomNotificationsEasyTest

#Application CustomNotifications.Win
#Application CustomNotifications.Web
#IfDef CustomNotifications.Win

*Action About...
 
*Action OK

#EndIf

#IfDef CustomNotifications.Web

*Action Choose Theme(Black Glass)

#EndIf
