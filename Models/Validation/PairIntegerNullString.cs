using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Validation
{
    public class Pair_RequiredIntegerNullString : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) 
            {
                return false;
            }
            var v = value as IntegerNullString;
            if (v == null) 
            {
                return false;
            }
            else if (v.Id == 0 || v.Id == null) 
            {
                return false;
            }
            return true;
        }
    }

    public class Pair_NullIntegerNullString : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            var v = value as IntegerNullString;
            if (v == null)
            {
                return false;
            }
            else if (v.Id == null)
            {
                return false;
            }
            return true;
        }
    }

    public class Pair_IntegerString : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            var v = value as IntegerNullString;
            if (v == null)
            {
                return false;
            }
            else if (v.Id == null || v.Id == 0)
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(v.Text) == true) 
            {
                return false;
            }
            return true;
        }
    }
}
