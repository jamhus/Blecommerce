namespace Blecommerce.Client.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> AddOrUpdate(Address address);
        Task<Address> GetAddress();
    }
}
