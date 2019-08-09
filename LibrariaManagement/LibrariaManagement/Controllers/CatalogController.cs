using System.Linq;
using LibrariaManagement.Models.Catalog;
using LibraryData;
using Microsoft.AspNetCore.Mvc;

namespace LibrariaManagement.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _assets;

        public CatalogController(ILibraryAsset assets)
        {
            _assets = assets;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id),
                    Type = _assets.GetType(result.Id)
                });

            var model = new AssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);

        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetById(id);

            var model = new AssetDetailModel
            {
                AsssetId = asset.Id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                Type = _assets.GetType(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                Isbn = _assets.GetIsbn(id)
            };

            return View(model);
        }
    }
}
