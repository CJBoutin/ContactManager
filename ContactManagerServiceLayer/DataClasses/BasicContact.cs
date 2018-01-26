using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManagerServiceLayer.DataClasses
{
    public class BasicContact
    {
        string _firstName;
        string _lastName;
        int _id;

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
    }
}