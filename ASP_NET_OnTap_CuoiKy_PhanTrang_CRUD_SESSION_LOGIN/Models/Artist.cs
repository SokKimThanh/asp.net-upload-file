using System;
using System.Collections.Generic;

namespace ASP_NET_OnTap_CuoiKy_PhanTrang_CRUD_SESSION_LOGIN.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
        }

        public int ArtistId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}
