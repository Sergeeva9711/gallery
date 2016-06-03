using System.ComponentModel.DataAnnotations;

namespace Evacuation.Web.Models
{
    public class UserModel
    {
        public int UserID { get; set; }        
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public int Age { get; set; }
        
        public string Email { get; set; }
        
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsDeleted { get; set; }
    }
}