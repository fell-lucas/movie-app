using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.Controllers;
using MovieApp.Dtos;
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

        [Fact]
        public async Task GetUnwatchedMoviesAsync_NoParams_ReturnsMovieList()
        {
            var movieList = new Movie[] {
                CreateRandomMovie(),
                CreateRandomMovie(),
                CreateRandomMovie(),
            };
            repositoryStub
                .Setup(repo => repo.GetUnwatchedMoviesFromDbAsync())
                .ReturnsAsync(movieList);

            var controller = new MoviesController(repositoryStub.Object);

            var result = await controller.GetUnwatchedMoviesAsync();

            result.Should().Equal(movieList.Select(movie => movie.AsDto()));
        }
        [Fact]
        public async Task GetWatchedMoviesAsync_NoParams_ReturnsMovieList()
        {
            var movieList = new Movie[] {
                CreateRandomMovie(),
                CreateRandomMovie(),
                CreateRandomMovie(),
            };
            repositoryStub
                .Setup(repo => repo.GetWatchedMoviesFromDbAsync())
                .ReturnsAsync(movieList);

            var controller = new MoviesController(repositoryStub.Object);

            var result = await controller.GetWatchedMoviesAsync();

            result.Should().Equal(movieList.Select(movie => movie.AsDto()));
        }

        [Fact]
        public async Task SearchMovieAsync_WithSearch_ReturnsMovies()
        {
            var movieList = new Movie[] {
                CreateRandomMovie(),
                CreateRandomMovie(),
                CreateRandomMovie(),
            };
            var search = "abc";
            repositoryStub
                .Setup(repo => repo.SearchMoviesFromApiAsync(search))
                .ReturnsAsync(movieList);
            var controller = new MoviesController(repositoryStub.Object);

            var result = await controller.SearchMoviesAsync(search);

            result.Should().Equal(movieList.Select(movie => movie.AsSearchDto()));
        }

        [Fact]
        public async Task CreateMovieAsync_WithNoExistingMovie_ReturnsCreatedAt()
        {
            var movieToCreate = CreateRandomMovie().AsCreateMovieDto();
            repositoryStub
                .Setup(repo => repo.GetMovieFromDbAsync(It.IsAny<String>()))
                .ReturnsAsync((Movie)null);
            var movie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.SearchSingleMovieFromApiAsync(It.IsAny<String>()))
                .ReturnsAsync(movie with { ImdbId = movieToCreate.ImdbId });
            var controller = new MoviesController(repositoryStub.Object);

            var result = await controller.CreateMovieAsync(movieToCreate);

            var createdMovie = (result.Result as CreatedAtActionResult).Value as MovieDto;
            movieToCreate.Should().BeEquivalentTo(
                createdMovie,
                options => options.ComparingByMembers<MovieDto>()
                    .ExcludingMissingMembers()
            );
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
