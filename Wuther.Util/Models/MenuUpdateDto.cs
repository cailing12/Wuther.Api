using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wuther.Util.Enums;

namespace Wuther.Util.Models
{
    public class MenuUpdateDto: IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Icon) || !Icon.Contains("&"))
            {
                yield return new ValidationResult("图标不符合规则", new[] { nameof(Icon) });
            }
        }
    }
}
