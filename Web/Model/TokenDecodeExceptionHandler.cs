using System;
using System.Collections.Generic;

namespace Web.Model {
    public class TokenDecodeExceptionHandler {   

        public static List<string> HandleJwtDecode(Action jwtDecodeAction) {
            var errors = new List<string>();
            try {
                jwtDecodeAction();
            }
            catch (Exception e) {
                errors.Add(e.ToString());
            }
            return errors;
        }
    }
}