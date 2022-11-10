using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public  class Book
    {
        #region Enum
        // A Book can be defined as a children book or an adult book
        public enum BookType
        {
            ChildrenBook,
            AdultBook

        }
        //the book can be in different statuses
        public enum BookStatus
        {
            InTheLibrary, //על המדף
            Reserved,//מוזמן
            OutOfTheLibrary,//מחוץ לסיפריה מישהו השאיל
            OutOfTheLibraryAndReserved,//מחוץ לסיפריה כי מישהו השאיל ובינתיים מישהו אחר הזמין את הספר

        }
       
        #endregion

        #region private Variable
        private String id; // תעודת זהות
        private BookType type; //סוג הספר
        private BookStatus status;// מצב הספר
        #endregion
        #region Propreties
        public String Id
        {
            get => id;

        }
        //ספר שמיועד לילדים ואם לא כתוב ספציפית לילדים אז הוא למבוגרים
        public BookType Type
        {
            get => type;
            set { type = value; }
        }
        public BookStatus Status
        {
            get { return status; }
            set { status = value; }
        }
        

        #endregion
        #region Constructors
        public Book()
        {
            Guid guid = Guid.NewGuid();
            id = guid.ToString();
            type = BookType.AdultBook;
            status = BookStatus.InTheLibrary;

        }
        public Book(BookType type,BookStatus status)
        {
            Guid guid = Guid.NewGuid(); // ספר ייחודי 
            id = guid.ToString();
            this.type = type;
            this.status = status;
        }
            #endregion
        }
}
