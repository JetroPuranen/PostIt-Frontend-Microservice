using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostIt.Application.Dto
{
    public class AddPostDto
    {
        public Guid UserId { get; set; }
        public string? Caption { get; set; }

        public string? ImageData { get; set; }
    }
    
}
