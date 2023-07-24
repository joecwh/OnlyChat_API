namespace API.Enums
{
    public enum Status
    {
        Success,
        Failed,
        NotFound,
        InternalServerError
    }

    public enum UserRole
    {
        User,
        Admin
    }

    public enum Error
    {
        USERID_NOT_FOUND,
        USER_NOT_FOUND,
        USERNAME_NOT_FOUND,
        EMAIL_NOT_FOUND,

        USERID_INVALID,
        USERNAME_INVALID,
        EMAIL_INVALID,
        USERNAME_OR_PASSWORD_INVALID,

        USERNAME_EXIST,
        EMAIL_EXIST,

        LOGIN_FAILURE,
        SIGNUP_FAILURE,
        ADD_ROLE_FAILURE
    }

    public enum Success
    {
        LOGIN_SUCCESS,
        SIGNUP_SUCCESS
    }

    public static class EnumCode
    {
        private static readonly Dictionary<Enum, string> Messages = new Dictionary<Enum, string>
        {
            //Status Code
            { Status.Success, "Success" },
            { Status.Failed, "Failed" },
            { Status.NotFound, "Not Found" },
            { Status.InternalServerError, "Internal Server Error" },


            //User Role Code
            { UserRole.User, "User Credential" },
            { UserRole.Admin, "Admin Credential" },
            

            //Error Code
            { Error.USERID_NOT_FOUND, "User ID is not found." },
            { Error.USER_NOT_FOUND, "User is not found." },
            { Error.USERNAME_NOT_FOUND, "Username is not found." },
            { Error.EMAIL_NOT_FOUND, "Email is not found." },

            { Error.USERID_INVALID, "User ID is invalid." },
            { Error.USERNAME_INVALID, "Username is invalid." },
            { Error.EMAIL_INVALID, "Email address is invalid." },
            { Error.USERNAME_OR_PASSWORD_INVALID, "Username or password is invalid." },

            { Error.USERNAME_EXIST, "Username has already existed, Please try another username." },
            { Error.EMAIL_EXIST, "Email has already existed, Please try another Email address." },

            { Error.LOGIN_FAILURE, "Login is Failure." },
            { Error.SIGNUP_FAILURE, "Sign Up is Failure." },
            { Error.ADD_ROLE_FAILURE, "Add User to Role is Failure." },

            { Success.LOGIN_SUCCESS, "Login success" },
            { Success.SIGNUP_SUCCESS, "SignUp success" }
        };

        public static string GetMessage<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            return Messages.TryGetValue(enumValue, out var message) ? message : enumValue.ToString();
        }

        public static string GetName(this Enum enumValue)
        {
            return Messages.TryGetValue(enumValue, out var message) ? message : Enum.GetName(enumValue.GetType(), enumValue);
        }
    }
}
