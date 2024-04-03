namespace Hospital_Management.Areas.Doctor.Models
{
    public class DoctorModel
    {
        public int User_Id { get; set; }
        public string Name { get; set; }
        
        public string Designation { get; set; }

        public string Phone { get; set; }

        public string Gender { get; set; }
        public TimeSpan Stime { get; set; }
        public TimeSpan Etime { get; set; }
        public DateTime Date { get; set; }

    }
}
