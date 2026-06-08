namespace Amazon.SharedKernel.Customers;

public record DeliveryAddress(int CountryId, int CityId, string PostalCode, string PhoneNumber, int BuildingNumber, int? ApartmentNumber);