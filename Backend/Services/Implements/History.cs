using AuthenticationModule.Helper;
using AuthenticationModule.Models;

namespace AuthenticationModule.Services.Implements
{
    public class History : IHistory
    {
        private readonly LoginPermissionContext _context;
        private readonly IPHelper _ipHelper;

        public History(
            LoginPermissionContext context,
            IPHelper ipHelper)
        {
            _context = context;
            _ipHelper = ipHelper;
        }

        public async Task SaveLoginHistory(Guid? userId, bool isSuccess)
        {
            var history = new LoginHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId ?? Guid.Empty,
                LoginAt = DateTime.Now,
                IpAddress = _ipHelper.GetIpAddress(),
                UserAgent = _ipHelper.GetUserAgent(),
                IsSuccess = isSuccess
            };

            _context.LoginHistories.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}
