using Hospital_Management.Areas.User.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Areas.User.Data
{
    public class user_layer
    {
        string connection = "Data Source=HP-JAY;database=HospitalDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public IEnumerable<usermodel> GetAllUser()
        {
            List<usermodel> users = new List<usermodel>();
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usermodel user = new usermodel();
                    user.UserName = reader["Name"].ToString();
                    user.Email = reader["Email"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.RoleId = Convert.ToInt32(reader["Role_ID"]);
                    users.Add(user);
                }
                conn.Close();
            }
            return users;
        }

        public void AddUser(usermodel user, HttpContext httpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Name", user.UserName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@Role_ID", user.RoleId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        httpContext.Session.SetString("Name", user.UserName);
                        httpContext.Session.SetString("Email",user.Email);
                        httpContext.Session.SetString("Password",user.Password);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, you might want to log it or throw it further.
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
