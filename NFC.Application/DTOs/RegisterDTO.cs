using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFC.Application.DTOs
{
	public class RegisterDTO
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
		[Required]
		public string RoleName { get; set; } // New: to choose role (Admin, User, etc.)
	
    }
}
