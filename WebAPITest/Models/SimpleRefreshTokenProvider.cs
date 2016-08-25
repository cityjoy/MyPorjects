using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPITest.Models;
using WebAPITest.Models.Entities;

namespace WebAPITest.Models
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (AuthRepository _repo = new AuthRepository())
            {

                var token = new RefreshToken()
                {
                        Id = refreshTokenId.GetHashCode().ToString(),
                        Subject = context.Ticket.Identity.Name,
                        IssuedUtc = DateTime.UtcNow,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            string hashedTokenId = context.Token.GetHashCode().ToString();

            using (AuthRepository _repo = new AuthRepository())
            {
                var refreshToken = await _repo.FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await _repo.RemoveRefreshToken(hashedTokenId);
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
 
    }
}
