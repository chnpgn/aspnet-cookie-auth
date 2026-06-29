using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Security_UnderTheHood.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; } = new Credential();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Verify the credentials
            if (Credential.Username == "admin" && Credential.Password == "admin")
            {
                // Create the security context
                var claims = new List<Claim> {
                    new(ClaimTypes.Name, "admin"),
                    new(ClaimTypes.Email, "admin@example.com"),
                    new("Department", "HR"), // Custom claim
                    new("Admin", "True"),
                    new("Manager", "True"),
                    new("EmploymentDate", "2026-02-22")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");

                ClaimsPrincipal ClaimsPrincipal = new(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", ClaimsPrincipal, authProperties);

                return RedirectToPage("/index");
            }
            return Page();
        }
    }

    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
