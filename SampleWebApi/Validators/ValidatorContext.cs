using SampleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi.Validators
{
    public class ValidatorContext
    {
        private List<IValidatable<Training>> validators;

        public ValidatorContext()
        {
            validators = new List<IValidatable<Training>>();
        }

        public string ErrorMessage { get; set; }

        public bool ContainsError()
        {
            foreach (var item in validators)
            {
                if (!item.IsValid())
                {
                    ErrorMessage = item.ErrorMessage;
                    return true;
                }
            }
            return false;
        }
        public void AddValidators(IValidatable<Training> validator)
        {
            validators.Add(validator);
        }

        public void AddValidators(List<IValidatable<Training>> validators)
        {
            this.validators.AddRange(validators);
        }
    }
}
