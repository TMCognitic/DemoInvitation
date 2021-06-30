using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoInvitation.Infrastructure.Security
{
    public class TokenService : ITokenRepository
    {
        private const string PassPhrase = "MaSuperPassPhrase"; //<-- à changer bien entendu
        private const string Prefixe = "Bearer ";

        private JwtSecurityTokenHandler _handler;
        private JwtHeader _header;

        private JwtSecurityTokenHandler Handler
        {
            get
            {
                return _handler ??= new JwtSecurityTokenHandler();
            }
        }
        private JwtHeader Header
        {
            get
            {
                return _header ??= new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PassPhrase)),
                        SecurityAlgorithms.HmacSha512));
            }
        }

        public string GenerateToken(TokenUser user)
        {
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                header: Header,
                payload: new JwtPayload(
                    issuer: null,
                    audience: null,
                    claims: new Claim[]
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("LastName", user.LastName),
                        new Claim("FirstName", user.FirstName),
                        new Claim("Email", user.Email),
                        new Claim("IsAdmin", user.IsAdmin.ToString()),
                    },
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.Date.AddDays(15))
                );
            return $"{Prefixe}{Handler.WriteToken(jwtSecurityToken)}";
        }

        public TokenUser ValidateToken(string token)
        {
            TokenUser user = null;

            token = token.Replace(Prefixe, "");
            JwtSecurityToken jwtSecurityToken = Handler.ReadJwtToken(token);
            DateTime now = DateTime.Now;
            if (jwtSecurityToken.ValidFrom <= now && jwtSecurityToken.ValidTo >= now)
            {
                JwtPayload payload = jwtSecurityToken.Payload;
                string compareToken = Handler.WriteToken(new JwtSecurityToken(Header, payload));

                if (token == compareToken)
                {
                    payload.TryGetValue("Id", out object id);
                    payload.TryGetValue("LastName", out object lastname);
                    payload.TryGetValue("FirstName", out object firstname);
                    payload.TryGetValue("Email", out object email);
                    payload.TryGetValue("IsAdmin", out object isAdmin);

                    user = new TokenUser()
                    {
                        Id = int.Parse((string)id),
                        LastName = (string)lastname,
                        FirstName = (string)firstname,
                        Email = (string)email,
                        IsAdmin = bool.Parse((string)isAdmin)
                    };
                }
            }

            return user;
        }
    }
}
