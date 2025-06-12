using Microsoft.AspNetCore.Identity;
using System;

namespace PosApi.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? FatherName { get; set; }
        public string? CNIC { get; set; }
        public string? DOB { get; set; } // was DateTime?
public string? JoiningDate { get; set; } // was DateTime?
        public string? Contact { get; set; }
        public string? Image { get; set; }
        
        public bool IsManagement { get; set; }
        public string? MemberType { get; set; }
        public string? RegisterationCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        // These should be int or nullable int if your DB allows nulls
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? Token { get; set; } 
        public DateTime? TokenValidation { get; set; } // Added to fix your exception

        // IdentityUser<int> already includes:
        // - Email, EmailConfirmed
        // - PhoneNumber, PhoneNumberConfirmed
        // - PasswordHash, SecurityStamp, etc.
        // - LockoutEnd (use DateTimeOffset?), LockoutEnabled
        // - UserName, NormalizedUserName
        // - AccessFailedCount, TwoFactorEnabled
    }
}
