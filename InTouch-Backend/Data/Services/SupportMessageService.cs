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

        public void sendSupportMessage(SupportMessagesDTO messageDTO)
        {
            var _user = _context.Users.FirstOrDefault(u => u.Email == messageDTO.UsernameOrEmail || u.Username == messageDTO.UsernameOrEmail);
            if (_user == null)
            {
                throw new Exception("This user is not registered");
            }
            if (!_user.isLocked)
            {
                throw new Exception("This user's account is not locked");
            }
            SupportMessages _message = new SupportMessages()
            {
                UserId = _user.Id,
                message = messageDTO.message
            };
            _context.SupportMessages.Add(_message);
            _context.SaveChanges();

        }
    }
}
