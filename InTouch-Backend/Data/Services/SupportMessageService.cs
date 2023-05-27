using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;

namespace InTouch_Backend.Data.Services
{
    public class SupportMessageService
    {
        private readonly AppDbContext _context;

        public SupportMessageService(AppDbContext context)
        {
            _context = context;
        }

        public List<SupportMessages> getSupportMessages()
        {
            List<SupportMessages> supportMessages = _context.SupportMessages.ToList();
            supportMessages.Reverse();
            return supportMessages;
        }

        public void deleteSupportMessage(DeleteSupportMessagesDTO supportMessage)
        {
            var _supportMessage = _context.SupportMessages.FirstOrDefault(r => r.Id == supportMessage.Id);
            if (_supportMessage == null)
            {
                throw new Exception("Message does not exist.");
            }
            _context.SupportMessages.Remove(_supportMessage);
            _context.SaveChanges();
        }
    }
}
