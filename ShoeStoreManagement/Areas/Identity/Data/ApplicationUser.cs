﻿using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // Add property for user
    DateTime createdDate = DateTime.Now;
    [NotMapped]
    public string singleAddress { get; set; } = string.Empty;
    [NotMapped]
    public string selectedRole { get; set; } = string.Empty;
    [NotMapped]
    public string filter { get; set; } = string.Empty;
    public Cart? Cart { get; set; }

    public List<Address> addresses { get; set; } = new List<Address>();
    public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
}
