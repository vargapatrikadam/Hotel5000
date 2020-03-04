﻿namespace Core.Enums
{
    public enum Errors
    {
        UNDEFINED = 1000,
        DATA_UNIQUENESS_CONFLICT = 2000,
        INVALID_PARAMETER = 3000,
        NOT_FOUND = 4000,
        UNAUTHORIZED = 5000,
        APPROVING_DATA_ALREADY_EXISTS = 10001,
        APPROVING_DATA_NOT_UNIQUE = 10002,
        APPROVING_DATA_NOT_FOUND = 10003,
        CONTACT_NOT_UNIQUE = 10004,
        CONTACT_NOT_FOUND = 10005,
        EMAIL_NOT_UNIQUE = 10006,
        USERNAME_NOT_UNIQUE = 10007,
        ROLE_NOT_EXISTS = 10008,
        USER_NOT_FOUND = 10009,
        PASSWORD_INCORRECT = 10010,
        PASSWORD_IS_EMPTY = 10011,
        PASSWORD_NOT_CONTAINS_LOWERCASE = 10012,
        PASSWORD_NOT_CONTAINS_UPPERCASE = 10013,
        PASSWORD_LENGTH_INCORRECT = 10014,
        PASSWORD_NOT_CONTAINS_NUMERIC = 10015,
        EMAIL_IS_EMPTY = 10016,
        EMAIL_INVALID = 10017,

        RESOURCE_OWNER_NOT_FOUND = 20001,
        ACCESSING_USER_NOT_FOUND = 20002,
        TOKEN_NOT_FOUND = 20003,
        TOKEN_INVALID = 20004,

        LODGING_TYPE_NOT_FOUND = 30001,
        LODGING_NOT_FOUND = 30002,
        COUNTRY_NOT_FOUND = 30003,
        INVALID_RESERVATION_WINDOW_DATES = 30004,
        INVALID_ROOM_CAPACITY = 30005,
        LODGING_ADDRESS_NOT_FOUND = 30006,
        RESERVATION_WINDOW_NOT_FOUND = 30007,
        ROOM_NOT_FOUND = 30008,
        CURRENCY_NOT_FOUND = 30009,
        ADDRESS_NOT_UNIQUE = 30010,
        APPROVING_DATA_NOT_VALID = 30011
    }
}
