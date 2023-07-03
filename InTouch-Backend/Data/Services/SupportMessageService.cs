using InTouch_Backend.Data.DTOs;
using InTouch_Backend.Data.Models;
using InTouch_Backend.Data.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InTouch_Backend.Data.Services
{
    public class SupportMessageService
    {
        private readonly AppDbContext _context;

        public SupportMessageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SupportMessages>> getSupportMessages()
        {
            List<SupportMessages> supportMessages =await _context.SupportMessages.ToListAsync();
            supportMessages.Reverse();
            return supportMessages;
        }

        public async Task deleteSupportMessage(DeleteSupportMessagesDTO supportMessage)
        {
            var _supportMessage =await _context.SupportMessages.FirstOrDefaultAsync(r => r.Id == supportMessage.Id);
            if (_supportMessage == null)
            {
                throw new Exception("Message does not exist.");
            }
            _context.SupportMessages.Remove(_supportMessage);
           await _context.SaveChangesAsync();
        }

        public async Task sendSupportMessage(SupportMessagesDTO messageDTO)
        {
            var _user =await _context.Users.FirstOrDefaultAsync(u => u.Email == messageDTO.UsernameOrEmail || u.Username == messageDTO.UsernameOrEmail);
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
           await _context.SupportMessages.AddAsync(_message);
           await _context.SaveChangesAsync();

        }
    }
}
