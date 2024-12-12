using AltoCartAPI.Helpers;
using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Services.Description;
using System.Web.SessionState;

namespace AltoCartAPI.Controllers
{
    [RoutePrefix("api/SqWeErEemIiOsNbBjjJTT")]
    public class BigPersonController : ApiController
    {

        AltoCartDB db = new AltoCartDB();

        #region En Eskiler

        /*
         AltoCartDB db = new AltoCartDB();
        JwtHelper helpers = new JwtHelper();

        [HttpPost]
        [Route("KSiUnMwWCqGtHYbFiiI")]
        public async Task<IHttpActionResult> BigPersonRegister([FromBody] TempBigPerson personInfos)
        {
            #region Eski kod
            //if (personInfos == null)
            //    return BadRequest("Person information cannot be null.");
            //if (string.IsNullOrWhiteSpace(personInfos.Username))
            //    return BadRequest("Username cannot be null or empty.");
            //if (string.IsNullOrWhiteSpace(personInfos.Email))
            //    return BadRequest("Email cannot be null or empty.");
            //if (string.IsNullOrWhiteSpace(personInfos.Password))
            //    return BadRequest("Password cannot be null or empty.");

            //    var bigPerson = new BigPerson()
            //    {
            //        GuidID = Guid.NewGuid(),
            //        Email = personInfos.Email,
            //        Password = BCrypt.Net.BCrypt.HashPassword(personInfos.Password),
            //        Username = personInfos.Username,
            //        CreatedAt = DateTime.UtcNow,
            //        IsActive = true
            //    };

            //    RefreshToken refreshToken = JwtHelper.GenerateRefreshToken(bigPerson.GuidID);

            //    try
            //    {
            //        db.RefreshTokens.Add(refreshToken);
            //        db.BigPersons.Add(bigPerson);
            //        await db.SaveChangesAsync();
            //        return Ok(new { Message = "Big Person Successfully Added." });
            //    }
            //    catch (DbUpdateException exMessage)
            //    {
            //        return BadRequest($"Error can't added to database: {exMessage.InnerException?.Message ?? exMessage.Message}");
            //    }

            #endregion

            if (personInfos == null)
                return BadRequest("Person information cannot be null.");

            var validationResult = ValidatePersonInfos(personInfos);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.ErrorMessage);

            if (await db.BigPersons.AnyAsync(x => x.Email == personInfos.Email))
            {
                return BadRequest("A person with this email already exists.");
            }

            var bigPerson = new BigPerson()
            {
                GuidID = Guid.NewGuid(),
                Email = personInfos.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(personInfos.Password),
                Username = personInfos.Username,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            RefreshToken refreshToken = JwtHelper.GenerateRefreshToken(bigPerson.GuidID);
            string accessToken = JwtHelper.GenerateAccessToken(bigPerson.GuidID);

            try
            {
                db.RefreshTokens.Add(refreshToken);
                db.BigPersons.Add(bigPerson);
                await db.SaveChangesAsync();

                return Ok(new { Message = $"Big Person Successfully Added. AccessToken {accessToken}", BigPersonID = bigPerson.GuidID });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Unexpected error: {ex.Message}"));
            }

        }



        //[HttpGet]
        //[Route("QnMUuIOswWmnNeQ")]
        //public async Task<IHttpActionResult> BigPersonLogin()
        //{
        //    return Ok();
        //}

        [HttpPost]
        [Route("qNmuUioSWwMNnEq")]
        public async Task<IHttpActionResult> BigPersonLogin([FromBody] TempBigPerson personInfos)
        {

            if (personInfos == null)
                return BadRequest("Fields are null");

            if (string.IsNullOrWhiteSpace(personInfos.Email))
                return BadRequest("Email is required.");

            if (string.IsNullOrWhiteSpace(personInfos.Password))
                return BadRequest("Password is required.");

            try
            {
                var bigPerson = await db.BigPersons
                    .FirstOrDefaultAsync(x => x.Email == personInfos.Email);

                if (bigPerson == null)
                    return BadRequest("No account found with this email. Please check your email address.");

                if (!bigPerson.IsActive)
                    return BadRequest("Your account is suspended.");

                if (!BCrypt.Net.BCrypt.Verify(personInfos.Password, bigPerson.Password))
                    return BadRequest("Incorrect password.");

                var refreshToken = await db.RefreshTokens
                    .FirstOrDefaultAsync(x => x.UserGuid == bigPerson.GuidID);

                if (refreshToken != null)
                {
                    refreshToken.ExpireDate = DateTime.UtcNow.AddDays(15); // Refresh token süresini yenile
                }
                else
                {
                    refreshToken = JwtHelper.GenerateRefreshToken(bigPerson.GuidID);
                    db.RefreshTokens.Add(refreshToken);
                }

                string generatedAccessToken = JwtHelper.GenerateAccessToken(bigPerson.GuidID);

                await db.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Login successful.",
                    RefreshToken = refreshToken.Token,
                    AccessToken = generatedAccessToken
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An unexpected error occurred. Please try again later.", ex));
            }


        }

        private (bool IsValid, string ErrorMessage) ValidatePersonInfos(TempBigPerson personInfos) // iki tane değer dönme işlemi için
        {
            if (string.IsNullOrWhiteSpace(personInfos.Username))
                return (false, "Username cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(personInfos.Email))
                return (false, "Email cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(personInfos.Password))
                return (false, "Password cannot be null or empty.");

            return (true, string.Empty);
        }


        #region Eskiler

        //[HttpPost]
        //[Route("waitingShops_sWjmSjIELi")]
        //public async Task<IHttpActionResult> ShopApproveFunction([FromBody] string shopNumber)
        //{
        //    if (!GetAccessToken(out var accessToken))
        //        return Content(HttpStatusCode.Unauthorized, "Access Token is missing or null");

        //    var tokenClaims = JwtHelper.ValidateToken(accessToken);
        //    if (tokenClaims == null)
        //        return Content(HttpStatusCode.Unauthorized, "Access token is null");

        //    string userStringGuid = tokenClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    if (!Guid.TryParse(userStringGuid, out Guid GuidID)) // eğer olmazsa yani FALSE olursa
        //        return Content(HttpStatusCode.Unauthorized, "Invalid guid ID in token");

        //    var bigPerson = await db.BigPersons.FirstOrDefaultAsync(x => x.GuidID == GuidID);

        //    int shopIDNumber = Convert.ToInt32(shopNumber);

        //    Shop shop = await db.Shops.FirstOrDefaultAsync(x => x.ID == shopIDNumber);

        //    if (shop == null)
        //        return Content(HttpStatusCode.BadRequest, "Shop is null");

        //    if (shop.IsDeleted)
        //        return Content(HttpStatusCode.NotFound, "There is no Shop with this ID");

        //    if(!shop.IsActive)
        //        return Content(HttpStatusCode.Forbidden, "This shop is suspended");

        //    try
        //    {
        //        shop.IsApprovedByBigPerson = true;
        //        shop.ApprovedByWho = bigPerson.Username;
        //        shop.ApprovedDate = DateTime.UtcNow;
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, $"Error occuired {dbEx.InnerException?.Message ?? dbEx.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, $"Error occuired {ex.InnerException?.Message ?? ex.Message}");
        //    }

        //    return null; // BURAYA BAK!!!

        //}

        //private bool GetAccessToken(out string AccessToken)
        //{
        //    AccessToken = null;

        //    var tokenHeader = Request.Headers.Authorization;
        //    if(tokenHeader == null || tokenHeader.Scheme != "Bearer")
        //        return false;

        //    AccessToken = tokenHeader.Parameter;

        //    return string.IsNullOrWhiteSpace(AccessToken);

        //}



        #endregion
         */

