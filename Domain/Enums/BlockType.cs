using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Enums;

public enum BlockType
{
    [Display(Name = "Contact Us")]
    Contactus = 0,
    [Display(Name = "About Us")]
    AboutUs = 1,
    [Display(Name = "Privacy")]
    Privacy = 2,
    [Display(Name = "Terms")]
    Terms = 3,
    [Display(Name = "Banners Details")]
    BannerSection = 4,
    [Display(Name = "Feedback Admin")]
    FeedbackEmailToAdmin,
    [Display(Name = "Feedback User")]
    FeedbackEmailToUser,
    [Display(Name = "Forgot Password")]
    ForgotPassword,
    [Display(Name = "Notify Admin Subscription")]
    NotifyAdminForSubscription,
    [Display(Name = "Notify User Subscription")]
    NotifyUserForSubscription,
    [Display(Name = "First Approve")]
    FirstApprovedEmailTemplate,
    [Display(Name = "First Security Approval")]
    FirstSecurityApprovalEmailTemplate,
    [Display(Name = "First Medical Approval")]
    FirstMedicalApprovalEmailTemplate,
    [Display(Name = "Complete Passport Document")]
    CompletePassportDocumentEmailTemplate,
    [Display(Name = "Book Ticket")]
    BookTicket,
    [Display(Name = "Home Page")]
    HomePage,
    [Display(Name = "Sign Up Banner")]
    SignUpBanner,
    [Display(Name = "Thanks Email")]
    ThanksEmail,
}


public static class EnumExtensions
{
    public static string DisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();
    }
}