using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Developer.Schema
{
    public class Controller
    {
        // <JsonIgnore>
        [Key]
        public int? CId { get; set; } = 0;
        [Required(ErrorMessage = "Controller name is required!")]
        [StringLength(100, ErrorMessage = "Controller name must be between 4 and 100 characters.", MinimumLength = 4)]
        public string ControllerName { get; set; } = "";
        public Common.IntegerNullString ParentController { get; set; } = new Common.IntegerNullString();
        [Required(ErrorMessage = "Controller type is required!")]
        public IntegerString ControllerType { get; set; } = new IntegerString();
        public bool IsVisible { get; set; } = true;
        public bool NeedAuthorisation { get; set; } = true;
        public bool NeedLogin { get; set; } = true;
        public bool NeedParentAuthorisation { get; set; } = false;
        public bool Status { get; set; } = true;
        public Common.IntegerNullString LoginType { get; set; } = new Common.IntegerNullString();
        public int? Ordinal { get; set; } = 0;
        public string DisplayText { get; set; } = "";
        public string URL { get; set; } = "";
        public string SqlCommand { get; set; } = "";
        public string SqlCommandKeys { get; set; } = "";
        public string EditController { get; set; } = "";
        public string CreateController { get; set; } = "";
        public string SqlCommandOrderBy { get; set; } = "";
        public string ControlController { get; set; } = "";
        public string ChildControllers { get; set; } = "";
    }
}
