﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Data.Enum;
using RunnersWebApp.Models;
using RunnersWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunnersWebApp.Test.RepositoryTests
{
    public class ClubRepositoryTest
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Clubs.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Clubs.Add(
                      new Club()
                      {
                          Title = "Best Club KG",
                          Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                          Description = "Best running club ever",
                          ClubCategory = ClubCategory.City,
                          Address = new Address()
                          {
                              Street = "Ahunbaeva 23",
                              City = "Bishkek",
                              Country = "Kyrgyzstan"
                          }
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void ClubRepository_Add_ReturnsBool()
        {
            //Arrange
            var club = new Club()
            {
                Title = "BestLOOOKING",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = "Best runners of universe",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "Ahunbaeva 54",
                    City = "Bishkek",
                    Country = "Kyrgyzstan"
                }
            };

            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);
            
            //Act
            var result = clubRepository.Add(club);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClubs()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Club>>();
        }
    }
}
