using System;
using System.Collections.Generic;
using System.Text;

namespace PContextus.Core.Helpers
{
    public static class StringExtension
    {

        public static  bool IsNullOrEmpty(this string text) {

            if (text != null && !text.Equals(string.Empty)) {
                return false;
            }

            return true;
        }
    }
}
