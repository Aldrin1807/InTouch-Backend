using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Services
{
    public class ReportsService
    {
        private readonly AppDbContext _context;

        public ReportsService(AppDbContext context)
        {
            _context = context; 
        }

        public async Task makeReport(ReportsDTO report)
        {
            var _report = new Reports()
            {
                UserId = report.UserId,
                PostId = report.PostId
            };
          await  _context.Reports.AddAsync(_report);
          await  _context.SaveChangesAsync();
        }

        public async Task<List<Reports>> getReports()
        {
            List<Reports> reports =await _context.Reports.ToListAsync();
            reports.Reverse();
            return reports;
        }

        public async Task deleteReport(ReportsDTO report)
        {
            var _report =await _context.Reports.FirstOrDefaultAsync(r => r.UserId == report.UserId && r.PostId == report.PostId);
            if(_report == null ) {
                throw new Exception("Report does not exist.");
            }
            _context.Reports.Remove(_report);
           await _context.SaveChangesAsync();
        }

    }
}
