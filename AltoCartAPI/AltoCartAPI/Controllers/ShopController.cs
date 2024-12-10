using AltoCartAPI.Helpers;
using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AltoCartAPI.Controllers
{
    [RoutePrefix("api/Shop_C_qeWqQmNsCCeRiIOZ")]
    public class ShopController : ApiController
    {

        #region En Eskiler

        /*
          AltoCartDB db = new AltoCartDB();

        [Route("Shop_Register_wERwCCCpOsLLIiiVwNm")]
        [HttpPost]
        public async Task<IHttpActionResult> ShopRegister([FromBody] TempShop tempShop)
        {

            var authHeader = Request.Headers.Authorization;
            if (authHeader == null ||authHeader.Scheme != "Bearer")
                return Content(HttpStatusCode.Unauthorized, "Access token is missing or invalid");

            var accessToken = authHeader.Parameter;

            var result = JwtHelper.ValidateToken(accessToken);

            if (result == null)
                return Content(HttpStatusCode.Unauthorized, "Access token is null");

            var userID = result.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userID))
                return Content(HttpStatusCode.Unauthorized, "Owner's Guid-ID is null");

            Guid userGuidID = Guid.Parse(userID);

            if (await db.Owners.FirstOrDefaultAsync(x => x.Guid_ID == userGuidID) != null)
            {

                if (tempShop == null)
                    return BadRequest("Fields are null");
                if (tempShop.ShopEmail == null)
                    return BadRequest("Shop email is null");
                if (tempShop.ShopName == null)
                    return BadRequest("Shop name is null");
                if (tempShop.ShopDescription == null)
                    return BadRequest("Shop Description is null");
                if (tempShop.ShopCategory_ID == -1)
                    return BadRequest("Shop category did not selected");
                if (tempShop.City_ID == -1)
                    return BadRequest("City did not selected");

                Shop shop = new Shop()
                {

                    ShopName = tempShop.ShopName,
                    ShopDescription = tempShop.ShopDescription,
                    ShopCategory_ID = tempShop.ShopCategory_ID,
                    City_ID = tempShop.City_ID,
                    ShopImageUrl = tempShop.ShopImageUrl,
                    ShopRating = 0,
                    IsApprovedByBigPerson = false,
                    ApprovedByWho = "not approved by someone",
                    PostalCode = tempShop.PostalCode,
                    PhoneNumber = tempShop.PhoneNumber,
                    ShopEmail = tempShop.ShopEmail,
                    FullAdress = tempShop.FullAdress,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                try
                {
                    db.Shops.Add(shop);
                    db.SaveChanges();
                    return Content(HttpStatusCode.Created, "Shop successfully added");
                }
                catch (DbException dbEx)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error occuired {dbEx.InnerException?.Message ?? dbEx.Message}");
                }
                catch (Exception ex)
                {
                    return Content(HttpStatusCode.InternalServerError, $"Error occuired {ex.InnerException?.Message ?? ex.Message}");
                }

            }
            else 
            return Content(HttpStatusCode.Unauthorized, "Guid-ID not found in database");





        }
         */

        #endregion



    }
}
