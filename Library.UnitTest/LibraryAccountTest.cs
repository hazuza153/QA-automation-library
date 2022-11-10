using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.UnitTest
{
    [TestClass]
    public class LibraryAccountTest
    {
        #region my loan tests
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [TestMethod]
        [Timeout(1000)] // 1000ms = 1 sec
        public void LoanBook_ChildTriesToGetAvailableChildernBook_ReturnTrueAndUpdateList()
        {
            //To crash timeout
            for (int i = 0; i < 10000; i++) ;

            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            bool actual;


            //Act
            actual = childLibraryAccount.LoanBook(childrenBook);


            //Assert
            Assert.IsTrue(actual, "bug : a child should be able to loan a children book");
            Assert.IsTrue(childLibraryAccount.LoanBooks.Contains(childrenBook), "bug: the loan process is a succuss however list is not updateted");
            Assert.AreEqual(childrenBook.Status, Book.BookStatus.OutOfTheLibrary, "bug : the book was loan however its status was not updated");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook), "bug : the child took the book however it appears in his reservation list");


        }



        [TestMethod]
        public void LoanBook_AdultTriesToGetAvailableAdultBook_ReturnTrueAndUpdateList()
        {
            #region loan test
            //Arrange - מעדכנים את האובייקט
            Reader adultReader = new Reader("bar", "haziza", Reader.ReaderType.adult);// Initialize a adult reader 
            LibraryAccount adultLibraryAccount = new LibraryAccount(adultReader);
            Book adultBook = new Book(Book.BookType.AdultBook, Book.BookStatus.InTheLibrary);
            bool actual;


            //Act - לבצע את הבדיקה
            actual = adultLibraryAccount.LoanBook(adultBook);


            //Assert - לאמת את התוצאה, מה שאנחנו מצפים
            Assert.IsTrue(actual, "bug : a adult should be able to loan a adult books");
            Assert.IsTrue(adultLibraryAccount.LoanBooks.Contains(adultBook), "bug: the loan process is a succuss however list is not updateted");
            Assert.AreEqual(adultBook.Status, Book.BookStatus.OutOfTheLibrary, "bug : the book was loan however its status was not updated");
            Assert.AreEqual(false,adultLibraryAccount.ReservedBooks.Contains(adultBook) ,"bug : the adult took the book however it appears in his reservation list");

        }
        [TestMethod]

            public void LoanBook_ReaderTriesToLoanBook_BookStatusOutOfTheLibraryAndReserved_ThrowInvailedOperationException()
        {
            //Arrange
            Reader adultReader = new Reader("bar", "haziza", Reader.ReaderType.adult);// Initialize a adult reader 
            LibraryAccount adultLibraryAccount = new LibraryAccount(adultReader);
            Book adultBook = new Book(Book.BookType.AdultBook, Book.BookStatus.OutOfTheLibraryAndReserved);

           
            Assert.ThrowsException<System.InvalidOperationException>(() => adultLibraryAccount.LoanBook(adultBook));
            Assert.IsFalse(adultLibraryAccount.LoanBooks.Contains(adultBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }
        [TestMethod]
        public void LoanBook_ReaderTriesToLoanBook_BookStatusReserved_ThrowInvailedOperationException()
        {
            //Arrange
            Reader adultReader = new Reader("bar", "haziza", Reader.ReaderType.adult);// Initialize a child reader 
            LibraryAccount adultLibraryAccount = new LibraryAccount(adultReader);
            Book adultBook = new Book(Book.BookType.AdultBook, Book.BookStatus.Reserved);

            //assert
            Assert.ThrowsException<System.InvalidOperationException>(() => adultLibraryAccount.LoanBook(adultBook));
            Assert.IsFalse(adultLibraryAccount.LoanBooks.Contains(adultBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }


        [TestMethod]
        public void LoanBook_AdultTryGetChildBook_ThrowInvailedOperationException()
        {
            //Arrange
            Reader adultReader = new Reader("bar", "haziza", Reader.ReaderType.adult);// Initialize a adult reader 
            LibraryAccount adultLibraryAccount = new LibraryAccount(adultReader);
            Book childBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);

            //assert
            Assert.ThrowsException<System.InvalidOperationException>(() => adultLibraryAccount.LoanBook(childBook));
            Assert.IsFalse(adultLibraryAccount.LoanBooks.Contains(childBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }



        [TestMethod]
        public void LoanBook_ChildTryGetAdultBook_ThrowInvailedOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book adultBook = new Book(Book.BookType.AdultBook, Book.BookStatus.InTheLibrary);

            //assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.LoanBook(adultBook));
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(adultBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }


        [TestMethod]
        public void LoanBook_TryToGetBookReachedLimit_ThrowInvailedOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);

            Book Dora = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary); //יצירת ספר
            childLibraryAccount.LoanBooks.Add(Dora); //מוסיף ספר
            Book RedCape = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            childLibraryAccount.LoanBooks.Add(RedCape);
            Book Shilgiya = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            childLibraryAccount.LoanBooks.Add(Shilgiya);
            // פה הקורא הגיע כבר למקסימום כי הוא כבר השאיל 3 ספרים מקודם
            Book Witcher = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);// יוצרים את הספר ה4

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.LoanBook(Witcher));
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(Witcher));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }

        [TestMethod]
        public void LoanBook_TryToGetBookHasDebt_ThrowInvailedOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);

            childLibraryAccount.OwnerDebt = 20; //חוב של 20 שח

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.LoanBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(childrenBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }

        #endregion



        [TestMethod]
        public void ReservedBook_AlreadyReservedBook_ReturnExceptionAndNotUpdateList()
        {
            // Assert.ThrowsException<InvalidCastException>(() => LibraryAccoun.ReserveBook(book));
            // Assert.IsFalse(libraryAccount.LoanBook)
        }



        #endregion



        #region my return tests

        [TestMethod]
        public void ReturnBook_TriesToReturnBookAlreadyTakenAndResrevedHappyPath_ReturnTrueAndUpdateList()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.OutOfTheLibraryAndReserved);
            childLibraryAccount.LoanBooks.Add(childrenBook);
            
            bool actual;


            //Act

            actual = childLibraryAccount.ReturnBook(childrenBook);



            //Assert
            Assert.IsTrue(actual, "bug : a child should be able to loan a children book");
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(childrenBook), "bug: the loan process is a succuss however list is not updateted");
            Assert.AreEqual(childrenBook.Status, Book.BookStatus.Reserved, "bug : the book was loan however its status was not updated");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook), "bug : the child took the book however it appears in his reservation list");


        }

        [TestMethod]
        public void ReturnBook_TriesToReturnBookHappyPath_ReturnTrueAndUpdateList()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            childLibraryAccount.LoanBook(childrenBook);
            bool actual;


            //Act

            actual = childLibraryAccount.ReturnBook(childrenBook);



            //Assert
            Assert.IsTrue(actual, "bug : a child should be able to loan a children book");
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(childrenBook), "bug: the loan process is a succuss however list is not updateted");
            Assert.AreEqual(childrenBook.Status, Book.BookStatus.InTheLibrary, "bug : the book was loan however its status was not updated");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook), "bug : the child took the book however it appears in his reservation list");


        }


        [TestMethod]
        public void ReturnBook_TryToReturnNotOnReaderList_ThrowInvailedOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);



            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReturnBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(childrenBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }



        #endregion


        #region my resreved Tests

        [TestMethod]
        public void ResrevedBook_TryToResrevedHappyPath_ReturnTrueAndUpdateList()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            bool actual;
            //Act
            actual = childLibraryAccount.ReservedBook(childrenBook);

            //Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(childLibraryAccount.ReservedBooks.Contains(childrenBook));
        }

        // tirgul start
        [TestMethod]
        public void LoanBook_TryToLoanChildBook_ReturnTrueAndUpdateList()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount libraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            bool actual;
            //Act
            actual = libraryAccount.LoanBook(childrenBook);

            //Assert
            Assert.IsTrue(actual);
            Assert.IsTrue(libraryAccount.LoanBooks.Contains(childrenBook));

        }
        //tirgul End

        // Request
        [TestMethod]
        public void ResrevedBook_TryToReserve_BookIsAlreadyReserved_ThrowInvalidOpeartionException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.Reserved);
            bool actual;


            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReservedBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook));

            //Assert.AreEqual(childLibraryAccount.ReservedBooks.Contains(childrenBook), false);
        }
        //  Request end

        [TestMethod]
        public void ReserveBook_TryToResreve_BookAlreadyReserved_ThrowInvalidOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.Reserved);

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReservedBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook));
        }

        [TestMethod]
        public void ReserveBook_TryToResreve_BookAlreadyTakenAndReserved_ThrowInvalidOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.OutOfTheLibraryAndReserved);

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReservedBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook));
        }

        [TestMethod]
        public void ReserveBook_AdultTryToResreveChildBook_ThrowInvalidOperationException()
        {
            //Arrange
            Reader adultReader = new Reader("bar", "haziza", Reader.ReaderType.adult);
            LibraryAccount adultLibraryAccount = new LibraryAccount(adultReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => adultLibraryAccount.ReservedBook(childrenBook));
            Assert.IsFalse(adultLibraryAccount.ReservedBooks.Contains(childrenBook));
        }

        [TestMethod]
        public void ReserveBook_ChildTryToResreveAdultBook_ThrowInvalidOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book adultBook = new Book(Book.BookType.AdultBook, Book.BookStatus.InTheLibrary);

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReservedBook(adultBook));
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(adultBook));
        }



        [TestMethod]
        public void ReserveBook_TryToResreve_ReaderReachedLimit_ThrowInvalidOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);

            Book Dora = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary); //יצירת ספר
            childLibraryAccount.ReservedBooks.Add(Dora); //מוסיף ספר
            Book RedCape = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            childLibraryAccount.ReservedBooks.Add(RedCape);
            Book Shilgiya = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);
            childLibraryAccount.ReservedBooks.Add(Shilgiya);
            // פה הקורא הגיע כבר למקסימום כי הוא כבר השאיל 3 ספרים מקודם
            Book Witcher = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);// יוצרים את הספר ה4

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReservedBook(Witcher));
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(Witcher));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא
        }

        [TestMethod]
        public void ReserveBook_TryToResreveWithDebt_ThrowInvalidOperationException()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book childrenBook = new Book(Book.BookType.ChildrenBook, Book.BookStatus.InTheLibrary);

            childLibraryAccount.OwnerDebt = 20; //חוב של 20 שח

            //Assert
            Assert.ThrowsException<System.InvalidOperationException>(() => childLibraryAccount.ReservedBook(childrenBook));
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(childrenBook));//בודק שהספר לא נכנס לרשימת הספרים המושאלים של הקורא

        }
        #endregion


        #region my Cancel tests
        [TestMethod]
        public void CancelBook_TryToCancel_ReturnTrue()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book bookToCancel = new Book(Book.BookType.ChildrenBook, Book.BookStatus.Reserved);
            bool actual;

            childLibraryAccount.ReservedBooks.Add(bookToCancel);

            //Act
            actual = childLibraryAccount.CancelBook(bookToCancel);


            //Assert
            Assert.IsTrue(actual, "bug : reader should be able to Cancel a book");
            Assert.IsFalse(childLibraryAccount.LoanBooks.Contains(bookToCancel), "bug: the loan process is a succuss however list is not updateted");
            Assert.AreEqual(bookToCancel.Status, Book.BookStatus.InTheLibrary, "bug : the book was loan however its status was not updated");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(bookToCancel), "bug : the child took the book however it appears in his reservation list");


        }

        [TestMethod]
        public void CancelBook_updateBookToOutOfLibrary_ReturnTrue()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book bookToCancel = new Book(Book.BookType.ChildrenBook, Book.BookStatus.OutOfTheLibrary);
            childLibraryAccount.ReservedBooks.Add(bookToCancel);
            bool actual;


            //Act
            actual = childLibraryAccount.CancelBook(bookToCancel);


            //Assert
            Assert.IsTrue(actual, "bug : a child should be able to loan a children book");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(bookToCancel), "bug: the loan process is a succuss however list is not updateted");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(bookToCancel), "bug : the child took the book however it appears in his reservation list");


        }


        [TestMethod]
        public void CancelBook_updateBookToInLibrary_ReturnTrue()
        {
            //Arrange
            Reader childReader = new Reader("bar", "haziza", Reader.ReaderType.child);// Initialize a child reader 
            LibraryAccount childLibraryAccount = new LibraryAccount(childReader);
            Book bookToCancel = new Book(Book.BookType.ChildrenBook, Book.BookStatus.Reserved);
            childLibraryAccount.ReservedBooks.Add(bookToCancel);
            bool actual;


            //Act
            actual = childLibraryAccount.CancelBook(bookToCancel);


            //Assert
            Assert.IsTrue(actual, "bug : a child should be able to loan a children book");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(bookToCancel), "bug: the loan process is a succuss however list is not updateted");
            Assert.AreEqual(bookToCancel.Status, Book.BookStatus.InTheLibrary, "bug : the book was loan however its status was not updated");
            Assert.IsFalse(childLibraryAccount.ReservedBooks.Contains(bookToCancel), "bug : the child took the book however it appears in his reservation list");


        }



        #endregion


        #region my xml tests

        private TestContext context;
        public TestContext TestContext
        {
            get { return context; }
            set { context = value; }
        }


        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
            @"|DataDirectory|\TestData\Tests.xml",
            "MyLibryaryTests",
            DataAccessMethod.Sequential)]

        public void CancelBook_to_test_from_XML()
        {
            //Arrange
            string ReaderType = TestContext.DataRow["ReaderType"].ToString();
            string BookType = TestContext.DataRow["BookType"].ToString();
            string BookStatus = TestContext.DataRow["BookStatus"].ToString();
            string expectedResult = TestContext.DataRow["ExpectedResult"].ToString();
            bool hasBookInReserved = Convert.ToBoolean(TestContext.DataRow["ReaderHasBookInReservedList"]);

            Book.BookStatus myBookStatus;
            Book.BookType myBookType;
            Reader.ReaderType myReaderType;

            if (ReaderType == "0") myReaderType = Reader.ReaderType.adult;
            else myReaderType = Reader.ReaderType.child;

            if (BookType == "0") myBookType = Book.BookType.AdultBook;
            else myBookType = Book.BookType.ChildrenBook;

            if (BookStatus == "0") myBookStatus = Book.BookStatus.InTheLibrary;
            else if (BookStatus == "1") myBookStatus = Book.BookStatus.Reserved;
            else if (BookStatus == "2") myBookStatus = Book.BookStatus.OutOfTheLibrary;
            else if (BookStatus == "3") myBookStatus = Book.BookStatus.OutOfTheLibraryAndReserved;
            else throw new ArgumentException("no status found");

            Reader reader = new Reader("Bar", "Haziza", myReaderType);
            LibraryAccount libraryAccount = new LibraryAccount(reader);
            Book book = new Book(myBookType, myBookStatus);

            //Assert
            if (hasBookInReserved == true)
            {
                libraryAccount.ReservedBooks.Add(book);
                bool actual = libraryAccount.CancelBook(book);
                Assert.AreEqual(actual, Convert.ToBoolean(expectedResult));
            }

            else
            {
                bool actual = libraryAccount.CancelBook(book);
                Assert.AreEqual(actual, Convert.ToBoolean(expectedResult));
            }


        }
        #endregion


        #region my data-row tests

        [TestMethod]

        //Test1
        [DataRow
            //Starts
            (Reader.ReaderType.adult,
            Book.BookStatus.Reserved,
            Book.BookType.AdultBook,
            true,
            true,
            DisplayName = "datarow_cancelbook_happypath_return_true")
            //Ends
            ]


        //Test2
        [DataRow
            //Starts
            (Reader.ReaderType.adult,
            Book.BookStatus.InTheLibrary,
            Book.BookType.AdultBook,
            false,
            false,
           DisplayName = "datarow_cancelbook_readerNotOwnsBook_return_false")
            //Ends
            ]

        public void CancelBookDatarow(Reader.ReaderType readerType,
            Book.BookStatus bookStatus,
            Book.BookType bookType,
            bool readerOwnsBook,
            bool expected)
        {
            //Arrange
            Reader reader = new Reader("bar", "haziza", readerType);
            LibraryAccount libraryAccount = new LibraryAccount(reader);
            Book book = new Book(bookType, bookStatus);

            if (readerOwnsBook == true)
                libraryAccount.ReservedBooks.Add(book);

            if (expected == true)
            {
                bool actual = libraryAccount.CancelBook(book);
                Assert.IsTrue(actual);
                Assert.IsFalse(libraryAccount.ReservedBooks.Contains(book));
                return;
            }

            else
            {
                Assert.IsFalse(libraryAccount.CancelBook(book));
                return;
            }


        }

        #endregion

    }
}


       