        #endregion

        [Route("create_new_big_person")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateBigPerson([FromBody] BigPerson bigPerson)
        {
            if (bigPerson == null)
                return Content(HttpStatusCode.BadRequest, "Parameters can not be null");

            if (string.IsNullOrWhiteSpace(bigPerson.Username) || string.IsNullOrWhiteSpace(bigPerson.Password) || string.IsNullOrWhiteSpace(bigPerson.Username) || string.IsNullOrWhiteSpace(bigPerson.Email))
                return Content(HttpStatusCode.BadRequest, "Please complete all fields");

            if (await db.BigPersons.AnyAsync(x => x.Username == bigPerson.Username))
                return Content(HttpStatusCode.BadRequest, "This username already in use");

            if (await db.BigPersons.AnyAsync(x => x.Email == bigPerson.Email))
                return Content(HttpStatusCode.BadRequest, "This email already has a account");

            try
            {
                bigPerson.Password = BCrypt.Net.BCrypt.HashPassword(bigPerson.Password);
                db.BigPersons.Add(bigPerson);
                await db.SaveChangesAsync();
                var createdRefreshToken = JwtHelper.GenerateRefreshTokenForBigPerson(bigPerson);
                var createdAccessToken = JwtHelper.GenerateAccessTokenForBigPerson(bigPerson);
                //return Content(HttpStatusCode.OK, $"Big Person successfully added to database. Refresh Token => {createdRefreshToken} / Access Token => {createdAccessToken}");
                return Content(HttpStatusCode.OK, new
                {
                    Message = "Successfully Created",
                    CreatedAccessToken = createdAccessToken,
                    CreatedRefreshToken = createdRefreshToken,
                });
            }
            catch (DbUpdateException dbUpdateEx)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired (DbUpdateException). {dbUpdateEx.InnerException.Message ?? dbUpdateEx.Message}");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired (Exception). Message: {ex.Message} - InnerException/Message: {ex.InnerException.Message}");
            }

        }


        [Route("login_big_person")]
        [HttpPost]
        public async Task<IHttpActionResult> LoginBigPerson([FromBody] TempBigPerson tempBigPerson)
        {
            if (tempBigPerson == null)
                return Content(HttpStatusCode.BadRequest, "Parameters can not be null");

            if (string.IsNullOrWhiteSpace(tempBigPerson.Username) || string.IsNullOrWhiteSpace(tempBigPerson.Password))
                return Content(HttpStatusCode.BadRequest, "Please complete all fields");

            try
            {
                var bigPerson = await db.BigPersons.FirstOrDefaultAsync(x => x.Username == tempBigPerson.Username);

                if (bigPerson == null)
                    return Content(HttpStatusCode.BadRequest, "Please check your username");

                if (BCrypt.Net.BCrypt.Verify(tempBigPerson.Password, bigPerson.Password))
                {
                    if (!bigPerson.IsActive)
                        return Content(HttpStatusCode.Forbidden, "Your account is inactive");

                    if (bigPerson.IsDeleted)
                        return Content(HttpStatusCode.BadRequest, "Your account is deleted");

                    var refreshToken = JwtHelper.ValidateRefreshToken(bigPerson.GuidID);

                    var authHeader = Request.Headers.Authorization;
                    if (authHeader == null || authHeader.Scheme != "Bearer")
                    {
                        var accToken = JwtHelper.GenerateAccessTokenForBigPerson(bigPerson);
                        //return Content(HttpStatusCode.BadRequest, "authHeader error");
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
                else
                    return Content(HttpStatusCode.BadRequest, "Please check your password");
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


    }
}
