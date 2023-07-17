using System.Data;

namespace YourPetAPI.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        //public string City { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }

      


    }
}
