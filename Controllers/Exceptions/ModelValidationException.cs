using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Exceptions
{
    public class ModelValidationException: Exception
    {
        public ModelValidationException(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState)
        {
            _ModelState = ModelState;
        }

        private Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary _ModelState;

        public Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState
        {
            get
            {
                return _ModelState;
            }

            set
            {
                _ModelState = value;
            }
        }
    }
}
