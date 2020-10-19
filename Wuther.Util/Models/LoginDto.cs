using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Wuther.Util.Models
{
    public class LoginDto : IValidatableObject
    {
        [Display(Name ="账号")]
        //[Required(ErrorMessage = "{0}为必填项目")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "{0}的长度只能是{2}位到{1}位")]
        public string Account { get; set; }

        [Display(Name = "密码")]
        //[Required(ErrorMessage = "{0}为必填项目")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "{0}的长度只能是{2}位到{1}位")]
        public string Password { get; set; }

        [Display(Name = "验证码")]
        //[Required(ErrorMessage = "{0}为必填项目")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "{0}的长度只能是4位")]
        public string VerifyCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //TODO 正则表达式验证用户名密码是否合法
            if (Account.Contains("'"))
            {
                yield return new ValidationResult("账号验证不合法", new[] { nameof(Account) });
            }
            if (Password.Contains("'"))
            {
                yield return new ValidationResult("密码验证不合法", new[] { nameof(Account) });
            }
        }
    }
}
