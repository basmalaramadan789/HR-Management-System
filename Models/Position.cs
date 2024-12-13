namespace WebApplication1.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal BaseSalary { get; set; }

        // Navigation property
        public ICollection<Employee> Employees { get; set; }
    }
}
