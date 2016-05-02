using System;
using Core.SDK.Dom;

namespace EntityListTest {
    public class Person : EntityBase<Person>
    {
        string firstName;
        string secondName;
        string comments;
        public Person(string firstName, string secondName) {
            this.firstName = firstName;
            this.secondName = secondName;
            comments = String.Empty;
        }
        public Person(string firstName, string secondName, string comments)
            : this(firstName, secondName) {
            this.comments = comments;
        }
        public string FirstName {
            get { return firstName; }
            set { firstName = value; }
        }
        public string SecondName {
            get { return secondName; }
            set { secondName = value; }
        }
        public string Comments {
            get { return comments; }
            set { comments = value; }
        }

        public override Person Clone()
        {
            Person app = new Person("", "");            
            return app;
        }

        public override bool Equals(Person other)
        {
            if (string.Equals(firstName, other.FirstName)) return true;
            else return false;
        }

        

        public override bool Equals(object obj)
        {
            Person app = obj as Person;
            if (app == null) return false;
            else return Equals(app);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return FirstName;
        }
    }
}