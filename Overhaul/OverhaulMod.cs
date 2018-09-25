using ModCommon;
using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OverhaulMod
{
    public class OverhaulMod : Mod<OverhaulModSaveSettings, OverhaulModSettings>, ITogglableMod
    {

        public static OverhaulMod instance;
        protected string SettingsFilename = Application.persistentDataPath + ModHooks.PathSeperator.ToString() + "OverhaulMod.settings.json";
        public Minimap minimap;


        public override void Initialize()
        {
            instance = this;

            SetupSettings();
            RegisterCallbacks();
        }

        private void SetupSettings()
        {
            bool flag = base.GlobalSettings.SettingsVersion != "v1.0b BETA";
            if (flag || !File.Exists(SettingsFilename))
            {
                if (flag)
                {
                    Log("Settings outdated! Rebuilding.");
                }
                else
                {
                    Log("Settings not found, rebuilding... File will be saved to: " + SettingsFilename);
                }

                base.GlobalSettings.Reset();
            }
            SaveGlobalSettings();
        }

        private void RegisterCallbacks()
        {
            Dev.Where();

            // AutoMinimap
            if (GlobalSettings.MinimapAutoUpdate)
            {
                ModHooks.Instance.CharmUpdateHook += CharmUpdate;
                ModHooks.Instance.HeroUpdateHook += HeroUpdate;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += UpdateMinimap;
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += UpdateMinimap;
            }
        }

        public void CharmUpdate(PlayerData pd, HeroController hc)
        {
            if (minimap != null)
            {
                if (pd.equippedCharm_2)
                {
                    GameMap map = GameManager.instance.gameMap.GetComponent<GameMap>();
                    map.SetupMap();
                    minimap.Show();
                    
                    minimap.UpdateAreas();
                }
                else
                {
                    minimap.Hide();
                }
            }
        }

        private void UnregisterCallbacks()
        {
            minimap.Unload();
            minimap = null;
            ModHooks.Instance.HeroUpdateHook -= HeroUpdate;
            ModHooks.Instance.CharmUpdateHook -= CharmUpdate;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= UpdateMinimap;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= UpdateMinimap;
        }

        public void SaveGame(SaveGameData data)
        {
            
        }

        private void UpdateMinimap(Scene from, Scene to)
        {
            if (HeroController.instance == null)
            {
                minimap.Unload();
                minimap = null;
            }

            GameManager.instance.StartCoroutine(UpdateMap());
        }

        private void UpdateMinimap(Scene from, LoadSceneMode lsm)
        {
            GameManager.instance.StartCoroutine(UpdateMap());
        }

        private IEnumerator UpdateMap()
        {
            yield return new WaitForSeconds(0.2f);
            if (minimap != null)
            {
                UpdateMinimap();
            }
            
        }

        private void UpdateMinimap()
        {
            if (HeroController.instance == null)
            {
                if (minimap != null)
                {
                    minimap.Unload();
                    minimap = null;
                }
            }

            if (minimap != null)
            {
                if (HeroController.instance.playerData.equippedCharm_2)
                {
                    minimap.Show();
                }
                else
                {
                    minimap.Hide();
                }
                GameMap map = GameManager.instance.gameMap.GetComponent<GameMap>();
                map.SetupMap();
                minimap.UpdateAreas();
            }
        }
        public void HeroUpdate()
        {
            bool equippedCompass = PlayerData.instance.equippedCharm_2;
            if (equippedCompass)
            {
                GameManager.instance.UpdateGameMap();
                if (GameManager.instance.gameMap != null)
                {
                    
                    if (minimap == null)
                    {
                        GameMap map = GameManager.instance.gameMap.GetComponent<GameMap>();
                        map.SetupMap();
                        minimap = new Minimap(map);
                        minimap.UpdateMap();
                        minimap.UpdateAreas();
                    }
                    else
                    {
                        minimap.UpdateMap();
                    }
                }
                
            }
        }

        public override string GetVersion()
        {
            return GlobalSettings.SettingsVersion;
        }

        public void Unload()
        {
            UnregisterCallbacks();
            instance = null;
        }
    }
}
