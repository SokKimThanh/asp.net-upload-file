using System;
using System.Collections.Generic;

namespace ASP_NET_OnTap_CuoiKy_PhanTrang_CRUD_SESSION_LOGIN.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Albums = new HashSet<Album>();
        }

        public int GenreId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
