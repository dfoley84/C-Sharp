# WS_FTP-SERVER
WS_FTP Server is a Server based software for secure file Transfers, 
from Ipswitch.  you can manage users from within the GUI Console or from the the Command-line.
if you work have ever worked within WS_FTP you will find that creating users or even managing them can be time consuming 
With this in Mind i have Created an ASP site that will manage the automation of the following; 

Create New User with Sub Accounts. 
Create New User with No Sub Accounts.
Disable User Accounts. Reset Passwords. 
Reset Sub Account Passwords.  

i have also created two powershell scripts as follows; 

Notify User ---> This Application will query the database for any account that will expire within 7 days
it will then email the User reminding them that there password is about to expire in X amount of Days. 

Password Reset. ---> Can be found on the Password.Txt File the program does the following; 
in the event that a User choices to irong the notification emails stating their 
account will expire in X amount of Days, this will automatically 
give a temporary password for any account that will expire within 2 days
