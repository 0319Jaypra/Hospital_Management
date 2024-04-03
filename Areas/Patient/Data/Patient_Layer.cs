using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Hospital_Management.Areas.Patient.Data
{
    public class Patient_Layer
    {
        string connection = "Data Source=HP-JAY;database=HospitalDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public IEnumerable<AppoinmnetModel> GetAppointments(string userId, HttpContext httpContext)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connection))
                {
                    SqlCommand cmd = new SqlCommand("GetAppoinmentById", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@User_Id", userId);
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
                    else
                    {
                        Console.WriteLine("No appointments found for user ID: " + userId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving appointments: " + ex.Message);
            }

            return list;
        }

        public IEnumerable<AppoinmnetModel> GetAllAppointments()
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAllAppoinment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                  
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
                    
                    list.Add(appointment);
                }
            }

            return list;
        }

        public IEnumerable<AppoinmnetModel> MarkAppointmentAsApproved(string id,string name)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAllAppoinmentByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Appoinment_Id", id);
                cmd.Parameters.AddWithValue("@Patient_Name", name);
                
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
                    appointment.isSelect = (bool)reader["isSelect"];

                    list.Add(appointment);
                }
            }

            return list;
        }

        public IEnumerable<AppoinmnetModel> GetAppoinmnetModel(string userId)
        {
            List<AppoinmnetModel> list = new List<AppoinmnetModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetUserById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", userId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    AppoinmnetModel appointment = new AppoinmnetModel();
                    
                    appointment.User_Id = reader["User_ID"].ToString();
                    appointment.Patient_Name = reader["Patient_Name"].ToString();
                    appointment.Doctor_Name = reader["Doctor_Name"].ToString();
                    appointment.Date =reader["Date"].ToString();
                    appointment.TimeDuration = reader["TimeDuration"].ToString();
                    appointment.Time = (TimeSpan)reader["Time"];
                    appointment.etime = (TimeSpan)reader["etime"];
                    list.Add(appointment);
                }
            }

            return list;
        }

        public IEnumerable<DoctorModel> GetDoctorProfile(string id)
        {
            List<DoctorModel> list = new List<DoctorModel>();
            
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("DoctorListWithDate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", id);
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
                    user.Stime = (TimeSpan)reader["Start_Time"];
                    user.Etime = (TimeSpan)reader["End_Time"];
                   
                    list.Add(user);
                }
                conn.Close();
            }
            return list;
        }


      

        public void UpdateAppointmentStatus(string Appoinment_Id)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("UpdateAppointmentDoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Appoinment_Id", Appoinment_Id);
               
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateAllAppointmentStatus(string Appoinment_Id, bool isDone)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("UpdateAllAppointmentDoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Appoinment_Id", Appoinment_Id);
                cmd.Parameters.AddWithValue("@isSelect", isDone);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public bool GetIsDoneStatus(string appointmentId)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetIsDoneStatus", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Appoinment_Id", appointmentId);
                conn.Open();
                // Execute the command and get the result
                object result = cmd.ExecuteScalar();

                // Check if the result is not null and convert it to a bool
                if (result != null)
                {
                    // Assuming the result is of type long (Int64)
                    long value = (long)result;
                    return value == 1; // Convert to bool based on the expected logic
                }
                else
                {
                    // Handle the case when the result is null
                    return false;
                }
            }
        }
        public available GetDoctorAvailableProfileById(string id)
        {
            available user = new available();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailableById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Name = reader["Name"].ToString();
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());

                }
                conn.Close();
            }
            return user;
        }

        public available GetAvailableDatesForDoctor(string Name)
        {
            available user = new available();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetAvailableByName", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Name", Name);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Date = Convert.ToDateTime(reader["Date"].ToString());
                }
                conn.Close();
            }
            return user;
        }

        public usermodel GetPatientProfileById(string id)
        {
            usermodel user = new usermodel();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("GetUserById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("User_ID", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.UserName = reader["Name"].ToString();

                }
                conn.Close();
            }
            return user;
        }
        public void AddAppoinment(AppoinmnetModel appoinmnet)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand("Add_Appoinment", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User_ID", appoinmnet.User_Id);
                        cmd.Parameters.AddWithValue("@Patient_Name", appoinmnet.Patient_Name);
                        cmd.Parameters.AddWithValue("@Doctor_Name", appoinmnet.Doctor_Name);
                        cmd.Parameters.AddWithValue("@Date", appoinmnet.Date);
                        cmd.Parameters.AddWithValue("@TimeDuration", appoinmnet.TimeDuration);
                        cmd.Parameters.AddWithValue("@Time", appoinmnet.Time);
                        cmd.Parameters.AddWithValue("@etime", appoinmnet.etime);
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

    }
}
