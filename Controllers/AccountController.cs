using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using DrinkAndGo.Data;
using DrinkAndGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrinkAndGo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;


        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //view of register
        public IActionResult Register()
        {

            return View();
        }
        //check your email was used
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailInUse(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Json(true);
            }
            return Json($"Email {Email} is already in use");
        }
        //create a account
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.Email,
                    Email = model.Email

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //create token to send email
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmLink = Url.Action("ConfirmEmail", "Account", 
                        new { userId = user.Id, token = token }, Request.Scheme);
                    await SendEmail(confirmLink, user.Email, "Your account is Succesfully created"
                        , "Please check this Link to confirmation your account : ");
                    //if  create in manager of admin,then return action: ListUsers
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "administration");
                    }

                    return View("ConfirmEmail");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View(model);
        }
        //send email
        public static async Task SendEmail(string content, string ToEmail, string subject, string Title)
        {
            try
            {
                var host = "smtp.gmail.com";
                var port = 587;
                var userName = "tuanhai20062000@gmail.com";
                var password = "tuanhai2018604555";
                var enable = true;
                var smtpClient = new SmtpClient()
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(userName, password)
                };
                var message = new MailMessage(userName, ToEmail);
                message.Subject = subject;
                message.Body = Title + content;
                await smtpClient.SendMailAsync(message);
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        //process link return to confirmation the account from email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null && token == null)
            {
                return RedirectToAction("index", "home");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "id : {is  not found}";
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("Successfully");
            }
            ViewBag.ErrorMessage = "cannot confirm your email";
            return View("Error");
        }
        //logout your account from web
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
       
      
        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            LoginViewModel model = new LoginViewModel()
            {
                returnUrl = ReturnUrl,
                ExternalLogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model);
        }
        //login method 
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            model.ExternalLogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && user.EmailConfirmed == false && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View(model);
                }
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,
                        model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login attemp");
            }

            return View(model);
        }
      
        //login by goggle or fb
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string ReturnUrl)
        {
            var redirect = Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = ReturnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirect);
            return new ChallengeResult(provider, properties);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string remoteError = null, string ReturnUrl= null)
        {
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            LoginViewModel loginViewModel = new LoginViewModel()
            {
                returnUrl = ReturnUrl,
                ExternalLogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error form external provider {remoteError}");
                return View("Login", loginViewModel);
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(" ", "Error loading external login information ");
            }
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                false, true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if(email!= null)
                {
                    var user = await userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        user = new IdentityUser()
                        {
                            UserName = email,
                            Email = email
                        };
                        await userManager.CreateAsync(user);
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(ReturnUrl);
                }
            }
            ViewBag.ErrorMessage = $"Email Claim not received from :{info.LoginProvider}" + "Please contact tuanhai20062000@gmail.com";
            return View("Error");
        }
        //ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && (await userManager.IsEmailConfirmedAsync(user)))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = user.Email,token = token }, Request.Scheme);
                    await SendEmail(passwordResetLink, model.Email, "Reset Password", "Please click this link to comfirmation to Reset your password: ");
                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token )
        {
            if (email != null && token != null) {

                ResetPassWordViewModel model = new ResetPassWordViewModel()
                {
                    Email = email,
                    Token = token
                };
                return View(model);
            }
            ViewBag.ErrorMessage = "the confirmation is failed";
            return View("Error");
            
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassWordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("login","account");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }


    }
}