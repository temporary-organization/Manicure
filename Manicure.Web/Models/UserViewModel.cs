﻿using System.Web;

namespace Manicure.Web.Models
{
    public class UserViewModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public HttpPostedFileBase Photo { get; set; }

        public string Role { get; set; }
    }
}