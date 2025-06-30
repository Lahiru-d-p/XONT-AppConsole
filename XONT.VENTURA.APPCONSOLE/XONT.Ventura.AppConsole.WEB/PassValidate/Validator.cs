using System;
using System.Collections.Generic;
using XONT.Common.Data;
using XONT.Common.Message;
using XONT.Ventura.AppConsole;
using XONT.Ventura.AppConsole.Domain;

namespace UserCreation.Common
{
    public class Validator
    {
        private bool _isSpecialVal;

        public bool SpecialcaractorVal(string password, int countSpe)
        {
            _isSpecialVal = false;

            int countSpecialCaractor = 0;

            foreach (char c in password)
            {
                int caractorval = Convert.ToInt32(c);


                if (((caractorval >= 33) && (caractorval <= 47)) ||
                    ((caractorval >= 58) && (caractorval <= 64)) ||
                    ((caractorval >= 91) && (caractorval <= 96)) ||
                    ((caractorval >= 123) && (caractorval <= 126)))
                {
                    countSpecialCaractor++;
                }
            }

            if ((countSpecialCaractor >= countSpe))
            {
                _isSpecialVal = true;
            }

            return _isSpecialVal;
        }

        public bool RangeVal(string password, int min, int max)
        {
            _isSpecialVal = false;
            if ((password.Length >= min) && (password.Length <= max))
                _isSpecialVal = true;
            return _isSpecialVal;
        }

        public bool CheckWithPassword(string oldpassword, string newpassword, string businessUnit)
        {
            _isSpecialVal = false;
            var stroEncript = new StroEncript();
            newpassword = stroEncript.Encript(newpassword);

            if (oldpassword.Equals(newpassword))
                _isSpecialVal = true;
            return _isSpecialVal;
        }

        public bool CheckWithAllPassword(List<string> oldpasswordlist, string newpassword, string businessUnit)
        {
            _isSpecialVal = true;
            var stroencript = new StroEncript();
            string newpasswords = stroencript.Encript(newpassword);

            foreach (string s in oldpasswordlist)
            {
                if (newpasswords.Equals(s))
                {
                    _isSpecialVal = false;
                }
            }

            return _isSpecialVal;
        }

        public string ConvertPass(string newpassword)
        {
            var stroencript = new StroEncript();
            string newpasswords = stroencript.Encript(newpassword);
            return newpasswords;
        }

        public void ValidateUserPassword(User user, string oldPass, string newPass, char changeType,
                                         ref string errorMsg, ref MessageSet _msg)
        {
            ValidateUserPasswordData(user, oldPass, newPass, changeType,
                                     ref errorMsg, ref _msg)
                ;
        }

