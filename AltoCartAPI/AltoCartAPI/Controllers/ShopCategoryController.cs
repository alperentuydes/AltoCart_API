using AltoCartAPI.Helpers;
using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Services.Description;

namespace AltoCartAPI.Controllers
{
    [RoutePrefix("api/Shop_Category")]
    public class ShopCategoryController : ApiController
    {

        #region En Eskiler

        //AltoCartDB db = new AltoCartDB();

        //[Route("Shop_Category_Add")]
        //[HttpPost]
        //public async Task<IHttpActionResult> ShopCategoryAdd([FromBody] TempShopCategory tempShopCat)
        //{

        //    var authHeader = Request.Headers.Authorization;
        //    if (authHeader == null || authHeader.Scheme != "Bearer")
        //        return Content(HttpStatusCode.Unauthorized, $"Access Token is missing or invalid");

        //    var accessToken = authHeader.Parameter;

        //    var result = JwtHelper.ValidateToken(accessToken);

        //    if(result == null)
        //        return Content(HttpStatusCode.Unauthorized, $"Access token is null");

        //    var userId = result.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Content(HttpStatusCode.Unauthorized, "BigPerson's Guid-ID not found in token");

        //    Guid userGuidID = Guid.Parse(userId);

        //    if (await db.BigPersons.FirstOrDefaultAsync(x => x.GuidID == userGuidID) != null)
        //    {

        //        if (tempShopCat == null)
        //            return BadRequest("Fields are null");
        //        if (tempShopCat.CategoryName == null)
        //            return BadRequest("Category name is null");
        //        if (tempShopCat.CategroyDescription == null)
        //            return BadRequest("Category description is null");

        //        if (db.ShopCategories.FirstOrDefault(x => x.ShopCategoryName == tempShopCat.CategoryName) != null)
        //            return BadRequest("This category name already exist");

        //        ShopCategory shopCategory = new ShopCategory()
        //        {
        //            ShopCategoryName = tempShopCat.CategoryName,
        //            ShopCategoryDescription = tempShopCat.CategroyDescription
        //        };

        //        try
        //        {
        //            db.ShopCategories.Add(shopCategory);
        //            await db.SaveChangesAsync();
        //            return Ok("Shop's category successfully added");
        //        }
        //        catch (DbUpdateException dbEx)
        //        {
        //            return BadRequest($"{dbEx.InnerException?.Message ?? dbEx.Message}");
        //        }

        //    }
        //    else
        //        return Content(HttpStatusCode.Unauthorized, "User ID is not found in database");

        //}

        #endregion

        // BIGPERSONLARIN ISDELETED VE ISACTIVE KISIMLARININ KONTROLLERINI YAP

        AltoCartDB db;

        [HttpPost]
        [Route("create_shop_category")]
        public async Task<IHttpActionResult> CreateShopCategory([FromBody] TempShopCategory tempShopCategory)
        {
            var requestHeader = Request.Headers.Authorization;
            if (requestHeader == null || requestHeader.Scheme != "Bearer")
                return Content(HttpStatusCode.Forbidden, "AccessToken is missing or invalid");

            var accessToken = requestHeader.Parameter;
            var claims = JwtHelper.ValidateAccessToken(accessToken);

            if (claims == null)
                return Content(HttpStatusCode.Forbidden, "AccessToken is null");


            try
            {
                using (db = new AltoCartDB())
                {

                    #region Eger ClaimTypes.Role tanımlı olursa

                    //ClaimsIdentity claimsIdentity = (ClaimsIdentity)claims.Identity;
                    //string role = claimsIdentity.FindFirst(ClaimTypes.Role).Value;
                    //if (role != null)
                    //    return Content(HttpStatusCode.Forbidden, "No ROLE");
                    //if (role != "BigPerson")
                    //    return Content(HttpStatusCode.Forbidden, "User Not BIGPERSON");

                    #endregion

                    #region Eger ClaimTypes.NameIdenifier dan GuidID ile tanımlamak istersek

                    ClaimsIdentity claimsIdentity = (ClaimsIdentity)claims.Identity; // Burada sanırsam SECURITY TOKEN olabilir.
                    if (claimsIdentity == null)
                        return Content(HttpStatusCode.Forbidden, "ClaimIndentity is null");

                    Guid identityGuid = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

                    if (identityGuid == null)
                        return Content(HttpStatusCode.Forbidden, "Guid ID not found");

                    //if (!await db.BigPersons.AnyAsync(x => x.GuidID == identityGuid))
                    //    return Content(HttpStatusCode.Forbidden, "BigPerson not found in database");

                    BigPerson bigPerson = await db.BigPersons.FirstOrDefaultAsync(x => x.GuidID == identityGuid);

                    if (bigPerson == null)
                        return Content(HttpStatusCode.Forbidden, "BigPerson not found in database");

                    if (tempShopCategory == null)
                        return Content(HttpStatusCode.BadRequest, "Fields are null");

                    if (string.IsNullOrWhiteSpace(tempShopCategory.CategoryName) || string.IsNullOrWhiteSpace(tempShopCategory.CategroyDescription))
                        return Content(HttpStatusCode.BadRequest, "Please complete all fields");

                    ShopCategory shopCategory = new ShopCategory()
                    {
                        ShopCategoryName = tempShopCategory.CategoryName,
                        ShopCategoryDescription = tempShopCategory.CategroyDescription,
                        IsDeleted = false
                    };

                    if (await db.ShopCategories.AnyAsync(x => x.ShopCategoryName == shopCategory.ShopCategoryName))
                        return Content(HttpStatusCode.BadRequest, "This shop category already created");

                    db.ShopCategories.Add(shopCategory);
                    await db.SaveChangesAsync();

                    return Content(HttpStatusCode.OK, new
                    {
                        Message = $"Shop category successfully created. Created Shop Category = {shopCategory.ShopCategoryName}. Description = {shopCategory.ShopCategoryDescription}"
                    });

                    #endregion
                }
            }
            catch (DbException ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired. {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, $"Error accuired. {ex.InnerException?.Message ?? ex.Message}");
            }

        }

