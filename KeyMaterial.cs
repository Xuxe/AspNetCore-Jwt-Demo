using System.Text;
using Microsoft.IdentityModel.Tokens;

public class KeyMaterial
{
    private string _secret;
    private SymmetricSecurityKey _securityKey;
    private SigningCredentials _signingCredentials;

    public KeyMaterial ()
    {
        /* Static 32 Byte key - should not be used in production
           generate a new one and read it from the configuration for example.
           You could also use RSA keys and encrypt the Json Web Token.
           Shouldn't be public!!!
        */
        _secret = "dQ3zZrLrqU4JjlNVW7s6ZTUNmlh2DFdg";
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        _signingCredentials = new SigningCredentials(
            _securityKey,
        SecurityAlgorithms.HmacSha256Signature,
        SecurityAlgorithms.Sha256Digest);
    }

    public SigningCredentials GetSigningCredentials ()
    {
        return _signingCredentials;
    }
}