        private void ValidateUserPasswordData(User user, string oldPass, string newPass, char changeType,
                                              ref string errorMsg, ref MessageSet _msg)
        {
            try
            {
                Password _password;

                Validator _validator;
                IUserDAO userDao;
                userDao = new UserManager();
                _validator = new Validator();
                var oldpsslists = new List<string>();


                //Load Password Data
                _password = new Password();
                userDao.GetPassword(user.BusinessUnit, ref _password, ref _msg);
                if (_msg != null)
                {
                    return;
                }
                //Password Check with Old Password with user Password
                bool checkoldpass =
                    _validator.CheckWithPassword(user.Password,
                                                 oldPass,
                                                 user.BusinessUnit.Trim());
                if (checkoldpass)
                {
                    //Validate PasswordLength
                    bool checkrange = _validator.RangeVal(newPass, Convert.ToInt32(_password.MinLength),
                                                          Convert.ToInt32(_password.MaxLength));

                    if (checkrange)
                    {
                        //Validate Password Special Charactors
                        bool checkspecial = _validator.SpecialcaractorVal(newPass,
                                                                          Convert.ToInt32(
                                                                              _password.NoOfSpecialCharacters));

                        //password range check
                        if (checkspecial)
                        {
                            //password check with old change passwords values
                            userDao.GetPasswordHistoryList(user.UserName,
                                                           _password.ReusePeriod.ToString(), ref oldpsslists, ref _msg);
                            if (_msg != null)
                            {
                                return;
                            }

                            bool checkallpass =
                                _validator.CheckWithAllPassword(oldpsslists, newPass,
                                                                user.BusinessUnit.Trim());

                            //V2004
                            if (checkallpass)
                            {
                                if (changeType == '1')
                                {
                                    userDao.SaveChangePassword(user.UserName, _validator.ConvertPass(newPass),"0" 
                                                               , ref _msg);
                                }
                                if (_msg != null)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                errorMsg = "Plese Enter Different Password";
                            }
                        }
                        else //-----------------------old password correct
                        {
                            errorMsg = "Please Enter " + _password.NoOfSpecialCharacters + " Special Character"; //VR006
                        }
                    }
                    else //Check with old pssword
                    {
                        errorMsg = "Plese Enter Range Between" + _password.MinLength + " To" +
                                   _password.MaxLength;
                    }
                }
                else //password range ckeck
                {
                    errorMsg = "Password Mismatch";
                }
            }
            catch (Exception ex)
            {
                _msg = MessageCreate.CreateErrorMessage(0, ex, "ValidateUserPassword", "XONT.Ventura.AppConsole.WEB.dll");

                return;
            }
        }

        //VR005 Added
        public void SetUserPassword(User user, string newPass, char changeType,
                         ref string errorMsg, ref MessageSet _msg)
        {
            SetUserPasswordData(user, newPass, changeType,
                                     ref errorMsg, ref _msg)
                ;
        }
        private void SetUserPasswordData(User user, string newPass, char changeType,
                                      ref string errorMsg, ref MessageSet _msg)
        {
            try
            {
                Password _password;

                Validator _validator;
                IUserDAO userDao;
                userDao = new UserManager();
                _validator = new Validator();
                var oldpsslists = new List<string>();


                //Load Password Data
                _password = new Password();
                userDao.GetPassword(user.BusinessUnit, ref _password, ref _msg);
                if (_msg != null)
                {
                    return;
                }

                //Validate PasswordLength
                bool checkrange = _validator.RangeVal(newPass, Convert.ToInt32(_password.MinLength),
                                                      Convert.ToInt32(_password.MaxLength));

                if (checkrange)
                {
                    //Validate Password Special Charactors
                    bool checkspecial = _validator.SpecialcaractorVal(newPass,
                                                                      Convert.ToInt32(
                                                                          _password.NoOfSpecialCharacters));

                    //password range check
                    if (checkspecial)
                    {
                        //password check with old change passwords values
                        userDao.GetPasswordHistoryList(user.UserName,
                                                       _password.ReusePeriod.ToString(), ref oldpsslists, ref _msg);
                        if (_msg != null)
                        {
                            return;
                        }

                        bool checkallpass =
                            _validator.CheckWithAllPassword(oldpsslists, newPass,
                                                            user.BusinessUnit.Trim());

                        //V2004
                        if (checkallpass)
                        {
                            if (changeType == '1')
                            {
                                userDao.SaveChangePassword(user.UserName, _validator.ConvertPass(newPass),"0"
                                                           , ref _msg);
                            }
                            if (_msg != null)
                            {
                                return;
                            }
                        }
                        else
                        {
                            errorMsg = "Plese Enter Different Password";
                        }
                    }
                    else //-----------------------old password correct
                    {
                        errorMsg = "Plese Enter " + _password.NoOfSpecialCharacters + " Special Caraters";
                    }
                }
                else //Check with old pssword
                {
                    errorMsg = "Plese Enter Range Between" + _password.MinLength + " To" +
                               _password.MaxLength;
                }

            }
            catch (Exception ex)
            {
                _msg = MessageCreate.CreateErrorMessage(0, ex, "ValidateUserPassword", "XONT.Ventura.AppConsole.WEB.dll");

                return;
            }
        }
        //VR005

    }
}