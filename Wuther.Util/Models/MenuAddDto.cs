using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wuther.Util.Enums;

namespace Wuther.Util.Models
{
    public class MenuAddDto: IValidatableObject
    {
        [Display(Name = "名称")]
        [Required(ErrorMessage = "{0}为必填项目")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0}的长度只能是{2}位到{1}位")]
        public string Name { get; set; }
        [Display(Name = "位置")]
        [Required(ErrorMessage = "{0}为必填项目")]
        public MenuPosition? Position { get; set; }
        public int? ParentId { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public int Seqno { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //MenuType menuType;
            //if (Enum.TryParse(Type.ToString(),out menuType))
            //{
            //    yield return new ValidationResult("账号和用户名不能相同", new[] { nameof(Type)});
            //}

            if (string.IsNullOrWhiteSpace(Icon) || !Icon.Contains("&"))
            {
                yield return new ValidationResult("图标不符合规则", new[] { nameof(Icon) });
            }
        }
    }
}
