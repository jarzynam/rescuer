﻿using System;

namespace Continuous.User.Users.Model
{
    /// <summary>
    /// User representation 
    /// </summary>
    [Obsolete]
    public class UserModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// User full name
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// User description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User account expiration date - if null, the account will never exipre
        /// </summary>
        public DateTime? AccountExpires { get; set; }

        /// <summary>
        /// User Password - will be empty for getUser
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User password expiration date - if null, password will never expire
        /// </summary>
        public DateTime? PasswordExpires { get; set; }

        /// <summary>
        /// Last date when password has been changed
        /// </summary>
        public DateTime? PasswordLastChange { get; set; }

        /// <summary>
        /// Determines if user must change password at next logon
        /// </summary>
        public bool PasswordMustBeChangedAtNextLogon { get; set; }

        /// <summary>
        /// Minimum length that password must have
        /// </summary>
        public Int64 PasswordMinLength { get; set; }

        /// <summary>
        /// How many times user can type wrong password
        /// </summary>
        public int PasswordMaxBadAttempts { get; set; }

        /// <summary>
        /// For how long system will be remember number of bad password attempts
        /// </summary>
        public TimeSpan PasswordBadAttemptsInterval { get; set; }

        /// <summary>
        /// User can change password by himself
        /// </summary>
        public bool PasswordCanBeChangedByUser { get; set; }

        /// <summary>
        /// Password is required to logon
        /// </summary>
        public bool PasswordRequired { get; set; }
    }
}
