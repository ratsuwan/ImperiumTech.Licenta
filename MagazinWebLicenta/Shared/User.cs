using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinWebLicenta.Shared
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordHSalt { get; set; }
		public DateTime DateCreated { get; set; } = DateTime.Now;
		public Address Address { get; set; }
		public string Role { get; set; } = "Customer";
	}
}
