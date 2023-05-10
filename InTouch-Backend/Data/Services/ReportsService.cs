using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;

namespace InTouch_Backend.Data.Services
{
    public class ReportsService
    {
        private readonly AppDbContext _context;

        public ReportsService(AppDbContext context)
        {
            _context = context; 
        }

        public void makeReport(ReportsDTO report)
        {
            var _report = new Reports()
            {
                UserId = report.UserId,
                PostId = report.PostId
            };
            _context.Reports.Add(_report);
            _context.SaveChanges();
        }

    }
}
