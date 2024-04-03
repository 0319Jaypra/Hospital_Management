using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;

namespace Hospital_Management.Areas.Doctor.Data
{
    public class layer
    {
        string connection = "Data Source=HP-JAY;database=HospitalDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        
        public IEnumerable<DoctorModel> GetDoctorProfile()
        {
            List<DoctorModel> list = new List<DoctorModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetDoctorProfile", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DoctorModel user = new DoctorModel();
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Designation = reader["Designation"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }

        public DoctorModel GetDoctorProfileById(int id)
        {
            DoctorModel user = new DoctorModel();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDoctorById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Designation = reader["Designation"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    
                }
                conn.Close();
            }
            return user;
        }

        public void DeleteDoctorAvailabilityByUserId(TimeSpan Stime)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteDoctorAvailabilityByUserId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Start_Time", Stime);
                conn.Open();
                cmd.ExecuteNonQuery();

                return ;
            }
        }

        public available GetDoctorAvailableProfileById(string id)
        {
            available user = new available();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDoctorById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    

                }
                conn.Close();
            }
            return user;
        }

        public IEnumerable<AppoinmnetModel> GetAppointments()
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAppoinment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                 // Corrected parameter name
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AppoinmnetModel appointment = new AppoinmnetModel();
                    appointment.Appoinment_Id = Convert.ToInt32(reader["Appoinment_Id"]);
                    appointment.User_Id = reader["User_ID"].ToString();
                    appointment.Patient_Name = reader["Patient_Name"].ToString();
                    appointment.Doctor_Name = reader["Doctor_Name"].ToString();
                    appointment.Date = reader["Date"].ToString();
                    appointment.TimeDuration = reader["TimeDuration"].ToString();
                    appointment.Time = (TimeSpan)reader["Time"];
                    appointment.etime = (TimeSpan)reader["etime"];
                    appointment.isSelect = Convert.ToBoolean(reader["isSelect"]);

                    list.Add(appointment);
                }
            }

            return list;
        }
      
        public void DoctorProfile(DoctorModel doctor, HttpContext httpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Doctor", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User_ID", doctor.User_Id);
                        cmd.Parameters.AddWithValue("@Name",doctor.Name);
                        cmd.Parameters.AddWithValue("@Designation",doctor.Designation );
                        cmd.Parameters.AddWithValue("@Phone", doctor.Phone);
                        cmd.Parameters.AddWithValue("@Gender", doctor.Gender);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void DoctorAvailableProfile(available doctor, HttpContext httpContext)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Available", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User_ID", doctor.User_Id);
                        cmd.Parameters.AddWithValue("@Name", doctor.Name);
                        cmd.Parameters.AddWithValue("@Date", doctor.Date);
                        cmd.Parameters.AddWithValue("@Stime", doctor.STime);
                        cmd.Parameters.AddWithValue("@Etime", doctor.ETime);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here, you might want to log it or throw it further.
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public IEnumerable<AppoinmnetModel> GetAppointments(string name)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    SqlCommand cmd = new SqlCommand("GetAppoinmentByname", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Doctor_Name", name);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AppoinmnetModel appointment = new AppoinmnetModel();
                            appointment.Appoinment_Id = Convert.ToInt32(reader["Appoinment_Id"]);
                            appointment.User_Id = reader["User_Id"].ToString();
                            appointment.Patient_Name = reader["Patient_Name"].ToString();
                            appointment.Doctor_Name = reader["Doctor_Name"].ToString();
                            appointment.Date = reader["Date"].ToString();
                            appointment.TimeDuration = reader["TimeDuration"].ToString();
                            appointment.Time = (TimeSpan)reader["Time"];
                            appointment.etime = (TimeSpan)reader["etime"];

                            list.Add(appointment);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving appointments: " + ex.Message);
            }

            return list;
        }
        public IEnumerable<available> GetDoctorAvailableProfile(string id)
        {
            List<available> list = new List<available>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailable", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue ("@User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    available user = new available();
                    user.User_Id = Convert.ToInt32(reader["User_ID"]);
                    user.Name = reader["Name"].ToString();
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());
                    user.STime = (TimeSpan)reader["Start_Time"];
                    user.ETime = (TimeSpan)reader["End_Time"];
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }

        public void resetpassword(resetpasswordviewmodel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("sp_Password", con);
                    command.Parameters.AddWithValue("@UserId", model.UserId);
                    command.Parameters.AddWithValue("@OldPassword", model.OldPassword);
                    int count = (int)command.ExecuteScalar();
                    if (count == 1)
                    {
                        SqlCommand cmd = new SqlCommand("ResetPassword", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserId", model.UserId);
                        cmd.Parameters.AddWithValue("@NewPassword", model.NewPassword);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Password reset successfully.");
                    }
                    else
                    {
                        Console.WriteLine("User ID or old password is incorrect.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
