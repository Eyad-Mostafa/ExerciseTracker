using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ExerciseTracker.API.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.NoResult());

        var authHeaders = Request.Headers["Authorization"].ToString();

        if (!authHeaders.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.Fail("Unknown Scheme"));

        //        var encodedCredentials = authHeaders.Substring("Basic ".Length);
        var encodedCredentials = authHeaders["Basic ".Length..]; // Range operator is better here.

        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

        var usernameAndPassword = decodedCredentials.Split(':');
        if (usernameAndPassword[0] != "admin" || usernameAndPassword[1] != "pass")
            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));

        var identity = new ClaimsIdentity(
        [
            new Claim (ClaimTypes.NameIdentifier, "1"),
            new Claim (ClaimTypes.Name, usernameAndPassword[0])
        ], "Basic");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Basic");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
