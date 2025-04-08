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
            _bookService.Setup(b => b.AddBook(It.IsAny<Book>()))
                .Callback<Book>((b) => 
                {
                     
                    throw new  Exception();
                }
             );
            //Act 
            //Assert
            Assert.Throws<ISBNDuplicateException>(() => _bookController.RegisterBook(monLivre));
            _bookService.Verify(r => r.AddBook(It.IsAny<Book>()), Times.Once);


        }
    }
}