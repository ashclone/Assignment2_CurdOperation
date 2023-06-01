namespace Assignment2_CurdOperation.Modals
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public string Bio { get; set; }
        public ICollection<StudentHobby> StudentHobby { get; set; }=new HashSet<StudentHobby>();
    }


}
