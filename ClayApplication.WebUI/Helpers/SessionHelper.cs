using ClayApplication.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClayApplication.WebUI.Helpers
{
    public static class SessionHelper
    {
        private const string CurrentSessionName = "currentUser";
        
        public static UserViewModel CurrentUserViewModel
        {
            get
            {
                UserViewModel currentUserViewModel = null;
                try
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session[CurrentSessionName] != null)
                        {
                            currentUserViewModel = (UserViewModel)HttpContext.Current.Session[CurrentSessionName];
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                return currentUserViewModel;
            }
            set
            {
                try
                {
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session[CurrentSessionName] = value;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }
    }
}