namespace ASP_NET_OnTap_CuoiKy_PhanTrang_CRUD_SESSION_LOGIN.Models
{
    public class AlbumUploadModel
    {
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public IFormFile? AlbumArtUrl { get; set; }
    }
}
