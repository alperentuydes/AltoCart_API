using AltoCartAPI.Helpers;
using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

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



    }
}
