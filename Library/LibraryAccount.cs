using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class LibraryAccount
    {
        #region private Variables

        private const int MAX_NUM_OF_LOAN_BOOKS = 3;
        private const int MAX_NUM_OF_RESERVED_BOOKS = 3;
        private Reader owner;//הקורא
        private List<Book> loanBooks;
        private List<Book> reservedBooks;
        private double ownerDebt;

        #endregion

        #region properties
        public int maxNumOfLoanBooks
        {
            get { return MAX_NUM_OF_LOAN_BOOKS; }

        }
        public int maxNumOfReservedBooks
        {
            get { return MAX_NUM_OF_RESERVED_BOOKS; }

        }
        public Reader Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        public List<Book> LoanBooks
        {
            get { return loanBooks; }
            set { loanBooks = value; }

        }
        public List<Book> ReservedBooks
        {
            get { return reservedBooks; }
            set { reservedBooks = value; }

        }
        public double OwnerDebt
        {
            get { return ownerDebt; }
            set { ownerDebt = value; }

        }
        #endregion

        #region constructors
        public LibraryAccount(Reader reader)
        {
            this.owner = reader;
            this.loanBooks = new List<Book>();
            this.reservedBooks = new List<Book>();
            this.ownerDebt = 0;//אין חוב ללקוח חדש
        }
        #endregion

        #region Methods
        public bool LoanBook(Book bookToLoan)
        {
            #region Validation according to task
            //לממש לבד:
            //Ensure that the book is in the library (according to book status)
            // or Ensure that the book is reserverd to me (according ot the content of my reserved books)
            //Ensure that this book is not in my loam book list
            //validtaion - check that the reader doesnt have more that the max number of loan books
            if (bookToLoan.Status == Book.BookStatus.OutOfTheLibraryAndReserved)
                throw new InvalidOperationException("The book are out on the library and he is reserved");


            if (bookToLoan.Status == Book.BookStatus.Reserved)
                throw new InvalidOperationException("the book are reserved");

            if (bookToLoan.Status == Book.BookStatus.OutOfTheLibrary)
                throw new InvalidOperationException("The book are not in library");

            if (loanBooks.Count == maxNumOfLoanBooks)
                throw new InvalidOperationException("Reader alrady has " + maxNumOfLoanBooks + "book (man number)");

            //validtaion - check that the current libraryAccount doesnt have any debt
            if (ownerDebt > 0)
                throw new InvalidOperationException("Reader alrady has " + ownerDebt + "NIS, so he cannot loan books");
            //לממש לבד
            //Ensure that the reader type match to the book type:
            //adult reader can only read adults books
            //child reader can only read childern books


            if (bookToLoan.Type != Book.BookType.ChildrenBook)
                if (owner.Type == Reader.ReaderType.child)
                    throw new InvalidOperationException("Child cant loan a child book");


            if (bookToLoan.Type != Book.BookType.AdultBook)
                if (owner.Type == Reader.ReaderType.adult)
                    throw new InvalidOperationException("Adult cant loan a Adult book");






            #endregion
            // if the code came to this place then we can proceed with the book loan activity
            LoanBooks.Add(bookToLoan);//add to list

            bookToLoan.Status = Book.BookStatus.OutOfTheLibrary;


            //לממש לבד עדכון סטטוס של הספר כדי לסמן שהוא נלקח
            //לממש לבד-אם הספר נמצא ברשימת הספרים המוזמנים שלי , יש להסיר אותו משם
            if (ReservedBooks.Contains(bookToLoan))
                if (ReservedBooks.Remove(bookToLoan))

                    LoanBooks.Add(bookToLoan);
            return true;

        }

        public bool ReturnBook(Book bookToReturn)
        {
            #region validation to ensure that return activity can be proceeded
            if (!loanBooks.Contains(bookToReturn))//אם הספר לא בפנים
            {
                throw new InvalidOperationException("we cannot return a book that is not our loan list books");
            }

            #endregion

            //if the code came to this place then we can proceed the return book activity
            //book status update
            if (bookToReturn.Status == Book.BookStatus.OutOfTheLibrary)//אם הוא מחוץ לסיפריה
                bookToReturn.Status = Book.BookStatus.InTheLibrary;//היה מחוץ לסיפריה וכעת הוא זמין
            else if (bookToReturn.Status == Book.BookStatus.OutOfTheLibraryAndReserved)
                bookToReturn.Status = Book.BookStatus.Reserved;

            //לממש לבד-להסיר את הספר מהרשימה של הקורא
            //(כי הוא כרגע החזיר אותו)

            loanBooks.Remove(bookToReturn);//הסרת ספר מהרשימה
            return true;
        }
        #endregion
        public bool ReservedBook(Book bookToResreve)
        {
            if (bookToResreve.Status == Book.BookStatus.OutOfTheLibraryAndReserved)
                throw new InvalidOperationException("The book are out on the library and he is reserved");


            if (bookToResreve.Status == Book.BookStatus.Reserved)
                throw new InvalidOperationException("the book are already reserved");


            if (bookToResreve.Type == Book.BookType.ChildrenBook)
                if (owner.Type != Reader.ReaderType.child)
                    throw new InvalidOperationException("Child cant loan a adult book");


            if (bookToResreve.Type == Book.BookType.AdultBook)
                if (owner.Type != Reader.ReaderType.adult)
                    throw new InvalidOperationException("Adult cant loan a child book");

            if (reservedBooks.Count == maxNumOfReservedBooks)
                throw new InvalidOperationException("Reader have reached the resereved limit");

            if (ownerDebt > 0)
                throw new InvalidOperationException("Reader alrady has " + ownerDebt + "New israeli shekels (NIS), so he cannot loan books");

            if (bookToResreve.Status == Book.BookStatus.OutOfTheLibrary)
                bookToResreve.Status = Book.BookStatus.OutOfTheLibraryAndReserved;


            if (bookToResreve.Status == Book.BookStatus.InTheLibrary)
                bookToResreve.Status = Book.BookStatus.Reserved;

            reservedBooks.Add(bookToResreve);
            return true;


        }
        public bool CancelBook(Book bookToCancel)
        {

            if (ReservedBooks.Contains(bookToCancel) == false)
                return false;

            //אם ביטלתי הזמנה אז הספר יהיה זמין בסיפריה
            if (bookToCancel.Status == Book.BookStatus.OutOfTheLibraryAndReserved)
                bookToCancel.Status = Book.BookStatus.OutOfTheLibrary;


            //אם ביטלתי ספר אז שיהיה זמין בסיפריה
            if (bookToCancel.Status == Book.BookStatus.Reserved)
                bookToCancel.Status = Book.BookStatus.InTheLibrary;

            reservedBooks.Remove(bookToCancel);

            return true;
        }
    }
}
