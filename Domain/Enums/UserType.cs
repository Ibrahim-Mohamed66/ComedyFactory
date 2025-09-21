using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum UserType
    {
        [Display(Name = "Dashboard")]
        Dashboard,
        [Display(Name = "User")]
        User,
        [Display(Name = "Guest")]
        Guest,
        [Display(Name = "Trainer")]
        Trainer,
        [Display(Name = "Admin")]
        Admin,
    }
}
