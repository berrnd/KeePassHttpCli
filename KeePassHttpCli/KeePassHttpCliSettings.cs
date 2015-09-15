using KeePassHttpClient;

namespace KeePassHttpCli
{
    public class KeePassHttpCliSettings : SavableObject
    {
        public KeePassHttpConnectionInfo ConnectionInfo { get; set; }
    }
}
