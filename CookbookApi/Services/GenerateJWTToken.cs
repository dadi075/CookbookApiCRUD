using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System.Collections.Generic;

namespace CookbookApi.Services
{
    public class GenerateJWTToken
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;

        public GenerateJWTToken()
        {
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
        }

        public string IssuingJWT(string user)
        {
            Dictionary<string, object> claims = new Dictionary<string, object> {
                {
                    "username",
                    user
                },
                {
                    "role",
                    "admin"
                }
            };

            string token = _jwtEncoder.Encode(claims, "S"); 
            return token;
        }

    }
}
