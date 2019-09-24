using SampleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApi.Validators
{
    public class DateOrderValidator : IValidatable<Training>
    {
        private DateTime startDate;
        private DateTime endDate;
        private const string INVALIDDATE = "InvalidDate";
        public DateOrderValidator(Training training)
        {
            this.startDate = training.StartDate;
            this.endDate = training.EndDate;
        }

        public string ErrorMessage { get; set; }

        public bool IsValid()
        {
            if (startDate > endDate)
            {
                ErrorMessage = "Start date should come before End Date";
                return false;
            }
                
            return true;
        }
    }
}
