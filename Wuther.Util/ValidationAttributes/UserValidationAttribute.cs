using System.ComponentModel.DataAnnotations;
using Wuther.Util.Models;

namespace Wuther.Util.ValidationAttributes
{
    public class UserValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = (UserAddDto)validationContext.ObjectInstance;
            if (addDto.Username == addDto.Password)
            {
                return new ValidationResult("用户名不能跟密码相同", new[] { nameof(ValidationAttribute) });
            }
            else {
                return ValidationResult.Success;
            }
        }

    }
}
