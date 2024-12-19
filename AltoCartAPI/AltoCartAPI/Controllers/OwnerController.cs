using AltoCartAPI.Helpers;
using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AltoCartAPI.Controllers
{
    [RoutePrefix("api/OwnerPREfunc_wKlWeRiIoSSmN")]
    public class OwnerController : ApiController
    {

        AltoCartDB db;

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

        [Route("create_owner")]
        public async Task<IHttpActionResult> CreateOwner([FromBody] TempOwner tempOwner)
        {

            if (tempOwner == null)
                return Content(HttpStatusCode.BadRequest, "Complete all fields");

            if (string.IsNullOrWhiteSpace(tempOwner.OwnerName) || string.IsNullOrWhiteSpace(tempOwner.OwnerSurname) || string.IsNullOrWhiteSpace(tempOwner.OwnerPhoneNumber) || string.IsNullOrWhiteSpace(tempOwner.OwnerEmail) || string.IsNullOrWhiteSpace(tempOwner.Password) || tempOwner.BirthDate == null || tempOwner.City_ID == -1 || tempOwner.Country_ID == -1)
                return Content(HttpStatusCode.BadRequest, "Complete all fiedls");

            try
            {

                using (db = new AltoCartDB())
                {
                    if (await db.Owners.AnyAsync(x => x.OwnerEmail == tempOwner.OwnerEmail))
                        return Content(HttpStatusCode.BadRequest, "This email already has a account");

                    Owner owner = new Owner()
                    {
                        Guid_ID = Guid.NewGuid(),
                        OwnerEmail = tempOwner.OwnerEmail,
                        OwnerName = tempOwner.OwnerName,
                        OwnerSurname = tempOwner.OwnerSurname,
                        City_ID = tempOwner.City_ID,
                        OwnersCity = db.Cities.FirstOrDefault(x => x.ID == tempOwner.City_ID).CityName,
                        Country_ID = tempOwner.Country_ID,
                        OwnersCountry = db.Countries.FirstOrDefault(x => x.ID == tempOwner.Country_ID).CountryName,
                        BirthDate = tempOwner.BirthDate,
                        IsActive = true,
                        IsDeleted = false,
                        OwnerPhoneNumber = tempOwner.OwnerPhoneNumber,
                        Shop_ID = null,
                        Shop = null,
                        OwnersShopName = null,
                        Cities = db.Cities.Find(tempOwner.City_ID),
                        Countries = db.Countries.Find(tempOwner.Country_ID),
                        CreatedAt = DateTime.UtcNow,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempOwner.Password)
                    };

                    var generatedAccTkn = JwtHelper.GenerateAccessTokenForOwner(owner);
                    var generatedRefreshTkn = JwtHelper.GenerateRefreshTokenForOwner(owner);

                    db.Owners.Add(owner);
                    await db.SaveChangesAsync();

                    return Content(HttpStatusCode.OK, new
                    {
                        Message = "Created successfully",
                        RefreshToken = generatedRefreshTkn,
                        AccessToken = generatedAccTkn
                    });

                }

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired, {ex}");
            }
        }

        [Route("login_owner")]
        public async Task<IHttpActionResult> LoginOwner([FromBody] TempOwner tempOwner)
        {

            if (tempOwner == null)
                return Content(HttpStatusCode.BadRequest, $"Complete {nameof(tempOwner)} fields");
            //throw new ArgumentNullException(nameof(tempOwner))

            if (string.IsNullOrWhiteSpace(tempOwner.OwnerEmail) || string.IsNullOrWhiteSpace(tempOwner.Password))
                return Content(HttpStatusCode.BadRequest, $"Please complete all sections");

            try
            {
                using (db = new AltoCartDB())
                {

                    if (!(await db.Owners.AnyAsync(x => x.OwnerEmail == tempOwner.OwnerEmail)))
                        return Content(HttpStatusCode.BadRequest, "Please check your email");

                    Owner owner = await db.Owners.FirstOrDefaultAsync(x => x.OwnerEmail == tempOwner.OwnerEmail);

                    if (owner.IsDeleted)
                        return Content(HttpStatusCode.BadRequest, "Account is deleted");

                    if (!owner.IsActive)
                        return Content(HttpStatusCode.Forbidden, "Your account is suspended");

                    if (!BCrypt.Net.BCrypt.Verify(tempOwner.Password, owner.PasswordHash))
                        return Content(HttpStatusCode.Forbidden, "Please check your password");

                    var refreshToken = JwtHelper.ValidateRefreshTokenForOwner(owner.Guid_ID);

                    var authHeader = Request.Headers.Authorization;
                    if (authHeader == null || authHeader.Scheme != "Bearer")
                    {
                        var accToken = JwtHelper.GenerateAccessTokenForOwner(owner);
                        return Content(HttpStatusCode.OK, new
                        {
                            Message = "Login successfull",
                            AccessToken = accToken,
                            RefreshToken = refreshToken
                        });
                    }

                    var accessTokenParam = authHeader.Parameter;

                    var accessToken = JwtHelper.ValidateAccessToken(accessTokenParam);

                    return Content(HttpStatusCode.OK, new
                    {
                        Message = "Login successfull",
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    });

                }
            }
            catch (DbException dbEx)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired (DbException). {dbEx.InnerException.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired (Exception). {ex.InnerException.Message ?? ex.Message}");
            }

        }

        //[HttpPost]
        //public async Task<IHttpActionResult> AddShop([FromBody] TempShop tempShop)
        //{
        //    Guid ownerGuid = CheckAccessToken();
        //    if (ownerGuid == null)
        //        return Content(HttpStatusCode.Forbidden, "Guid id is null");

        //    using (db = new AltoCartDB())
        //    {
        //        try
        //        {
        //            Owner owner = await db.Owners.FirstOrDefaultAsync(x => x.Guid_ID == ownerGuid);
        //            if (owner == null)
        //                return Content(HttpStatusCode.Forbidden, "Guid id is not found in Owner Table");

        //            if (owner.IsDeleted)
        //                return Content(HttpStatusCode.Forbidden, "Not Found Account");

        //            if (!owner.IsActive)
        //                return Content(HttpStatusCode.Forbidden, "Your account is suspanded");

                    

        //        }
        //        catch
        //        {

        //        }
        //    }


        //}

        public Guid CheckAccessToken()
        {
            #region OLD

            //var requestedTokenHeader = Request.Headers.Authorization;
            //if (requestedTokenHeader == null || requestedTokenHeader.Scheme != "Bearer")
            //    throw new SecurityTokenException("Access token is missing or invalid");

            //string accessTokenParam = requestedTokenHeader.Parameter;
            //var claimsPrincipal = JwtHelper.ValidateAccessTokenForOwner(accessTokenParam);

            //var claim = (ClaimsIdentity)claimsPrincipal.Identity;

            //if (claim == null)
            //    throw new Exception("Access token's claims are null");

            //string nameIdenifier = claim.FindFirst(ClaimTypes.NameIdentifier).Value;

            //if (nameIdenifier == null)
            //    throw new Exception("Guid ID is null");

            //return nameIdenifier;

            #endregion

            AuthenticationHeaderValue requestedHeaderAuth = Request.Headers.Authorization;
            if (requestedHeaderAuth == null || requestedHeaderAuth.Scheme != "Bearer")
                throw new AuthenticationException("Token is missing or invalid");

            string tokenParameter = requestedHeaderAuth.Parameter;
            if (tokenParameter == null)
                throw new Exception("Token parameters are null");

            ClaimsPrincipal claimsPrincipal = JwtHelper.ValidateAccessTokenForOwner(tokenParameter);

            if (claimsPrincipal == null)
                throw new Exception("Claims principals are null");

            ClaimsIdentity claim = (ClaimsIdentity)claimsPrincipal.Identity;

            Guid guidID = Guid.Parse(claim.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (guidID == Guid.Empty)
                throw new ArgumentNullException("Guid id is empty");

            return guidID;
        }

    }
}
