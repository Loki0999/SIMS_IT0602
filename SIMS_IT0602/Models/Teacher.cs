using System;
namespace SIMS_IT0602.Models
{
	public class Teacher
	{
		public int Id { get; set; }
		public String Name { get; set; }
		public DateOnly DoB { get; set; }
		public string Major { get; set; }
		public Teacher()
		{
		}
	}
}

