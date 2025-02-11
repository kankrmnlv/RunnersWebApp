using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RunnersWebApp.Controllers;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunnersWebApp.Test.ControllerTests
{
    public class ClubControllerTest
    {
        private ClubController _clubController;
        private IClubInterface _clubInterface;
        private IPhotoInterface _photoInterface;
        private IHttpContextAccessor _contextAccessor;

        public ClubControllerTest()
        {
            _clubInterface = A.Fake<IClubInterface>();
            _photoInterface = A.Fake<IPhotoInterface>();
            _contextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _clubController = new ClubController(_clubInterface, _photoInterface, _contextAccessor);
        }

        [Fact]
        public void ClubController_Index_ReturnsSuccess()
        {
            //Arrange
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(() => _clubInterface.GetAll()).Returns(clubs);

            //Act
            var result = _clubController.Index();

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void ClubController_Detail_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubInterface.GetByIdAsync(id)).Returns(club);

            //Act
            var result = _clubController.Detail(id);

            //Assert
            result.Should().BeOfType<Task<IActionResult>>();

        }
    }
}
