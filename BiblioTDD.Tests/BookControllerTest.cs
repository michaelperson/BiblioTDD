using BiblioTDD.Models.Domain;
using System.ComponentModel;
using Moq;
using BiblioTDD.Models.Interfaces;
using BiblioTDD.app;
using BiblioTDD.Models.Exceptions;
namespace BiblioTDD.Tests
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _bookService;
        private readonly BookController _bookController;

        public BookControllerTest()
        {
            _bookService = new();
            _bookController = new BookController(_bookService.Object);
        }

        [Fact]
        [Description("BK-01 : Ajout d'un livre avec toutes les informations valides")]
        public void AddBookWithValidData()
        {
            //Arrange
            Book monLivre = new Book()
            {
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350884",
                Genre = "Programming",
                Year = 2008,
                Copies = 3
            };           

            //Act
            bool IsSucceed = _bookController.RegisterBook(monLivre);
            //Assert
            Assert.True(IsSucceed);
            _bookService.Verify(r=>r.AddBook(It.IsAny<Book>()), Times.Once);

        }

        [Fact]
        [Description("BK-02 : Ajout d'un livre avec ISBN existant - EXCEPTION")]
        public void AddBookWithAnExistingISBNThrowException()
        {
            //Arrange
            Book monLivre = new Book()
            {
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350887",
                Genre = "Programming",
                Year = 2008,
                Copies = 3
            };

            _bookService.Setup(b => b.GetById(It.IsAny<int>())).Returns(new Book());
           
            //Act 
            //Assert
            Assert.Throws<ISBNDuplicateException>(() => _bookController.RegisterBook(monLivre));
            _bookService.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);


        }
        [Fact]
        [Description("BK-03 : Ajout d'un livre avec des champs obligatoires manquants - EXCEPTION")]
        public void AddBookWithEmptyTitleThrowException()
        {
            //Arrange
            Book monLivre = new Book()
            {
                Title = "",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350887",
                Genre = "Programming",
                Year = 2008,
                Copies = 3
            };
            
            //Act 
            //Assert
            Assert.Throws<MandatoryFieldMissingException>(() => _bookController.RegisterBook(monLivre));
            _bookService.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);


        }
        [Fact]
        [Description("BK-03 : Ajout d'un livre avec des champs obligatoires manquants - EXCEPTION")]
        public void AddBookWithEmptyISBNThrowException()
        {
            //Arrange
            Book monLivre = new Book()
            {
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "",
                Genre = "Programming",
                Year = 2008,
                Copies = 3
            }; 
            //Act 
            //Assert
            Assert.Throws<MandatoryFieldMissingException>(() => _bookController.RegisterBook(monLivre));
            _bookService.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);


        }
        [Fact]
        [Description("BK-04 : Ajout d'un livre avec un nombre de copies négatif - EXCEPTION")]
        public void AddBookWithNegativeCopiesThrowsException()
        {
            // Arrange
            Book monLivre = new Book()
            {
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350884",
                Genre = "Programming",
                Year = 2008,
                Copies = -1
            };

            // Act & Assert
            Assert.Throws<NegativeCopiesException>(() => _bookController.RegisterBook(monLivre));
            _bookService.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Never);
        }
        [Fact]
        [Description("BK-05 : Mise à jour des informations d'un livre existant")]
        public void UpdateBookTitleSuccessfully()
        {
            // Arrange
            Book originalBook = new Book
            {
                BookId = 1,
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350884",
                Year = 2008,
                Genre = "Programming",
                Copies = 3
            };

            Book updatedBook = new Book
            {
                BookId = 1,
                Title = "Clean Coder", // le nouveau titre
                Auteur = "Robert C. Martin",
                ISBN = "9780132350884",
                Year = 2008,
                Genre = "Programming",
                Copies = 3
            };

            _bookService.Setup(s => s.GetById(originalBook.BookId)).Returns(originalBook);

            // Act
            bool result = _bookController.UpdateBook(updatedBook);

            // Assert
            Assert.True(result);
            _bookService.Verify(s => s.UpdateBook(updatedBook), Times.Once);
        }
        [Fact]
        [Description("BK-06 : Mise à jour du nombre de copies d'un livre existant")]
        public void UpdateBookCopiesSuccessfully()
        {
            // Arrange
            Book originalBook = new Book
            {
                BookId = 1,
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350884",
                Year = 2008,
                Genre = "Programming",
                Copies = 3
            };

            Book updatedBook = new Book
            {
                BookId = 1,
                Title = "Clean Code",
                Auteur = "Robert C. Martin",
                ISBN = "9780132350884",
                Year = 2008,
                Genre = "Programming",
                Copies = 5 // ajout de 2 copies
            };

            _bookService.Setup(s => s.GetById(originalBook.BookId)).Returns(originalBook);

            // Act
            bool result = _bookController.UpdateBook(updatedBook);

            // Assert
            Assert.True(result);
            _bookService.Verify(s => s.UpdateBook(updatedBook), Times.Once);
        }
        [Fact]
        [Description("BK-07 : Mise à jour d'un livre inexistant - EXCEPTION")]
        public void UpdateNonExistingBookThrowsException()
        {
            // Arrange
            Book updatedBook = new Book
            {
                BookId = 999, // ID inexistant
                Title = "Unknown Book",
                Auteur = "Nobody",
                ISBN = "0000000000000",
                Year = 2000,
                Genre = "None",
                Copies = 1
            };

            _bookService.Setup(s => s.GetById(updatedBook.BookId)).Returns((Book?)null); // Simule un livre inexistant

            // Act & Assert
            Assert.Throws<BookNotFoundException>(() => _bookController.UpdateBook(updatedBook));
            _bookService.Verify(s => s.UpdateBook(It.IsAny<Book>()), Times.Never);
        }

    }
}