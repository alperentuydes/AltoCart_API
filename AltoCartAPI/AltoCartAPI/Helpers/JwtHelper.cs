using AltoCartAPI.Models;
using AltoCartAPI.TemporaryModels;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.EntitySql;
using System.Data.Entity.Core.Metadata.Edm;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using System.Web.SessionState;
using System.Web.UI.WebControls;

namespace AltoCartAPI.Helpers
{
    public class JwtHelper
    {

        private const string jwtKey = "fe9b655019e715bef92d08c058e832e71bff1f4575de656b6e58002a3c457835";
        private const string issuer = "AltoCartAPI";
        private const string audience = "AltoCartWEB";


        public static string GenerateAccessTokenForBigPerson(BigPerson bigPerson, int expireMinute = 30)
        {
            var EncodedeKey = Encoding.UTF8.GetBytes(jwtKey);

            var claims = new List<Claim>()
            {
                new Claim("jti", Guid.NewGuid().ToString()),

                //new Claim("role", "BigPerson"), // Burası sonradan eklendi
                new Claim(ClaimTypes.Role, "BigPerson"), // Burası sonradan eklendi

                new Claim(ClaimTypes.NameIdentifier, bigPerson.GuidID.ToString()),
                new Claim(ClaimTypes.Name, bigPerson.Username),
                new Claim(ClaimTypes.Email, bigPerson.Email),
                new Claim("IsActive", bigPerson.IsActive.ToString(), ClaimValueTypes.Boolean),
                new Claim("IsDeleted", bigPerson.IsDeleted.ToString(), ClaimValueTypes.Boolean),
                new Claim("ExpireDateOfAccessToken", DateTime.UtcNow.AddMinutes(15).ToString("yyyy-MM-dd HH:mm:ss")),
            };


            var claimIdentity = new ClaimsIdentity(claims);

            var secTokenDesc = new SecurityTokenDescriptor()
            {
                Subject = claimIdentity,
                Expires = DateTime.UtcNow.AddMinutes(expireMinute),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(EncodedeKey), SecurityAlgorithms.HmacSha256),
                Issuer = issuer,
                Audience = audience,

            };

            JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
            var CreatedToken = TokenHandler.CreateToken(secTokenDesc);
            return TokenHandler.WriteToken(CreatedToken);
        }

