using Assignment2_FrontEnd.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment2_FrontEnd.Custom_Validation
{
    public class AgeValidation : ValidationAttribute
    {
       
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var student = (StudentVm)validationContext.ObjectInstance;

            if (student.Age != null)
            {
                var age = DateTime.Today.Year - student.Age;

                return (age >= 18)
                    ? ValidationResult.Success
                    : new ValidationResult("Student should be at least 18 years old.");
            }

            return new ValidationResult("Date of Birth is required.");
        }
    }
}
