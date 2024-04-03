
using System.ComponentModel.DataAnnotations;


namespace Hospital_Management.Areas.Doctor.Models
{
    public class available
    {
        public int User_Id { get; set; }

        public string Name { get; set;}

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Please select a future date.")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan STime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan ETime { get; set; }

    }

    // Custom validation attribute to ensure future dates
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dt = (DateTime)value;
            return dt.Date >= DateTime.Now.Date;
        }
    }
}




