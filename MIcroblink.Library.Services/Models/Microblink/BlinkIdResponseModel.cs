using System;

namespace Microblink.Library.Services.Models.Microblink
{
    public class BlinkIdResponseModel
    {
        public string ExecutionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public Result Result { get; set; }
        public bool SuccessfullyParsed 
        {
            get 
            {
                return !string.IsNullOrEmpty(Result.FirstName) 
                    && !string.IsNullOrEmpty(Result.LastName) 
                    && Result.DateOfBirth.SuccessfullyParsed;
            }
        }
    }

    public class Result
    {
        public DateOfBirth DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public MrzData MrzData { get; set; }
        
    }

    public class DateOfBirth 
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool SuccessfullyParsed { get; set; }
        public DateTime? Date
        {
            get
            {
                if (!SuccessfullyParsed)
                    return null;
                
                return new DateTime(Year, Month, Day);
            }
        }
    }

    public class MrzData
    {
        public string RawMrzString { get; set; }
    }
}
