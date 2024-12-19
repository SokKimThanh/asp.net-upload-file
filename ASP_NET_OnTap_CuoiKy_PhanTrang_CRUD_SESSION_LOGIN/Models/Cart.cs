using System;
using System.Collections.Generic;

namespace ASP_NET_OnTap_CuoiKy_PhanTrang_CRUD_SESSION_LOGIN.Models
{
    public partial class Cart
    {
        public int RecordId { get; set; }
        public string CartId { get; set; } = null!;
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Album Album { get; set; } = null!;
    }
}
