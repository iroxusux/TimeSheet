namespace TimeSheet.Models
{
    public class User
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string CellNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string AccessibleName { get; } = "User";
    }
}
