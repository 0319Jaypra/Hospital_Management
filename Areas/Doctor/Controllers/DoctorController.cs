using Hospital_Management.Areas.Doctor.Data;
using Hospital_Management.Areas.Doctor.Models;
using Hospital_Management.Areas.Patient.Models;
using Hospital_Management.Areas.User.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace Hospital_Management.Areas.Doctor.Controllers
{
    public class DoctorController : Controller
    {
        layer layer = new layer();

        public IActionResult AppoinmentLists()
        {
            string id = HttpContext.Session.GetString("name");
            IEnumerable<AppoinmnetModel> appoinmnets = new List<AppoinmnetModel>();
            appoinmnets = layer.GetAppointments(id).ToList();
            return View(appoinmnets);
            }
        public IActionResult DoctorProfileList()
        {

            List<DoctorModel> users = new List<DoctorModel>();
            users = layer.GetDoctorProfile().ToList();
            return View(users);
        }
        

        [HttpGet]
        public IActionResult DoctorProfile()
        {
            return View();            
        }

        [HttpPost]
        public IActionResult DoctorProfile([Bind] DoctorModel doctor)
        {
            layer.DoctorProfile(doctor, HttpContext);
            return RedirectToAction("DoctorProfileList");
        }

        [HttpGet]
        public IActionResult EditDoctorProfile(int id)
        {
            DoctorModel doctor = layer.GetDoctorProfileById(id);
            return View(doctor);
            
        }

        [HttpPost]
        public IActionResult EditDoctorProfile(int id,[Bind] DoctorModel doctor)
        {
            layer.DoctorProfile(doctor, HttpContext);
            return RedirectToAction("DoctorProfileList");
        }

        [HttpGet]
        public IActionResult DoctorAvailableProfile()
        {
            var id = HttpContext.Session.GetString("userid");
            available doctor = layer.GetDoctorAvailableProfileById(id);
            return View(doctor);

        }

        [HttpPost]
        public IActionResult DoctorAvailableProfile(int id, [Bind] available doctor)
        {
            layer.DoctorAvailableProfile(doctor, HttpContext);
            return RedirectToAction("DoctorAvailableProfileList");
        }

        public IActionResult DoctorAvailableProfileList()
        {
            var id = HttpContext.Session.GetString("userid");
            List<available> users = new List<available>();

            users = layer.GetDoctorAvailableProfile(id).ToList();
            return View(users);
        }

        public IActionResult AppoinmentList()
        {
            List<AppoinmnetModel> appoinmnet = new List<AppoinmnetModel>();
            appoinmnet= layer.GetAppointments().ToList();
            return View(appoinmnet);
        }



        [HttpGet]
        public IActionResult resetpassword()
        {
            string id = HttpContext.Session.GetString("userid");
            var model = new resetpasswordviewmodel()
            {
                UserId = id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult resetpassword([Bind]resetpasswordviewmodel user) 
        {
            layer.resetpassword(user);
            return RedirectToAction("Login", "User", new { area = "User" });
        }


        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "User", new { area = "User" });
        }

        
        public IActionResult DeleteDoctorAvailability(TimeSpan Stime)
        {
            layer.DeleteDoctorAvailabilityByUserId(Stime);

            return RedirectToAction("DoctorAvailableProfileList");
        }

    }
}
