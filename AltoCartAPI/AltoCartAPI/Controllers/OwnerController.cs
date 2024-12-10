using AltoCartAPI.Helpers;
using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AltoCartAPI.Controllers
{
    [RoutePrefix("api/OwnerPREfunc_wKlWeRiIoSSmN")]
    public class OwnerController : ApiController
    {

        #region En Eskiler

        /*
         AltoCartDB db = new AltoCartDB();

        [HttpPost]
        [Route("O_RegisterFunc_JuLiIwMnQ")]
        public async Task<IHttpActionResult> OwnerRegister([FromBody] TempOwner ownerInformations)
        {
            if (ownerInformations == null)
                return BadRequest("Please fill all fields");

            if (string.IsNullOrEmpty(ownerInformations.OwnerName))
                return BadRequest("Please enter your name");

            if (string.IsNullOrEmpty(ownerInformations.OwnerSurname))
                return BadRequest("Please enter your surname");

            if (string.IsNullOrEmpty(ownerInformations.OwnerPhoneNumber))
                return BadRequest("Please enter your phone number");

            if (string.IsNullOrEmpty(ownerInformations.OwnerEmail))
                return BadRequest("Please enter your email");

            if (ownerInformations.BirthDate == null)
                return BadRequest("Please enter your birth date");

            if (await db.Owners.AnyAsync(x => x.OwnerEmail == ownerInformations.OwnerEmail))
            {
                return BadRequest("This email has already an account");
            }

            Owner owner = new Owner
            {
                Guid_ID = Guid.NewGuid(),
                Shop_ID = null,
                OwnerEmail = ownerInformations.OwnerEmail,
                OwnerName = ownerInformations.OwnerName,
                OwnerSurname = ownerInformations.OwnerSurname,
                OwnerPhoneNumber = ownerInformations.OwnerPhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(ownerInformations.Password),
                BirthDate = ownerInformations.BirthDate,
                Country_ID = ownerInformations.Country_ID,
                City_ID = ownerInformations.City_ID,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsActive = true
            };

            var refreshToken = JwtHelper.GenerateRefreshToken(owner.Guid_ID);
            var accessToken = JwtHelper.GenerateAccessToken(owner.Guid_ID);

            try
            {
                db.RefreshTokens.Add(refreshToken);
                db.Owners.Add(owner);
                await db.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Successfully created",
                    Owner = new
                    {
                        owner.OwnerName,
                        owner.OwnerSurname,
                        owner.OwnerEmail,
                        owner.OwnerPhoneNumber,
                        owner.BirthDate,
                        owner.CreatedAt
                    }
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                return BadRequest($"Validation error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest($"Database error occurred: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Unexpected error: {ex.Message}"));
            }
        }

        //[HttpPost]
        //[Route("O_LoginFunc_sWjMCsNiIOPXzxAa")]
        //public async Task<IHttpActionResult> OwnerLogin([FromBody] TempOwner ownerInformations)
        //{

        //}




        [HttpPost]
        [Route("O_LoginFunc_sWjMCsNiIOPXzxAa")]
        public async Task<IHttpActionResult> OwnerLogin([FromBody] TempOwner ownerInformations)
        {

            if (!TryGetAccessToken(out var accessToken))
                return Content(HttpStatusCode.Unauthorized, "Access Token is missing or null");

            var tokenClaims = JwtHelper.ValidateToken(accessToken);
            if (tokenClaims == null)
                return Content(HttpStatusCode.Unauthorized, "Token is null");

            string userGuidID = tokenClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userGuidID, out Guid guidID))
                return Content(HttpStatusCode.Unauthorized, "Invalid Guid ID in token");

            var owner = await db.Owners.FirstOrDefaultAsync(x => x.Guid_ID == guidID);
            if (owner == null)
                return Content(HttpStatusCode.Unauthorized, "No account found with provided GUID");

            if (owner.IsDeleted)
                return Content(HttpStatusCode.Unauthorized, "Account does not exist");
            if (!owner.IsActive)
                return Content(HttpStatusCode.Unauthorized, "Account is suspended");

            if (ownerInformations == null || string.IsNullOrWhiteSpace(ownerInformations.OwnerEmail) || string.IsNullOrWhiteSpace(ownerInformations.Password))
                return Content(HttpStatusCode.BadRequest, "Email and password are required");

            if (string.Equals(owner.OwnerEmail, ownerInformations.OwnerEmail))
            {

                if (BCrypt.Net.BCrypt.Verify(ownerInformations.Password, owner.PasswordHash))
                {
                    await UpdateOrCreateRefreshToken(owner.Guid_ID);
                    var newAccessToken = JwtHelper.GenerateAccessToken(owner.Guid_ID);

                    return Ok(new
                    {
                        Message = "Login successfull",
                        AccessToken = newAccessToken
                    });
                }
                else
                    return Content(HttpStatusCode.BadRequest, "Check your password");
            }
            else
                return Content(HttpStatusCode.BadRequest, "Check your email");
        }





        #region Other Functions

        private bool TryGetAccessToken(out string accessToken)
        {

            accessToken = null;
            var requestedAuthHeader = Request.Headers.Authorization;

            if (requestedAuthHeader == null || requestedAuthHeader.Scheme != "Bearer")
                return false;

            accessToken = requestedAuthHeader.Parameter;

            return !string.IsNullOrWhiteSpace(accessToken);

        }

        private async Task UpdateOrCreateRefreshToken(Guid userGuid)
        {
            var refreshToken = await db.RefreshTokens.FirstOrDefaultAsync(x => x.UserGuid == userGuid);
            if (refreshToken != null)
            {
                refreshToken.ExpireDate = DateTime.UtcNow.AddDays(15);
            }
            else
            {
                refreshToken = JwtHelper.GenerateRefreshToken(userGuid);
                db.RefreshTokens.Add(refreshToken);
            }

            await db.SaveChangesAsync();
        }

        #endregion

         */


        #endregion

    }
}
