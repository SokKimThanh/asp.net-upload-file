using ASP_NET_OnTap_CuoiKy_PhanTrang_CRUD_SESSION_LOGIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using X.PagedList.Extensions;

namespace ASP_NET_OnTap_CuoiKy_PhanTrang_CRUD_SESSION_LOGIN.Controllers
{
    public class AlbumController : Controller
    {
        private readonly MusicContext musicContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AlbumController(MusicContext musicContext, IWebHostEnvironment webHostEnvironment)
        {
            this.musicContext = musicContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult CreateList(int? page)
        {
            var list = musicContext.Albums.ToPagedList(page ?? 1, 10);
            return View(list);
        }

        public IActionResult CreateAlbum()
        {
            ViewBag.SelectListArtist = new SelectList(musicContext.Artists.ToList(), "ArtistId", "Name");
            ViewBag.SelectListGenre = new SelectList(musicContext.Genres.ToList(), "GenreId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateAlbum(AlbumUploadModel albumUploadModel)
        {
            if (ModelState.IsValid)
            {
                // uploadfolder
                var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                // filename
                var fileName = Guid.NewGuid().ToString() + "_" + albumUploadModel.AlbumArtUrl?.FileName;
                // fileppath
                var filePath = Path.Combine(uploadFolder, fileName);
                // filestream
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    // coptyTo
                    albumUploadModel?.AlbumArtUrl?.CopyTo(fileStream);
                }

                Album album = new Album()
                {
                    AlbumArtUrl = fileName,
                    Price = albumUploadModel.Price,
                    Title = albumUploadModel.Title,
                    ArtistId = albumUploadModel.ArtistId,
                    GenreId = albumUploadModel.GenreId,
                };

                musicContext.Add(album);
                musicContext.SaveChanges();
            }

            return RedirectToAction("CreateList");
        }

        [HttpGet]
        public IActionResult EditAlbum(int id)
        {
            var album = musicContext.Albums.SingleOrDefault(item => item.AlbumId == id);

            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "Images", album.AlbumArtUrl);

            // gan hinh mac dinh khi khong co hinh trong csdl
            if (System.IO.File.Exists(imagePath))
            {
                // neu co hinh
                ViewBag.imagePath = "../../Images/" + album.AlbumArtUrl;
            }
            else
            {
                imagePath = "../../Images/default.png";
                ViewBag.imagePath = imagePath;
            }

            if (album == null)
            {
                return NotFound();
            }

            ViewBag.SelectListGenres = new SelectList(musicContext.Genres.ToList(), "GenreId", "Name");
            ViewBag.SelectListArtist = new SelectList(musicContext.Artists.ToList(), "ArtistId", "Name");

            return View(album);
        }

        [HttpPost]
        public IActionResult EditAlbum(int id, AlbumUploadModel albumUploadModel)
        {
            // Tìm album
            var album = musicContext.Albums.SingleOrDefault(item => item.AlbumId == id);


            // check null album
            if (album == null)
            {
                return NotFound();
            }
            // check modelstate 
            if (ModelState.IsValid)
            {
                // check hình trong form data

                if (albumUploadModel.AlbumArtUrl != null)
                {
                    // Xóa ảnh cũ 
                    var oldImage = Path.Combine(webHostEnvironment.WebRootPath, "Images", album.AlbumArtUrl);
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                    // Lưu ảnh mới 
                    // uploadfolder
                    var uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    // filename
                    var fileName = Guid.NewGuid().ToString() + "_" + albumUploadModel.AlbumArtUrl?.FileName;
                    // fileppath
                    var filePath = Path.Combine(uploadFolder, fileName);


                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        // coptyTo
                        albumUploadModel?.AlbumArtUrl?.CopyTo(fileStream);
                    }


                    // Cập nhật thông tin album
                    album.AlbumArtUrl = fileName;
                    album.ArtistId = albumUploadModel.AlbumId;
                    album.Title = albumUploadModel.Title;
                    album.GenreId = albumUploadModel.GenreId;

                    // Lưu thay đổi vào DB
                    musicContext.Update(album);
                    musicContext.SaveChanges();
                }
            }
            // return album list
            return RedirectToAction("CreateList");
        }
        /// <summary>
        /// Get request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteAlbum(int id)
        {
            var album = musicContext.Albums.Find(id);
            if (album == null)
            {
                return NotFound();
            }

            // Xóa ảnh cũ 
            if (album.AlbumArtUrl != "default.png")
            {
                var oldImage = Path.Combine(webHostEnvironment.WebRootPath, "Images", album.AlbumArtUrl);

                if (System.IO.File.Exists(oldImage))
                {
                    try
                    {
                        // Xóa tệp
                        System.IO.File.Delete(oldImage);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Không thể xóa tệp: {ex.Message}");
                    }
                }
            }
            musicContext.Remove(album);
            musicContext.SaveChanges();
            return RedirectToAction("CreateList");
        }
    }
}
