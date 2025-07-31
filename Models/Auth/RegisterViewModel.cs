namespace ArticleManagement.Web.Models.Auth;

using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
  [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
  [Display(Name = "Usuario")]
  public string Username { get; set; } = string.Empty;

  [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
  [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
  [Display(Name = "Correo electrónico")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "La contraseña es obligatoria.")]
  [DataType(DataType.Password)]
  [Display(Name = "Contraseña")]
  public string Password { get; set; } = string.Empty;

  [Required(ErrorMessage = "Debe confirmar la contraseña.")]
  [DataType(DataType.Password)]
  [Display(Name = "Confirmar contraseña")]
  [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
  public string ConfirmPassword { get; set; } = string.Empty;
}