        public static RefreshToken GenerateRefreshTokenForBigPerson(BigPerson bigPerson, int expireDay = 15)
        {
            //AltoCartDB db = new AltoCartDB();

            try
            {
                using (AltoCartDB db = new AltoCartDB())
                {
                    var EncodedKey = Encoding.UTF8.GetBytes(jwtKey);

                    var secTokenDesc = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                    new Claim(ClaimTypes.NameIdentifier, bigPerson.GuidID.ToString())
                }),
                        Expires = DateTime.UtcNow.AddDays(expireDay),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(EncodedKey), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var secTokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = secTokenHandler.CreateToken(secTokenDesc);
                    var Token = secTokenHandler.WriteToken(createdToken);

                    RefreshToken refToken = new RefreshToken()
                    {
                        ExpireDate = secTokenDesc.Expires.Value,
                        Token = Token,
                        UserGuid = bigPerson.GuidID,
                    };
                    db.RefreshTokens.Add(refToken);
                    db.SaveChanges();
                    return refToken;

                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating refresh token for big person {bigPerson.GuidID}. Message: {ex.Message}", ex);
            }
            //return new RefreshToken()
            //{
            //    ExpireDate = secTokenDesc.Expires.Value,
            //    Token = Token,
            //    UserGuid = bigPerson.GuidID,
            //};
        }


        public static string ValidateRefreshToken(Guid bigPersonsGuid)
        {
            if (bigPersonsGuid == Guid.Empty)
                throw new ArgumentNullException("Big person Guid ID is null");

            AltoCartDB db = new AltoCartDB();

            BigPerson bigPerson = db.BigPersons.FirstOrDefault(x => x.GuidID == bigPersonsGuid);
            if (bigPerson != null)
            {

                RefreshToken refreshToken = db.RefreshTokens.FirstOrDefault(x => x.UserGuid == bigPersonsGuid);

                if (refreshToken == null)
                {
                    //throw new Exception("Big person GuidID not found in refresh token database");
                    var refToken = JwtHelper.GenerateRefreshTokenForBigPerson(bigPerson);
                    return refToken.Token;
                }


                if (refreshToken.ExpireDate > DateTime.UtcNow)
                {
                    //var result = JwtHelper.GenerateAccessTokenForBigPerson(bigPerson);
                    //return result;
                    return refreshToken.Token;
                }
                else
                    return null;
            }
            else
                throw new Exception("Invalid GuidID for Big Person (Big Person not found)");
        }

        public static ClaimsPrincipal ValidateAccessToken(string accessToken)// GELEN ACCESSTOKEN DOLULUK KONTROLU??
        {
            // BURASI DÜZGÜN BİR ŞEKİLDE ÇALIŞIYOR. TOKEN SÜRESİ GEÇMİŞ İSE HATA VERİYOR OLMASI GEREKTİĞİ GİBİ!! HİÇ BİR SIKINTI YOK

            var EncodedKey = Encoding.UTF8.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            if (jwtToken != null)
            {
                var valTokenParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(EncodedKey),
                    ValidateIssuerSigningKey = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                };

                try
                {
                    ClaimsPrincipal claims = tokenHandler.ValidateToken(accessToken, valTokenParameters, out var validatedToken);

                    if (validatedToken.ValidFrom > DateTime.UtcNow) // SANKİ BURAYI HEP GEÇİYOR... FALSE DÖNÜYOR
                    {
                        return claims;
                    }
                    else
                    {
                        //Claim identity = claims.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
                        //ClaimsIdentity identity = (ClaimsIdentity)claims.Identity;
                        //Guid guidID = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

                        ClaimsIdentity identity = (ClaimsIdentity)claims.Identity;
                        Guid guidID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                        string result = ValidateRefreshToken(guidID);

                        if (result != null)
                        {
                            return claims;
                        }
                        else
                        {
                            throw new Exception("Refresh token is expired or invalid.");
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Invalid Access Token", ex);
                }

            }
            else
                throw new ArgumentException("Invalid Access Token");

        }

        //public static async Task<string> ValidateRefreshTokenForOwner(Guid guidID)
        public static string ValidateRefreshTokenForOwner(Guid guidID)
        {
            if (guidID == null)
                throw new ArgumentNullException("Guid ID is null. JWTHelper/ValidateRefreshTokenForOwner ");

            try
            {
                using (AltoCartDB db = new AltoCartDB())
                {

                    //if (!await db.Owners.AnyAsync(x => x.Guid_ID == guidID))
                    if (!db.Owners.Any(x => x.Guid_ID == guidID))
                            throw new NoNullAllowedException("Owner not found in database");

                    RefreshToken refreshToken = db.RefreshTokens.FirstOrDefault(x => x.UserGuid == guidID);
                    if (refreshToken == null)
                    {
                        var _newRefreshToken = JwtHelper.GenerateRefreshTokenForOwner(db.Owners.FirstOrDefault(x => x.Guid_ID == guidID));
                        return _newRefreshToken.Token;
                    }

                    if (refreshToken.ExpireDate > DateTime.UtcNow)
                    {
                        return refreshToken.Token;
                    }
                    else
                        return null;

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error accuired (JWTHelper/ValidateRefreshTokenForOwner). {ex.InnerException?.Message ?? ex.Message} ");
            }

        }

        public static ClaimsPrincipal ValidateAccessTokenForOwner(string accessToken)
        {

            var EncodedKey = Encoding.UTF8.GetBytes(jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            if (jwtToken != null)
            {
                var valTokenParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(EncodedKey),
                    ValidateIssuerSigningKey = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                };

                try
                {
                    ClaimsPrincipal claims = tokenHandler.ValidateToken(accessToken, valTokenParameters, out var validatedToken);

                    if (validatedToken.ValidFrom > DateTime.UtcNow)
                    {
                        return claims;
                    }
                    else
                    {
                        //Claim identity = claims.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
                        //ClaimsIdentity identity = (ClaimsIdentity)claims.Identity;
                        //Guid guidID = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

                        ClaimsIdentity identity = (ClaimsIdentity)claims.Identity;
                        Guid guidID = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                        //Task<string> result = ValidateRefreshTokenForOwner(guidID);
                        string result = ValidateRefreshTokenForOwner(guidID);

                        if (result != null)
                        {
                            return claims;
                        }
                        else
                        {
                            throw new Exception("Refresh token is expired or invalid.");
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Invalid Access Token", ex);
                }

            }
            else
                throw new ArgumentException("Invalid Access Token");
        }


        //public static async Task<string> GenerateAccessTokenForOwner(Owner owner, int expireMinute = 30)
        public static string GenerateAccessTokenForOwner(Owner owner, int expireMinute = 30)
        {
            var encodedKey = Encoding.UTF8.GetBytes(jwtKey);

            try
            {
                using (AltoCartDB db = new AltoCartDB())
                {
                    var claims = new List<Claim>
                     {
                        new Claim("ClaimGuid", Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, owner.Guid_ID.ToString()),
                        new Claim(ClaimTypes.Name, owner.OwnerName),
                        new Claim(ClaimTypes.Surname, owner.OwnerSurname),
                        //new Claim("City", owner.Cities.CityName),
                        //new Claim("City", Convert.ToString(await db.Cities.FirstOrDefaultAsync(x => x.ID == owner.City_ID).Result.CityName)),
                        new Claim("City", Convert.ToString(db.Cities.FirstOrDefault(x => x.ID == owner.City_ID).CityName)),
                        new Claim(ClaimTypes.DateOfBirth, owner.BirthDate.ToString("yyyy-MM-dd")),
                        new Claim(ClaimTypes.Email, owner.OwnerEmail),
                        new Claim("IsActive" , owner.IsActive.ToString()),
                        //new Claim(ClaimTypes.Country, owner.Countries.CountryName),
                        new Claim(ClaimTypes.Country, Convert.ToString(db.Countries.FirstOrDefault(x => x.ID == owner.Country_ID).CountryName)),
                        new Claim("IsDeleted" , owner.IsDeleted.ToString()),
                        new Claim("ExpireDate" , DateTime.UtcNow.AddMinutes(expireMinute).ToString()),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);

                    SecurityTokenDescriptor secToken = new SecurityTokenDescriptor
                    {
                        Issuer = issuer,
                        Audience = audience,
                        Subject = claimsIdentity,
                        Expires = Convert.ToDateTime(claimsIdentity.Claims.First(x => x.Type == "ExpireDate").Value),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256)
                    };

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(secToken);
                    return tokenHandler.WriteToken(createdToken);
                }

            }
            catch
            {
                return null;
            }
        }

        public static RefreshToken GenerateRefreshTokenForOwner(Owner owner, int expireDay = 15)
        {
            #region Old without USING

            //AltoCartDB db = new AltoCartDB();

            //var EncodedKey = Encoding.UTF8.GetBytes(jwtKey);

            //var secTokenDesc = new SecurityTokenDescriptor()
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, owner.Guid_ID.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(expireDay),
            //    Issuer = issuer,
            //    Audience = audience,
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(EncodedKey), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var secTokenHandler = new JwtSecurityTokenHandler();
            //var createdToken = secTokenHandler.CreateToken(secTokenDesc);
            //var Token = secTokenHandler.WriteToken(createdToken);

            //try
            //{
            //    RefreshToken refToken = new RefreshToken()
            //    {
            //        ExpireDate = secTokenDesc.Expires.Value,
            //        Token = Token,
            //        UserGuid = owner.Guid_ID,
            //    };
            //    db.RefreshTokens.Add(refToken);
            //    db.SaveChanges();
            //    return refToken;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

            #endregion

            try
            {
                // Otomatik olarak dispose işlemini yapar.
                using (AltoCartDB db = new AltoCartDB())
                {
                    var encodedKey = Encoding.UTF8.GetBytes(jwtKey);

                    var secTokenDesc = new SecurityTokenDescriptor()
                    {
                        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, owner.Guid_ID.ToString()) }),
                        Expires = DateTime.UtcNow.AddDays(expireDay),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256)
                    };

                    var secTokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = secTokenHandler.CreateToken(secTokenDesc);
                    var token = secTokenHandler.WriteToken(createdToken);

                    RefreshToken refreshToken = new RefreshToken()
                    {
                        ExpireDate = secTokenDesc.Expires.Value,
                        Token = token,
                        UserGuid = Guid.Parse(secTokenDesc.Subject.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                    };

                    db.RefreshTokens.Add(refreshToken);
                    db.SaveChanges();

                    return refreshToken;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating refresh token for owner {owner.Guid_ID}. Message: {ex.Message}", ex);
            }
        }


        //public static string GenerateAccessToken(Guid userGuidID, int expireMinute = 30)
        //{
        //    var EncodedKey = Encoding.UTF8.GetBytes(jwtKey);
        //    DateTime Now = DateTime.UtcNow;

        //    var TokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, userGuidID.ToString())
        //        }),
        //        Expires = Now.AddMinutes(expireMinute),
        //        Issuer = Issuer,
        //        Audience = Audience,
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(EncodedKey), SecurityAlgorithms.HmacSha256Signature)
        //    };


        //    JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
        //    var CreatedToken = TokenHandler.CreateToken(TokenDescriptor);
        //    return TokenHandler.WriteToken(CreatedToken);
        //}

        #region En Eskiler

        /*
         //public static RefreshToken GenerateRefreshToken(Guid guidID, int expireDate = 15)
        //{
        //    var EncodedKey = Encoding.UTF8.GetBytes(jwtKey);
        //    DateTime Now = DateTime.UtcNow;

        //    var TokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, guidID.ToString())
        //        }),
        //        Expires = Now.AddDays(expireDate),
        //        Issuer = Issuer,
        //        Audience = Audience,
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(EncodedKey), SecurityAlgorithms.HmacSha256Signature)
        //    };


        //    JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
        //    var CreatedToken = TokenHandler.CreateToken(TokenDescriptor);
        //    var Token = TokenHandler.WriteToken(CreatedToken);
        //    return new RefreshToken()
        //    {
        //        ExpireDate = Now.AddDays(expireDate),
        //        Token = Token,
        //        UserGuid = guidID,
        //    };
        //}

        //public static ClaimsPrincipal ValidateToken(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(jwtKey);

        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidIssuer = Issuer,
        //        ValidAudience = Audience,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuerSigningKey = true,
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true
        //    };

        //    try
        //    {
        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
        //        return principal;
        //    }
        //    catch (SecurityTokenExpiredException)
        //    {
        //        throw new SecurityTokenExpiredException("Token is expired");///<<summary> BURADA </summary>
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Invalid token", ex);
        //    }

        //}
         */

        #endregion

        //public static string LearnFromRefreshToken(Guid guidID)
        //{
        //    AltoCartDB db = new AltoCartDB();

        //    if (guidID == null)
        //        throw new NullReferenceException("Guid ID is null");

        //    var RefreshTokensUser = db.RefreshTokens.FirstOrDefault(x => x.UserGuid == guidID);

        //    if (RefreshTokensUser != null)
        //    {
        //        if (RefreshTokensUser.ExpireDate > DateTime.UtcNow)
        //        {
        //            string AccessToken = GenerateAccessToken(guidID);
        //            return AccessToken;
        //        }
        //        else
        //            throw new Exception("Token is expired");
        //    }
        //    else
        //        throw new ArgumentNullException("Tokens account not found");

        //}

    }
}