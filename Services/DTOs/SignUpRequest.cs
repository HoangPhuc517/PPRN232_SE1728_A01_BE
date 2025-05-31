using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum;

namespace Services.DTOs
{
    public class SignUpRequest
    {
        [Required]
        public string AccountName { get; set; }

        [Required]
        [EmailAddress]
        public string AccountEmail { get; set; }

        [Required]
        public RoleEnum AccountRole { get; set; }

        [Required]
        public string AccountPassword { get; set; }
    }
}
