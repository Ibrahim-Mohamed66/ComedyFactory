using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public IEnumerable<string?> Errors { get; set; } = new List<string>();
        public string? Token { get; set; }   
        public string? RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
