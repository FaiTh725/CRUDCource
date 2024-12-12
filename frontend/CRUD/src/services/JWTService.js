import { jwtDecode } from "jwt-decode";

const decodeJWT = (token) => {
  if(token)
    {
      try
      {
        var data = jwtDecode(token);
    
        const role = data["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        const email = data["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
        return {
          role: role,
          email: email
        };
      }
      catch
      {
        return null;
      }
    }
  
    return null;
}

export default decodeJWT;