namespace FWAPPA.Model;

public enum AirportAdditionError
{
    InvalidIdLength,
    InvalidCityLength,
    InvalidRating,
    InvalidDate,
    DuplicateAirportId,
    DBAdditionError,
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
    InvalidCityLength,
    InvalidRating,
    InvalidDate,
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