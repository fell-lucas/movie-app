using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.Controllers;
using MovieApp.Dtos.Movie;
using MovieApp.Entities;
using MovieApp.Repositories;
using MovieApp.Services;
using Xunit;

namespace MovieApp.Tests
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMoviesRepository> repositoryStub = new();
        private readonly Mock<IImdbSearchService> imdbSearchServiceStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetMovieAsync_WithUnexistingItem_ReturnsNotFound()
        {
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync((Movie)null);

            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.GetMovieAsync("abc");

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetMovieAsync_WithExistingItem_ReturnsExpectedMovie()
        {
            // Arrange
            var expectedMovie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync(expectedMovie);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);
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
                .Setup(repo => repo.GetAllMoviesAsync())
                .ReturnsAsync(movieList);

            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

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
                .Setup(repo => repo.GetUnwatchedMoviesAsync())
                .ReturnsAsync(movieList);

            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

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
                .Setup(repo => repo.GetWatchedMoviesAsync())
                .ReturnsAsync(movieList);

            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

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
            imdbSearchServiceStub
                .Setup(stub => stub.SearchMoviesFromApiAsync(search))
                .ReturnsAsync(movieList);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.SearchMoviesAsync(search);

            result.Should().Equal(movieList.Select(movie => movie.AsSearchDto()));
        }

        [Fact]
        public async Task CreateMovieAsync_WithNoExistingMovie_ReturnsCreatedAt()
        {
            var movieToCreate = CreateRandomMovie().AsCreateMovieDto();
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync((Movie)null);
            var movie = CreateRandomMovie();
            imdbSearchServiceStub
                .Setup(stub => stub.SearchSingleMovieFromApiAsync(It.IsAny<String>()))
                .ReturnsAsync(movie with { ImdbId = movieToCreate.ImdbId });
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.CreateMovieAsync(movieToCreate);

            var createdMovie = (result.Result as CreatedAtActionResult).Value as MovieDto;
            movieToCreate.Should().BeEquivalentTo(
                createdMovie,
                options => options.ComparingByMembers<MovieDto>()
                    .ExcludingMissingMembers()
            );
        }

        [Fact]
        public async Task CreateMovieAsync_WithExistingMovie_ReturnsConflict()
        {
            var movieToCreate = CreateRandomMovie().AsCreateMovieDto();
            var existingMovie = CreateRandomMovie() with { ImdbId = movieToCreate.ImdbId };
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync(existingMovie);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.CreateMovieAsync(movieToCreate);

            result.Result.Should().BeOfType<ConflictResult>();
        }

        [Fact]
        public async Task UpdateMovieAsync_WithExistingItem_ReturnsNoContent()
        {
            var existingMovie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync(existingMovie);
            var updatedMovie = new UpdateMovieDto(rand.Next(2) == 0);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.UpdateMovieAsync(existingMovie.ImdbId, updatedMovie);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateMovieAsync_WithNonExistingMovie_ReturnsNotFound()
        {
            var existingMovie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync((Movie)null);
            var updatedMovie = new UpdateMovieDto(rand.Next(2) == 0);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.UpdateMovieAsync(existingMovie.ImdbId, updatedMovie);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteMovieAsync_WithExistingMovie_ReturnsNoContent()
        {
            var existingMovie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync(existingMovie);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.DeleteMovieAsync(existingMovie.ImdbId);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteMovieAsync_WithNonExistingMovie_ReturnsNotFound()
        {
            var existingMovie = CreateRandomMovie();
            repositoryStub
                .Setup(repo => repo.GetMovieAsync(It.IsAny<String>()))
                .ReturnsAsync((Movie)null);
            var controller = new MoviesController(repositoryStub.Object, imdbSearchServiceStub.Object);

            var result = await controller.DeleteMovieAsync(existingMovie.ImdbId);

            result.Should().BeOfType<NotFoundResult>();
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
