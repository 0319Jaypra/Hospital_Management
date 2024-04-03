using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Data;
using Hospital_Management.Areas.Patient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management.Areas.Patient.Controllers
{
   
    public class PatientController : Controller
    {
        Patient_Layer layer = new Patient_Layer();
        public IActionResult AppoinmnetList(string date)
        {
            string id = HttpContext.Session.GetString("userid");
            IEnumerable<AppoinmnetModel> appoinmnets = new List<AppoinmnetModel>();
           
                appoinmnets = layer.GetAppointments(id, HttpContext).ToList();
            

            return View(appoinmnets);
        }

        public IActionResult GetAppointmentsByDate( string date)
        {
            string connection = "Data Source=HP-JAY;database=HospitalDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            List<DoctorModel> appointments = new List<DoctorModel>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"
            SELECT U.[User_ID], 
                   U.[Name], 
                   U.[Designation], 
                   U.[Phone], 
                   U.[Gender],
                   A.[Start_Time], 
                   A.[End_Time] 
            FROM [dbo].[User_tbl] U
            JOIN [dbo].[Available_tbl] A ON U.[User_ID] = A.[User_Id]
            WHERE CONVERT(DATE, @Date) BETWEEN CONVERT(DATE, A.[Date]) AND CONVERT(DATE, A.[Date])";

                SqlCommand cmd = new SqlCommand(query, conn);
               
                cmd.Parameters.AddWithValue("@Date", date);

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
                    appointments.Add(user);
                }

                conn.Close();
            }

            return Json(appointments);
        }
        [HttpGet]
        public IActionResult AddAppoinment(string id)
        {
            string userId = HttpContext.Session.GetString("userid");

            var patient = layer.GetPatientProfileById(userId);
            var doctor = layer.GetDoctorAvailableProfileById(id);

            var model = new AppoinmnetModel
            {
                User_Id = userId,
                Patient_Name =patient.UserName,
                Doctor_Name = doctor.Name, 
                Date = doctor.Date.ToString() 
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AddAppoinment(AppoinmnetModel appoinmnet)
        {
            layer.AddAppoinment(appoinmnet);
            return RedirectToAction("AppoinmnetList");
        }

        public IActionResult ApproveAppointment(string id, string name)
        {
            List<AppoinmnetModel> appoinmnet = layer.MarkAppointmentAsApproved(id, name).ToList();

            return View(appoinmnet);
        }
        [HttpPost]
        public IActionResult ApproveAppointment(string id,bool isSelect)
        {
            layer.UpdateAllAppointmentStatus(id, isSelect);
            return RedirectToAction("AppoinmentLists", "Doctor" , new {area = "Doctor" });
        }
        [HttpGet]
        public IActionResult GetDoneStatus(string appointmentId)
        {
            bool isDone = layer.GetIsDoneStatus(appointmentId);
            return Json(isDone);
        }

        [HttpPost]
        public IActionResult UpdateDoneStatus(string appointmentId)
        {
                layer.UpdateAppointmentStatus(appointmentId);
            return Ok(); // Return a success response
        }

        public IActionResult DoctorProfileList()
        {
            string id = "1";
            List<DoctorModel> users = new List<DoctorModel>();
            users = layer.GetDoctorProfile(id).ToList();
            return View(users);
        }

        public IActionResult GetAvailableTimes(DateTime Date, string doctorName, string TimeDuration)
        {
            var availableTimes = GetAvailableTimesForDate(Date, doctorName, TimeDuration);

            return Json(new { availableTimes });
        }
        public IActionResult GetAvailableTime(DateTime Date, string doctorName, string TimeDuration)
        {
            var availableTimes = GetAvailableTimesForDate(Date, doctorName, TimeDuration);

            return Json(new { availableTimes });
        }

        private List<string> GetAvailableTimesForDate(DateTime Date, string doctorName, string TimeDuration)
        {
            var availableTimes = new List<string>();
            int t = Convert.ToInt32(TimeDuration);
            var availableSlots = GetAvailableSlotsForDate(Date, doctorName);
            var a =  " Select your slot";
            availableTimes.Add(a.ToString());
            foreach (var slot in availableSlots)
            {
                var currentTime = slot.STime;
                var endTime = slot.ETime;
                while (currentTime < endTime)
                {
                    if(!layer.GetAllAppointments().Any(u => u.Time<=currentTime&& u.etime >currentTime))
                    {
                        availableTimes.Add(currentTime.ToString(@"hh\:mm"));
                    }
                    currentTime = currentTime.Add(TimeSpan.FromMinutes(t));
                }
            }
            return availableTimes;
        }
        private List<available> GetAvailableSlotsForDate(DateTime date, string doctorName)
        {
            string connection = "Data Source=HP-JAY;database=HospitalDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

            var availableSlots = new List<available>();

            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                using (var command = new SqlCommand("SELECT Start_Time, End_Time FROM Available_tbl WHERE Date = @Date AND Name = @Name", conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Name", doctorName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var stime = (TimeSpan)reader["Start_Time"];
                            var etime = (TimeSpan)reader["End_Time"];

                            availableSlots.Add(new available { Date = date, STime = stime, ETime = etime });
                        }
                    }
                }
            }
            return availableSlots;
        }

        public IActionResult CheckAppointmentOverlap(TimeSpan Stime)
        {
            try
            {
                var existingAppointments = layer.GetAllAppointments().Any(u => u.Time == Stime);

                return Json(new { overlap = existingAppointments });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while checking appointment overlap: {ex}");

                return StatusCode(500, "An error occurred while checking appointment overlap.");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "User", new { area = "User" });
        }

    }
}
