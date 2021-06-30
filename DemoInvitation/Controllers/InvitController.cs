using DemoInvitation.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoInvitation.Controllers
{
    public class InvitController : Controller
    {
        private readonly ITokenRepository _tokenRepository;

        public InvitController(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        //-----------------------------------------------------------------
        //-----------------------------------------------------------------
        // ATTENTION DE BIEN CHANGER LA PASSPHRASE DANS LE TOKEN SERVICE
        //-----------------------------------------------------------------
        //-----------------------------------------------------------------

        //Affiche les données du Token
        public IActionResult Index(string token)
        {
            TokenUser user = _tokenRepository.ValidateToken(token);

            if (user is null)
                return RedirectToAction("Error", "Home");

            return View(user);
        }        
    }
}
