using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Library.UnitTest
{
    [TestFixture]
    class LibraryAccount_NUNIT_Tests
    {
        [Test]
        public void ReturnBook_ChildTriesToReturnBook_HappyPath_ReturnsTrueAndUpdateList()
        {
            //arrange
            Reader childReader = new Reader("Bar", "Haziza", Reader.ReaderType.child); 
            LibraryAccount libraryAccount = new LibraryAccount(childReader); 
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            bool actual;
            libraryAccount.LoanBook(childrenBook);

            //act 
            actual = libraryAccount.ReturnBook(childrenBook);

            //assert
            Assert.That(actual, Is.EqualTo(true));
            Assert.That(libraryAccount.LoanBooks.Contains(childrenBook), Is.False);
            Assert.That(childrenBook.Status, Is.EqualTo(Book.BookStatus.InTheLibrary));
        }

        [Test]
        public void ReturnBook_TriesToReturn_BookNotOnAccountBorrowedList_ThrowInvalidOperationException()
        {
            //arrange
            Reader childReader = new Reader("Bar", "Haziza", Reader.ReaderType.child);
            LibraryAccount libraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            bool actual;

            //act & assert
            Assert.Throws<InvalidOperationException>(() => libraryAccount.ReturnBook(childrenBook));
            Assert.That(libraryAccount.LoanBooks.Contains(childrenBook), Is.False);
        }

        [Test] 
        public void LoanBook_AdultTriesToLoan_ChildBook_ThrowInvalidOperationException()
        {
            //Arrange
            Reader adultReader = new Reader("Bar", "Haziza", Reader.ReaderType.adult); 
            LibraryAccount childLibraryAccount = new LibraryAccount(adultReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);

            //act & assert
            Assert.Throws<InvalidOperationException>(() => childLibraryAccount.LoanBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(childrenBook));

        }

    }
}
