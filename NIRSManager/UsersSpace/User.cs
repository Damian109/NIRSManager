using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSManager.UsersSpace
{
    internal sealed class User
    {
        #region Private
        private string _name;
        private string _surname;
        private string _secondName;
        private DateTime _dateOfBirth;
        private string _position;
        #endregion

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string SurName
        {
            get { return _surname; }
            set { _surname = value; }
        }
        public string SecondName
        {
            get { return _secondName; }
            set { _secondName = value; }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


    }
}
