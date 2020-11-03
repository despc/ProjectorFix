using NLog;
using Torch;
using Torch.API;

namespace ProjectorFix
{
    public class ProjectorFixPlugin : TorchPluginBase
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public override void Init(ITorchBase torch)
        {
            Log.Info("Init ProjectorFixPlugin");
        }
    }
}