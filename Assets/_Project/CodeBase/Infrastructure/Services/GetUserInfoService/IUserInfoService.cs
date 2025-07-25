using UnityEngine;

namespace CodeBase.Infrastructure.Services.GetUserInfoService
{
    public interface IUserInfoService : IService
    {
        public DeviceType GetDeviceType();
        public string GetLanguage();
        string GetUserID();
    }
}