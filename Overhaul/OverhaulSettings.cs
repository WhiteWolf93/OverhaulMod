using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;

namespace OverhaulMod
{
    public class OverhaulSettingVars
    {
        //change when the global settings are updated to force a recreation of the global settings
        public const string ModVersion = "v1.0 BETA";
        //change when the global settings are updated to force a recreation of the global settings
        public const string GlobalSettingsVersion = "v1.0 BETA";
    }

    //Global (non-player specific) settings
    public class OverhaulModSettings : IModSettings
    {
        public void Reset()
        {
            BoolValues.Clear();
            StringValues.Clear();
            IntValues.Clear();
            FloatValues.Clear();
            MinimapAutoUpdate = true;
            SettingsVersion = "v1.0 BETA";
        }

        public string SettingsVersion
        {
            get { return GetString(null, "SettingsVersion"); }
            set { SetString(value, "SettingsVersion"); }
        }

        public bool MinimapAutoUpdate
        {
            get { return GetBool(true, "MinimapAutoUpdate"); }
            set { SetBool(value, "MinimapAutoUpdate"); }
        }
        /*
        public bool WaywardCompassChanges
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool HeavyBlowChanges
        {
            get => GetBool(true);
            set => SetBool(value);
        }
        */
    }

    //Player specific settings
    public class OverhaulModSaveSettings : IModSettings
    {
    }
}
