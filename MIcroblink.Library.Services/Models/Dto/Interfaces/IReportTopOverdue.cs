namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    /// <summary>
    /// Top users by total overdue
    /// </summary>
    public interface IReportTopOverdue
    {          
        int UserId { get; set; }        
        string FullName { get; set; }        
        int OverdueInDays { get; set; }
    }
}
