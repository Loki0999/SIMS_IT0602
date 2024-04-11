namespace SIMS_IT0602.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly DoB { get; set; }
        public string Address { get; set; }
        public string Major { get; set; }
        public Student() 
        { 
        }
    }
}
