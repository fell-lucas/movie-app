using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.Controllers;
using MovieApp.Entities;
using MovieApp.Repositories;
using Xunit;

namespace MovieApp.Tests
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMoviesRepository> repositoryStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetMovieAsync_WithUnexistingItem_ReturnsNotFound()
        {
            repositoryStub
                .Setup(repo => repo.GetMovieFromDbAsync(It.IsAny<String>()))
                .ReturnsAsync((Movie)null);

            var controller = new MoviesController(repositoryStub.Object);

            var result = await controller.GetMovieAsync("abc");

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetMovieAsync_WithExistingItem_ReturnsExpectedMovie()
        {
            // Arrange
            var expectedMovie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.GetMovieFromDbAsync(It.IsAny<String>()))
                .ReturnsAsync(expectedMovie);
            var controller = new MoviesController(repositoryStub.Object);
            // Act
            var result = await controller.GetMovieAsync(rand.Next(1000).ToString());
            // Assert
            result.Value.Should().BeEquivalentTo(expectedMovie);
        }

        [Fact]
        public async Task GetMoviesAsync_NoParams_ReturnsMovieList()
        {
            var movieList = new Movie[] {
                CreateRandomMovie(),
                CreateRandomMovie(),
                CreateRandomMovie(),
            };
            repositoryStub
                .Setup(repo => repo.GetAllMoviesFromDbAsync())
                .ReturnsAsync(movieList);

            var controller = new MoviesController(repositoryStub.Object);

            var result = await controller.GetAllMoviesAsync();

            result.Should().Equal(movieList.Select(movie => movie.AsDto()));
        }

        private Movie CreateRandomMovie()
        {
            var str = rand.Next(1000).ToString();
            return new Movie
            {
                Id = Guid.NewGuid(),
                Description = str,
                Image = str,
                ImdbId = str,
                Title = str,
                Watched = rand.Next(2) == 0
            };
        }
    }
}
