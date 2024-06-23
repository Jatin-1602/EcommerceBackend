using EcommerceMariaDB.Models;

namespace EcommerceMariaDB.Interfaces
{
    public interface ITrackOrderRepo
    {
        bool CreateTrack(List<int> orderItemID);
        bool UpdateStatus(int orderItemID, string status);
        TrackOrder? GetStatus(int orderItemID);
    }
}
