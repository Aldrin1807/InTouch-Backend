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

        public List<Reports> getReports()
        {
            List<Reports> reports = _context.Reports.ToList();
            reports.Reverse();
            return reports;
        }

        public void deleteReport(ReportsDTO report)
        {
            var _report = _context.Reports.FirstOrDefault(r => r.UserId == report.UserId && r.PostId == report.PostId);
            if(_report == null ) {
                throw new Exception("Report does not exist.");
            }
            _context.Reports.Remove(_report);
            _context.SaveChanges();
        }

    }
}
