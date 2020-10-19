using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wuther.Util.Enums;

namespace Wuther.Util.Models
{
    public class UserUpdateDto
    {
        [Display(Name = "账号")]
        [Required(ErrorMessage = "{0}为必填项目")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0}的长度只能是{2}位到{1}位")]
        //[MaxLength(20,ErrorMessage ="{0}的最大长度不可以超过{1}")]
        public string Account { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Gender? Sex { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Department { get; set; }
        [Phone]
        public string Phone { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Account == Username)
            {
                //yield return new ValidationResult("账号和用户名不能相同",new[] { nameof(UserAddDto) });
                yield return new ValidationResult("账号和用户名不能相同", new[] { nameof(Account), nameof(Username) });
            }
        }
    }
}
