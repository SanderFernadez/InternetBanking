
using System.ComponentModel.DataAnnotations;


namespace InternetBanking.Core.Application.ViewModels.Users
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el nombre del usuario")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar el apellido del usuario")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden")]
        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar el documento de identidad")]
        [DataType(DataType.Text)]
        public string Cedula { get; set; }



        [Required(ErrorMessage = "Debe colocar el numero de telefono")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Por favor, selecciona un archivo.")]
        [DataType(DataType.Upload)]

        public List<string> Roles { get; set; }
        public decimal? InitialAmount { get; set; }
        public bool HasError { get; set; }
        public bool IsVerified { get; set; }

        public string? Error { get; set; }

      
    }
}
