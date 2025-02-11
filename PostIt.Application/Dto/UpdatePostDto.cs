using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostIt.Application.Dto
{
    public class UpdatePostDto
    {
        public Guid? Id { get; set; }
        public string? Caption { get; set; }
        public Guid? LikedByUserId { get; set; }
        public CommentDto? Comment { get; set; }
    }

    
}
