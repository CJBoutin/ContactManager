using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManagerServiceLayer.DataClasses
{
    public class AdvancedContact
    {
        string FName { get; set; }
        string LName { get; set; }

        internal List<Emails> Emaillist1
        {
            get
            {
                return Emaillist;
            }

            set
            {
                Emaillist = value;
            }
        }

        internal List<PhoneNumbers> PhoneList1
        {
            get
            {
                return PhoneList;
            }

            set
            {
                PhoneList = value;
            }
        }

        internal List<Addresses> Addresslist1
        {
            get
            {
                return Addresslist;
            }

            set
            {
                Addresslist = value;
            }
        }

        List<Addresses> Addresslist = new List<Addresses>();
        List<PhoneNumbers> PhoneList = new List<PhoneNumbers>();
        List<Emails> Emaillist = new List<Emails>();
    }
}

public class Addresses
{
    string AddressType;
    int StreetNo;
    string StreetName;
    string City;
    string Province;
    string Country;

    public string AddressType1
    {
        get
        {
            return AddressType;
        }

        set
        {
            AddressType = value;
        }
    }

    public int StreetNo1
    {
        get
        {
            return StreetNo;
        }

        set
        {
            StreetNo = value;
        }
    }

    public string StreetName1
    {
        get
        {
            return StreetName;
        }

        set
        {
            StreetName = value;
        }
    }

    public string City1
    {
        get
        {
            return City;
        }

        set
        {
            City = value;
        }
    }

    public string Province1
    {
        get
        {
            return Province;
        }

        set
        {
            Province = value;
        }
    }

    public string Country1
    {
        get
        {
            return Country;
        }

        set
        {
            Country = value;
        }
    }
}

public class PhoneNumbers
{
    string PhoneType;
    int AreaCode;
    int SignificantNumber;

    public string PhoneType1
    {
        get
        {
            return PhoneType;
        }

        set
        {
            PhoneType = value;
        }
    }

    public int AreaCode1
    {
        get
        {
            return AreaCode;
        }

        set
        {
            AreaCode = value;
        }
    }

    public int SignificantNumber1
    {
        get
        {
            return SignificantNumber;
        }

        set
        {
            SignificantNumber = value;
        }
    }
}

class Emails
{
    string EmailType;
    string EmailAddress;

    public string EmailType1
    {
        get
        {
            return EmailType;
        }

        set
        {
            EmailType = value;
        }
    }

    public string EmailAddress1
    {
        get
        {
            return EmailAddress;
        }

        set
        {
            EmailAddress = value;
        }
    }
}

