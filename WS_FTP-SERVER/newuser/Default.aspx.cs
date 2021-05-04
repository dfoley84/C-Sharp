using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Deployment;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        errorDiv.Visible = false;
        successDiv.Visible = false;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       
        //Gobal Varables 
        string user = username.Text; 
        string userFullName = name.Text;
        string userEmail = email.Text;   
        string dropdown = null;
        string dropdownSub = null; 

        //USERID
        string USERIDsubAccount1 = null;
        string USERIDsubAccount2 = null;
        string USERIDsubAccount3 = null;
        string USERIDsubAccount4 = null;
        string USERIDsubAccount5 = null;
        
        //SQL Varables
        string sqlCheckUser = null;
        string SQLUpdatePasswordExpire = null;
        string SQLUpdateEmail = null;
        string SQLUpdateFullName = null;

        string updateSubAccount1 = null;
        string updateSubAccount2 = null;
        string updateSubAccount3 = null;
        string updateSubAccount4 = null;
        string updateSubAccount5 = null;

        string sqlCheckUsersubAccount1 = null;
        string sqlCheckUsersubAccount2 = null;
        string sqlCheckUsersubAccount3 = null;
        string sqlCheckUsersubAccount4 = null;
        string sqlCheckUsersubAccount5 = null;

        //Host Getting Folder Path
        string FOLDERPATHSUBACCOUN1 = null;
        string FOLDERPATHSUBACCOUN2 = null;
        string FOLDERPATHSUBACCOUN3 = null;
        string FOLDERPATHSUBACCOUN4 = null;
        string FOLDERPATHSUBACCOUN5 = null;

        
        string FoldersubAccount1 = null;
        string FoldersubAccount2 = null;
        string FoldersubAccount3 = null;
        string FoldersubAccount4 = null;
        string FoldersubAccount5 = null;

        //Passowrds
        PasswordGenerator passwordGenerator = PasswordGenerator.GetPasswordGenerator();
        string mainpass = passwordGenerator.NewPassword();
        string subpass1 = passwordGenerator.NewPassword();
        string subpass2 = passwordGenerator.NewPassword();
        string subpass3 = passwordGenerator.NewPassword();
        string subpass4 = passwordGenerator.NewPassword();
        string subpass5 = passwordGenerator.NewPassword();


        //Below Code will Add the Path to have the Folders Created.
        string FolderName = @"\\<NETWORK PATH>\e$\TEST_USER\users\" + user;
        string pathString = System.IO.Path.Combine(FolderName, user + "-c1");
        string pathString2 = System.IO.Path.Combine(FolderName, user + "-c2");
        string pathString3 = System.IO.Path.Combine(FolderName, user + "-c3");
        string pathString4 = System.IO.Path.Combine(FolderName, user + "-c4");
        string pathString5 = System.IO.Path.Combine(FolderName, user + "-c5");


        //Simple Text Field Vaildator
        if (user == String.Empty)
        {
            return;
        }//end if 
        else
        {
            //Creating If Statments for Groups and SubAccount Groups.
            if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
                dropdownSub = @""" sub Accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == " ")
            {
                dropdown = "";
                dropdownSub = @"""  Sub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == " ")
            {
                dropdown = @""" """;
                dropdownSub = @"""  Sub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
                dropdownSub = @""" Sub accounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
                dropdownSub = @""" subaccounts""";
            }
            else if (GroupDropDown.SelectedItem.Value == "")
            {
                dropdown = "";
                dropdownSub = @""" Sub accounts""";
            }


             /*
             * Connection TO SQL Database to Update the Map Drives / Add Email Address and finally Ad 42 day Account Lockout
             * */

            SqlConnection cnn;
            SqlCommand command;
            string ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            sqlCheckUser = "SELECT COUNT(*) FROM dbo.Host_Users WHERE User_LoginID=@user";

            
            cnn = new SqlConnection(ConnectionString);
            try
            {
                cnn.Open();
                using (command = new SqlCommand(sqlCheckUser, cnn))
                {
                    command.Parameters.Add("user", SqlDbType.Char).Value = user;
                    int userCount = (int)command.ExecuteScalar();
                    if (userCount > 0)
                    {
                        errorDiv.Visible = true;
                        successDiv.Visible = false;
                    }//end if
                    else
                    {
                        errorDiv.Visible = false;
                        successDiv.Visible = true;

                        /************************************************************
                         * If User Is not found within the WS_FTP Database Create Sub Accounts
                         * 
                         * */

                        //Creating User Directory  for Sub Accounts
                        System.IO.Directory.CreateDirectory(pathString);
                        System.IO.Directory.CreateDirectory(pathString2);
                        System.IO.Directory.CreateDirectory(pathString3);
                        System.IO.Directory.CreateDirectory(pathString4);
                        System.IO.Directory.CreateDirectory(pathString5);

                        using (Process process = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + " -h <HOSTNAME> -p " + mainpass + " +lock +root +g " + dropdown))
                        {
                            process.WaitForExit();
                        }
                          using (Process process1 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + "-c1 -h <HOSTNAME> -p " + subpass1 + " +lock +root +g " + dropdownSub))
                        {
                            process1.WaitForExit();
                        }
                          using (Process process2 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + "-c2 -h <HOSTNAME> -p " + subpass2 + " +lock +root +g " + dropdownSub))
                        {
                            process2.WaitForExit();
                        }
                          using (Process process3 = Process.Start (@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + "-c3 -h <HOSTNAME>c -p " + subpass3 + " +lock +root +g " + dropdownSub))
                        {
                            process3.WaitForExit();
                        }
                          using (Process process4 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + "-c4 -h <HOSTNAME> -p " + subpass4 + " +lock +root +g " + dropdownSub))
                        {
                            process4.WaitForExit();
                        }
                          using (Process process5 = Process.Start(@"C:\Program Files (x86)\Ipswitch\WS_FTP Server\iftpaddu.exe", "-add -u " + user + "-c5 -h <HOSTNAME> -p " + subpass5 + " +lock +root +g " + dropdownSub))
                        {
                            process5.WaitForExit();
                        }
                          //Update Expire Days, Enter in Email Address and Enter in Full Name
                        SQLUpdatePasswordExpire = "UPDATE dbo.Host_Users SET Pass_Expire_Days = 42 WHERE User_LoginID=@user";
                        SQLUpdateEmail = "UPDATE dbo.Host_Users SET User_Email_addr='" + userEmail + "' WHERE User_LoginID=@user";
                        SQLUpdateFullName = "UPDATE dbo.Host_Users SET User_FullName='" + userFullName + "' WHERE User_LoginID=@user";

                        //update Folder Path
                        updateSubAccount1 = "UPDATE dbo.Host_Users SET User_Home_Folder='/ftp_users/" + user + "/" + user + "-c1' WHERE User_LoginID='" + user + "-c1'";
                        updateSubAccount2 = "UPDATE dbo.Host_Users SET User_Home_Folder='/ftp_users/" + user + "/" + user + "-c2' WHERE User_LoginID='" + user + "-c2'";
                        updateSubAccount3 = "UPDATE dbo.Host_Users SET User_Home_Folder='/ftp_users/" + user + "/" + user + "-c3' WHERE User_LoginID='" + user + "-c3'";
                        updateSubAccount4 = "UPDATE dbo.Host_Users SET User_Home_Folder='/ftp_users/" + user + "/" + user + "-c4' WHERE User_LoginID='" + user + "-c4'";
                        updateSubAccount5 = "UPDATE dbo.Host_Users SET User_Home_Folder='/ftp_users/" + user + "/" + user + "-c5' WHERE User_LoginID='" + user + "-c5'";


                        //Getting SubAccounts
                        sqlCheckUsersubAccount1 = "SELECT * FROM dbo.Host_Users WHERE User_LoginID='" + user + "-c1'";
                        sqlCheckUsersubAccount2 = "SELECT * FROM dbo.Host_Users WHERE User_LoginID='" + user + "-c2'";
                        sqlCheckUsersubAccount3 = "SELECT * FROM dbo.Host_Users WHERE User_LoginID='" + user + "-c3'";
                        sqlCheckUsersubAccount4 = "SELECT * FROM dbo.Host_Users WHERE User_LoginID='" + user + "-c4'";
                        sqlCheckUsersubAccount5 = "SELECT * FROM dbo.Host_Users WHERE User_LoginID='" + user + "-c5'";
                        cnn = new SqlConnection(ConnectionString);
                         try
              {
                cnn.Open();
                using (command = new SqlCommand(sqlCheckUser, cnn))
                {
                    command.Parameters.Add("user", SqlDbType.Char).Value = user;
                    int userCount1 = (int)command.ExecuteScalar();
                    if (userCount1 > 0)
                    {
                        errorDiv.Visible = false;
                        successDiv.Visible = true;

                        //Update User Email Address
                        SqlCommand Udpateemail = new SqlCommand(SQLUpdateEmail, cnn);
                        Udpateemail.Parameters.Add("user", SqlDbType.Char).Value = user;
                        Udpateemail.ExecuteNonQuery();

                        //Update The Password Expire Days
                        SqlCommand cmd = new SqlCommand(SQLUpdatePasswordExpire, cnn);
                        cmd.Parameters.Add("user", SqlDbType.Char).Value = user;
                        cmd.ExecuteNonQuery();
                    
                        //Update User Full Name
                        SqlCommand updateName = new SqlCommand(SQLUpdateFullName, cnn);
                        updateName.Parameters.Add("user", SqlDbType.Char).Value = user;
                        updateName.ExecuteNonQuery();

                        //Update Sub Account 1 Dir Listing
                        SqlCommand subaccount1 = new SqlCommand(updateSubAccount1, cnn);
                        subaccount1.Parameters.Add("user", SqlDbType.Char).Value = user;
                        subaccount1.ExecuteNonQuery();

                        //Update Sub Account 2 Dir Listing
                        SqlCommand subaccount2 = new SqlCommand(updateSubAccount2, cnn);
                        subaccount2.Parameters.Add("user", SqlDbType.Char).Value = user;
                        subaccount2.ExecuteNonQuery();

                        //Update Sub Account 3 Dir Listing
                        SqlCommand subaccount3 = new SqlCommand(updateSubAccount3, cnn);
                        subaccount3.Parameters.Add("user", SqlDbType.Char).Value = user;
                        subaccount3.ExecuteNonQuery();

                        //Update Sub Account 4 Dir Listing
                        SqlCommand subaccount4 = new SqlCommand(updateSubAccount4, cnn);
                        subaccount4.Parameters.Add("user", SqlDbType.Char).Value = user;
                        subaccount4.ExecuteNonQuery();

                        //Update Sub Account 5 Dir Listing
                        SqlCommand subaccount5 = new SqlCommand(updateSubAccount5, cnn);
                        subaccount5.Parameters.Add("user", SqlDbType.Char).Value = user;
                        subaccount5.ExecuteNonQuery();

                                    /*********************************************************************
                                   * Sending Email with Password.
                                   * 
                                   * *************************************************/

                       string Str = " ";
                        Str += "SFTP Account has been Created for : " + " " + userFullName;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Username : " + " " + user;
                        Str += Environment.NewLine;
                        Str += "Main Account Password : " + " " + mainpass;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Sub Account 1 Password : " + " " + subpass1;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Sub Account 2 Password : " + " " + subpass2;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Sub Account 3 Password : " + " " + subpass3;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Sub Account 4 Password : " + " " + subpass4;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Sub Account 5 Password : " + " " + subpass5;
                        Str += Environment.NewLine;
                        Str += Environment.NewLine;
                        Str += "Thanks";


                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("");
                        mail.From = new MailAddress("");
                        mail.To.Add("");
                        mail.CC.Add(" ");
                        mail.Subject = "User Account Created for " + userFullName;
                        mail.Body = Str;
                        SmtpServer.Send(mail);
                        username.Text = string.Empty;
                    }//end if
                    else
                    { }
                }//End Using
            /***************************************************************************************
             * Updating Folder Permissions 
             * ***********************************************************************/
                //Sub Account 1 
                SqlDataAdapter da = new SqlDataAdapter(sqlCheckUsersubAccount1, cnn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    USERIDsubAccount1 = row["UserID"].ToString();
                    //Update Folder Permissions
                    string folderPerSub1 = "UPDATE dbo.Folder_Permissions SET Permission_Type = 0 , Permit_Read = 1 , Permit_Write = 1 ,  Permit_Delete = 1 , Permit_List = 1 , Permit_Folder_Create = 1 WHERE userID='" + USERIDsubAccount1 + "'";
                    SqlCommand folderpermissionSub1 = new SqlCommand(folderPerSub1, cnn);
                    folderpermissionSub1.ExecuteNonQuery();
                    //Getting FolderID 
                    FOLDERPATHSUBACCOUN1 = "SELECT * FROM dbo.Folder_Permissions WHERE UserID='" + USERIDsubAccount1 + "'";              
             }//For Each

                             //Sub Account 2 
                SqlDataAdapter db = new SqlDataAdapter(sqlCheckUsersubAccount2, cnn);
                DataTable dc = new DataTable();
                db.Fill(dc);
                foreach (DataRow row in dc.Rows)
                {
                    USERIDsubAccount2 = row["UserID"].ToString();
                    //Update Folder Permissions
                    string folderPerSub1 = "UPDATE dbo.Folder_Permissions SET Permission_Type = 0 , Permit_Read = 1 , Permit_Write = 1 ,  Permit_Delete = 1 , Permit_List = 1 , Permit_Folder_Create = 1 WHERE userID='" + USERIDsubAccount1 + "'";
                    SqlCommand folderpermissionSub1 = new SqlCommand(folderPerSub1, cnn);
                    folderpermissionSub1.ExecuteNonQuery();
                    //Getting FolderID 
                    FOLDERPATHSUBACCOUN2 = "SELECT * FROM dbo.Folder_Permissions WHERE UserID='" + USERIDsubAccount2 + "'";    
                }//For Each

                             //Sub Account 3 
                SqlDataAdapter de = new SqlDataAdapter(sqlCheckUsersubAccount3, cnn);
                DataTable df = new DataTable();
                de.Fill(df);

                foreach (DataRow row in df.Rows)
                {
                    USERIDsubAccount3 = row["UserID"].ToString();
                  
                    //Update Folder Permissions
                    string folderPerSub1 = "UPDATE dbo.Folder_Permissions SET Permission_Type = 0 , Permit_Read = 1 , Permit_Write = 1 ,  Permit_Delete = 1 , Permit_List = 1 , Permit_Folder_Create = 1 WHERE userID='" + USERIDsubAccount1 + "'";
                    SqlCommand folderpermissionSub1 = new SqlCommand(folderPerSub1, cnn);
                    folderpermissionSub1.ExecuteNonQuery();

                    //Getting FolderID 
                    FOLDERPATHSUBACCOUN3 = "SELECT * FROM dbo.Folder_Permissions WHERE UserID='" + USERIDsubAccount3 + "'";
            
                        
                }//For Each

                             //SubAccount 4
                SqlDataAdapter dg = new SqlDataAdapter(sqlCheckUsersubAccount4, cnn);
                DataTable dh = new DataTable();
                dg.Fill(dh);

                foreach (DataRow row in dh.Rows)
                {
                    USERIDsubAccount4 = row["UserID"].ToString();
                    
                    //Update Folder Permissions
                    string folderPerSub1 = "UPDATE dbo.Folder_Permissions SET Permission_Type = 0 , Permit_Read = 1 , Permit_Write = 1 ,  Permit_Delete = 1 , Permit_List = 1 , Permit_Folder_Create = 1 WHERE userID='" + USERIDsubAccount1 + "'";
                    SqlCommand folderpermissionSub1 = new SqlCommand(folderPerSub1, cnn);
                    folderpermissionSub1.ExecuteNonQuery();

                    //Getting FolderID 
                    FOLDERPATHSUBACCOUN4 = "SELECT * FROM dbo.Folder_Permissions WHERE UserID='" + USERIDsubAccount4 + "'";

                        
                }//For Each

                          //   SubAccount 5
                SqlDataAdapter di = new SqlDataAdapter(sqlCheckUsersubAccount5, cnn);
                DataTable dj = new DataTable();
                di.Fill(dj);

                foreach (DataRow row in dj.Rows)
                {
                    USERIDsubAccount5 = row["UserID"].ToString();
                  
                    //Update Folder Permissions
                    string folderPerSub1 = "UPDATE dbo.Folder_Permissions SET Permission_Type = 0 , Permit_Read = 1 , Permit_Write = 1 ,  Permit_Delete = 1 , Permit_List = 1 , Permit_Folder_Create = 1 WHERE userID='" + USERIDsubAccount1 + "'";
                    SqlCommand folderpermissionSub1 = new SqlCommand(folderPerSub1, cnn);
                    folderpermissionSub1.ExecuteNonQuery();

                    //Getting FolderID 
                    FOLDERPATHSUBACCOUN5 = "SELECT * FROM dbo.Folder_Permissions WHERE UserID='" + USERIDsubAccount5 + "'";
                        
                }//For Each
             
            /***************************************************************************************
            * END OF Updating Folder Permissions 
            * ***********************************************************************/

                             /**************************************************************************
                              * UPDATE FOLDER PATH FOR SUB ACCOUNTS
                              * *********************************************************/
                //Updating PATH Sub Account 1 
                SqlDataAdapter dk = new SqlDataAdapter(FOLDERPATHSUBACCOUN1, cnn);
                DataTable dl = new DataTable();
                dk.Fill(dl);
                foreach (DataRow row in dl.Rows)
                {
                    FoldersubAccount1 = row["FolderID"].ToString();
                  
                    //Update Folder Permissions
                    string FolderHostSub1 = "UPDATE dbo.Host_Folders SET Relative_Path ='/ftp_users/" + user + "/" + user + "-c1' WHERE FolderID='" + FoldersubAccount1 + "'";
                    SqlCommand folderSub1 = new SqlCommand(FolderHostSub1, cnn);
                    folderSub1.ExecuteNonQuery();

                }//For Each

                //Updating PATH Sub Account 2
                SqlDataAdapter dm = new SqlDataAdapter(FOLDERPATHSUBACCOUN2, cnn);
                DataTable dn = new DataTable();
                dm.Fill(dn);
                foreach (DataRow row in dn.Rows)
                {
                    FoldersubAccount2 = row["FolderID"].ToString();
                    
                    //Update Folder Permissions
                    string FolderHostSub2 = "UPDATE dbo.Host_Folders SET Relative_Path ='/ftp_users/" + user + "/" + user + "-c2' WHERE FolderID='" + FoldersubAccount2 + "'";
                    SqlCommand folderSub2 = new SqlCommand(FolderHostSub2, cnn);
                    folderSub2.ExecuteNonQuery();

                }//For Each


                //Updating PATH Sub Account 3 
                SqlDataAdapter dp = new SqlDataAdapter(FOLDERPATHSUBACCOUN3, cnn);
                DataTable dq = new DataTable();
                dp.Fill(dq);
                foreach (DataRow row in dq.Rows)
                {
                    FoldersubAccount3 = row["FolderID"].ToString();
                   
                    //Update Folder Permissions
                    string FolderHostSub3 = "UPDATE dbo.Host_Folders SET Relative_Path ='/ftp_users/" + user + "/" + user + "-c3' WHERE FolderID='" + FoldersubAccount3 + "'";
                    SqlCommand folderSub3 = new SqlCommand(FolderHostSub3, cnn);
                    folderSub3.ExecuteNonQuery();

                }//For Each


                //Updating PATH Sub Account 4 
                SqlDataAdapter dr = new SqlDataAdapter(FOLDERPATHSUBACCOUN4, cnn);
                DataTable ds = new DataTable();
                dr.Fill(ds);
                foreach (DataRow row in ds.Rows)
                {
                    FoldersubAccount4 = row["FolderID"].ToString();
                  
                    //Update Folder Permissions
                    string FolderHostSub4 = "UPDATE dbo.Host_Folders SET Relative_Path ='/ftp_users/" + user + "/" + user + "-c4' WHERE FolderID='" + FoldersubAccount4 + "'";
                    SqlCommand folderSub4 = new SqlCommand(FolderHostSub4, cnn);
                    folderSub4.ExecuteNonQuery();

                }//For Each


                //Updating PATH Sub Account 4 
                SqlDataAdapter du = new SqlDataAdapter(FOLDERPATHSUBACCOUN5, cnn);
                DataTable dv = new DataTable();
                du.Fill(dv);
                foreach (DataRow row in dv.Rows)
                {
                    FoldersubAccount5 = row["FolderID"].ToString();
                
                    //Update Folder Permissions
                    string FolderHostSub5 = "UPDATE dbo.Host_Folders SET Relative_Path ='/ftp_users/" + user + "/" + user + "-c5' WHERE FolderID='" + FoldersubAccount5 + "'";
                    SqlCommand folderSub5 = new SqlCommand(FolderHostSub5, cnn);
                    folderSub5.ExecuteNonQuery();

                }//For Each
                  /**************************************************************************
                   *END  UPDATE FOLDER PATH FOR SUB ACCOUNTS
                   * *********************************************************/
            }//End Try
            catch (Exception ex)
            { }//End Catch
                }//
              }//end using
                command.Dispose();
                cnn.Close();
            }//end Try
            catch(Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Not Connected')", true);
            }//End Catch
        }//end else
    }//End Button on Click

    /********************************************************************************
       * Password Generator
       ********************************************************************** */
    public sealed class PasswordGenerator
    {
        private static readonly PasswordGenerator instance = new PasswordGenerator();
        private Random random = new Random();
        private PasswordGenerator() { }
        public static PasswordGenerator GetPasswordGenerator()
        {
            return instance;
        }
        int lowerscase = ;
        int uppercase = ;
        int numbercase = ;
        int spe = ;
        private const string lowers = "abcdefghijklmnopqrstuvwxyz";
        private const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string number = "0123456789";
        private const string specail = "{$}?_=/+-!*";
        public string NewPassword()
        {
            string generated = "!";
            for (int i = 1; i <= lowerscase; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    lowers[this.random.Next(lowers.Length - 1)].ToString()
                );
            for (int i = 1; i <= spe; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    specail[this.random.Next(specail.Length - 1)].ToString()
                );
            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    uppers[this.random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numbercase; i++)
                generated = generated.Insert(
                    this.random.Next(generated.Length),
                    number[this.random.Next(number.Length - 1)].ToString()
                );
            return generated.Replace("!", string.Empty);
        }//END PASSWORD
    }//END PASSWORD FUNCTION
}//End C# Class


                   

                   