        //[HttpGet]
        //[Route("list_shop_category")]
        //public async Task<(IHttpActionResult, List<ShopCategory>)> ListShopCategory()
        //{

        //    var tokenHeader = Request.Headers.Authorization;
        //    if (tokenHeader == null || tokenHeader.Scheme != "Bearer")
        //        return (Content(HttpStatusCode.BadRequest,"invalid token"), null);

        //    List<ShopCategory> shops = await db.ShopCategories.ToListAsync();
        //    return ((Content(HttpStatusCode.OK, "invalid token"), shops) );
        //}

        [HttpGet]
        [Route("list_shop_category")]
        public async Task<IHttpActionResult> ListShopCategory()
        {

            var tokenHeader = Request.Headers.Authorization;
            if (tokenHeader == null || tokenHeader.Scheme != "Bearer")
                return Content(HttpStatusCode.BadRequest, "AccessToken is missing or invalid");

            string accessToken = tokenHeader.Parameter;
            var claims = JwtHelper.ValidateAccessToken(accessToken);

            if (claims == null)
                return Content(HttpStatusCode.BadRequest, "Claims are null");

            using (db = new AltoCartDB())
            {
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)claims.Identity;
                Guid guidID = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);

                try
                {
                    BigPerson bigPerson = await db.BigPersons.FirstOrDefaultAsync(x => x.GuidID == guidID);

                    if (bigPerson == null)
                        return Content(HttpStatusCode.Forbidden, "Not Bigperson");

                    List<ShopCategory> shops = await db.ShopCategories.ToListAsync();
                    return Content(HttpStatusCode.OK, new
                    {
                        Message = "List func successfull",
                        Shops = shops
                    });

                }
                catch (DbException ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error accuired. {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error accuired. {ex.InnerException?.Message ?? ex.Message}");
                }
            }


        }

        [HttpDelete]
        [Route("delete_shop_category")]
        public async Task<IHttpActionResult> DeleteShopCategory([FromBody] TempShopCategory tempShopCategory)
        {
            Guid guidID = Guid.Parse(CheckAccessToken());


            using (db = new AltoCartDB())
            {
                try
                {
                    BigPerson bigperson = await db.BigPersons.FirstOrDefaultAsync(x => x.GuidID == guidID);

                    if (bigperson == null)
                        return Content(HttpStatusCode.Forbidden, "Not BigPerson");

                    if (tempShopCategory == null)
                        return Content(HttpStatusCode.BadRequest, "Temp Shop Category is null");

                    if (tempShopCategory.ID == -1)
                        return Content(HttpStatusCode.BadRequest, "Please enter ID number for delete shop category");

                    ShopCategory shopCategory = await db.ShopCategories.FirstOrDefaultAsync(x => x.ID == tempShopCategory.ID);

                    if (shopCategory == null)
                        return Content(HttpStatusCode.BadRequest, "Please enter valid ID number for delete shop category");

                    shopCategory.IsDeleted = true;
                    await db.SaveChangesAsync();

                    //db.ShopCategories.Remove(shopCategory);
                    //db.SaveChanges();
                    return Content(HttpStatusCode.OK, new
                    {
                        Message = "Shop Category Delete From Our Database"
                    });

                }
                catch (DbException ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error accuired. {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error accuired. {ex.InnerException?.Message ?? ex.Message}");
                }
            }
        }

        public string CheckAccessToken()
        {
            var requestedHeaderAuth = Request.Headers.Authorization;
            if (requestedHeaderAuth == null || requestedHeaderAuth.Scheme != "Bearer")
                throw new SecurityTokenArgumentException("Access token is invalid or empty");

            var claims = JwtHelper.ValidateAccessToken(requestedHeaderAuth.Parameter);
            if (claims == null)
                throw new ArgumentNullException("Access token is null");

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)claims.Identity;
            if (claimsIdentity == null)
                throw new SecurityTokenException("claims identities are null");

            string guidID = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (guidID == null)
                throw new SecurityTokenArgumentException("Guid ID is null");

            return guidID;
        }

    }
}
