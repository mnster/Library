using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LibrariaManagement.Controllers;
using LibrariaManagement.Models.Catalog;
using LibrariaManagement.Models.CheckoutModels;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using NUnit.Framework;
using LibraryServices;

namespace Library.Tests.Controllers
{
    [TestFixture]
    public class CatalogControllerShould
    {
        private static IEnumerable<LibraryAsset> GetByIdAllAssets()
        {
            return new List<LibraryAsset>
            {
                new Book
                {
                    Title = "Orlando",
                    Author = "Virginia Woolf",
                    Year = 1928,
                    Cost = 23.0M,
                    ImageUrl = "foo",
                    Status = new Status
                    {
                        Name = "Checked In",
                        Id = 1
                    }
                },

                new Video
                {
                    Title = "Happy People",
                    Director = "Werner Herzog",
                    ImageUrl = "images/sample.jpg",
                    Status = new Status
                    {
                        Name = "Lost",
                        Id = 3
                    }
                }
            };
        }

        private static Book GetByIdAsset()
        {
            return new Book
            {
                Title = "Orlando",
                Author = "Virginia Woolf",
                Status = new Status
                {
                    Name = "Checked In",
                    Id = 1
                }
            };
        }

        private static AssetHoldModel GetByIdCurrentHold()
        {
            return new AssetHoldModel
            {
                PatronName = "Foo",
                HoldPlaced = "Bar"
            };
        }

        [Test]
        public void Call_CheckInItem_In_Service_When_CheckIn_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.CheckIn(24);

            mockCheckoutService.Verify(s => s.CheckInItem(24), Times.Once());
        }


        [Test]
        public void Call_CheckoutItem_In_Service_When_PlaceCheckout_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            sut.PlaceCheckout(24, 1);

            mockCheckoutService.Verify(s => s.CheckoutItem(24, 1), Times.Once());
        }

        [Test]
        public void Call_GetById_In_Service_When_Checkout_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            sut.Checkout(24);

            mockLibraryAssetService.Verify(s => s.GetById(24), Times.Once());
        }

        [Test]
        public void Call_GetById_In_Service_When_Hold_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            sut.Hold(24);

            mockLibraryAssetService.Verify(s => s.GetById(24), Times.Once());
        }

        [Test]
        public void Call_MarkFound_In_Service_When_MarkFound_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.MarkFound(24);

            mockCheckoutService.Verify(s => s.MarkFound(24), Times.Once());
        }

        [Test]
        public void Call_MarkLost_In_Service_When_MarkLost_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.MarkLost(24);

            mockCheckoutService.Verify(s => s.MarkLost(24), Times.Once());
        }

        [Test]
        public void Call_PlaceHold_In_Service_When_PlaceHold_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            sut.PlaceHold(24, 1);

            mockCheckoutService.Verify(s => s.PlaceHold(24, 1), Times.Once());
        }

        [Test]
        public void Redirect_To_Detail_When_PlaceCheckout_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.PlaceCheckout(24, 1);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Redirect_To_Detail_When_PlaceHold_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.PlaceHold(24, 1);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Return_AssetIndexListingModel()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();

            mockLibraryAssetService.Setup(r => r.GetAll()).Returns(GetByIdAllAssets());

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Index();
            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<AssetIndexModel>();
        }

        [Test]
        public void Return_BranchDetailModel()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);
            var result = controller.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<AssetDetailModel>();
        }

        [Test]
        public void Return_Catalog_Index_View()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();

            mockLibraryAssetService.Setup(r => r.GetAll()).Returns(GetByIdAllAssets());

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Index();
            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetIndexModel>();
            viewModel.Subject.Assets.Count().Should().Be(2);
        }

        [Test]
        public void Return_CheckIn_View()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();

            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Hawkins Library"
            });

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetDetailModel>();

            viewModel.Subject.Title.Should().Be("Orlando");
        }

        [Test]
        public void Return_CheckOut_View()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.Checkout(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<CheckoutModel>();
            viewModel.Subject.Title.Should().Be("Orlando");
        }

        [Test]
        public void Return_CheckOutModel()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.Checkout(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<CheckoutModel>();
        }

        [Test]
        public void Return_CheckOutModel_When_Hold_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.Hold(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<CheckoutModel>();
        }

        [Test]
        public void Return_DetailView_When_CheckIn_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.CheckIn(24);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Return_DetailView_When_MarkFound_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.MarkFound(24);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();
            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Return_DetailView_When_MarkLost_Called()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.MarkLost(24);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Test]
        public void Return_LibraryAsset_Detail_View()
        {
            var mockLibraryAssetService = new Mock<LibraryAssetService>();
            var mockCheckoutService = new Mock<CheckoutService>();
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(GetByIdAsset());

            mockCheckoutService.Setup(r => r.GetCurrentHoldPlaced(24)).Returns(GetByIdCurrentHold().HoldPlaced);
            mockCheckoutService.Setup(r => r.GetCurrentHoldPatron(24)).Returns(GetByIdCurrentHold().PatronName);

            mockCheckoutService.Setup(r => r.GetCheckoutHistory(24)).Returns(new List<CheckoutHistory>
            {
                new CheckoutHistory()
            });

            mockLibraryAssetService.Setup(r => r.GetType(24)).Returns("Book");
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(24)).Returns(new LibraryBranch
            {
                Name = "Hawkins Library"
            });
            mockLibraryAssetService.Setup(r => r.GetAuthorOrDirector(24)).Returns("Virginia Woolf");
            mockLibraryAssetService.Setup(r => r.GetById(24)).Returns(new LibraryCard
            {
                Id = 1
            });
            mockLibraryAssetService.Setup(r => r.GetDeweyIndex(24)).Returns("ELEVEN");
            mockCheckoutService.Setup(r => r.GetCheckoutHistory(24)).Returns(new List<CheckoutHistory>
            {
                new CheckoutHistory()
            });
            mockCheckoutService.Setup(r => r.GetLatestCheckout(24)).Returns(new Checkout());
            mockCheckoutService.Setup(r => r.GetCurrentPatron(24)).Returns("NANCY");
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetDetailModel>();
            viewModel.Subject.Title.Should().Be("Orlando");
        }
    }
}