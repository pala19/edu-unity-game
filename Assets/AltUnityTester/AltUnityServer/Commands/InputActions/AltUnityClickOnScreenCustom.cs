
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnityClickOnScreenCustom : AltUnityCommand
    {
#if ALTUNITYTESTER

        private UnityEngine.Vector2 position;
        readonly string count;
        readonly string interval;
#endif

        public AltUnityClickOnScreenCustom(params string[] parameters) : base(parameters, 5)
        {
#if ALTUNITYTESTER
            this.position = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[2]);
            this.count = Parameters[3];
            this.interval = Parameters[4];
#endif
        }

        public override string Execute()
        {
#if ALTUNITYTESTER

            if (!int.TryParse(count, out int pCount)) { pCount = 1; }
            if (!float.TryParse(interval, out float pInterval)) { pInterval = 0f; }

            Input.SetCustomClick(position, pCount, pInterval);
            return "Ok";
#else
            return null;
#endif
        }
    }
}