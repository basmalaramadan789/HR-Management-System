using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{ 

    public class CreatePayrollDTO
    {
        [Required(ErrorMessage = "Employee is required")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Gross amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Gross amount must be a positive number")]
        [Display(Name = "Gross Amount")]
        [DataType(DataType.Currency)]
        public decimal GrossAmount { get; set; }

        [Required(ErrorMessage = "Tax amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Tax amount must be a positive number")]
        [Display(Name = "Tax Amount")]
        [DataType(DataType.Currency)]
        public decimal TaxAmount { get; set; }

        [Required(ErrorMessage = "Net amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Net amount must be a positive number")]
        [Display(Name = "Net Amount")]
        [DataType(DataType.Currency)]
        public decimal NetAmount { get; set; }
    }
}

