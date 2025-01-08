namespace FWAPPA.Model;

public enum AirportAdditionError
{
    InvalidIdLength,
    InvalidCityLength,
    InvalidRating,
    InvalidDate,
    DuplicateAirportId,
    DBAdditionError,
    NoDateSelectedError,
    NoError
}

public enum AirportDeletionError
{
    AirportNotFound,
    DBDeletionError,
    NoError
}

public enum AirportEditError
{
    AirportNotFound,
    InvalidFieldError,
    DBEditError,
    NoError
}

public enum FlyWisconsinLevel
{
    Bronze,
    Silver,
    Gold,
    None
}