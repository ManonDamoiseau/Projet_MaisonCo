using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Services.UserAccountMapping;


namespace Projet_MaisonCo_backend.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public UserRole Role { get; set; }
	}
}
