using System;


namespace InternetBanking.Core.Application.Dtos.Account
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Cedula { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public string ConfirmPassword { get; set; }
       
    }
}
