using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Core.Logging.Interpolation;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using Spectrals.DinkumBetterVanilla.Redux.Patches;


namespace Spectrals.DinkumBetterVanilla.Redux
{
    // Token: 0x02000002 RID: 2
    [BepInPlugin("spectrals.dinkumbettervanilla.redux", "Dinkum: Better Vanilla - Redux", "1.0.0")]
    [BepInProcess("Dinkum.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private void StoreModId()
        {
            ConfigEntry<int> configEntry = base.Config.Bind<int>("Developer", "NexusID", modId, "nexus mod id -- automatically generated -- cannot be changed");
            if (configEntry.Value != modId)
            {
                configEntry.Value = modId;
                base.Config.Save();
            }
        }

        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public Plugin()
        {
            this.IsModEnabled = base.Config.Bind<bool>("!Config", "IsModEnabled", true, "If this value is true the mod would be enable otherwise it would be disable.");
            this.IsFishStackable = base.Config.Bind<bool>("Inventory", "IsFishStackable", true, "If this value is true all fishes would be stackable (by example you could carry 999999 Marlin).");
            this.IsFishStorable = base.Config.Bind<bool>("Inventory", "IsFishStorable", false, "If this value is true all fishes could be put in container (chest, ...).");
            this.IsUnderwaterCreatureStackable = base.Config.Bind<bool>("Inventory", "IsUnderwaterCreatureStackable", true, "If this value is true all underwater creature would be stackable.");
            this.IsBugStackable = base.Config.Bind<bool>("Inventory", "IsBugStackable", true, "If this value is true all bugs would be stackable (by example you could carry 999999 Blue Moon Butterfly).");
            this.IsBugStorable = base.Config.Bind<bool>("Inventory", "IsBugStorable", false, "If this value is true all bugs could be put in container (chest, ...).");
            this.ShouldShowItemValue = base.Config.Bind<bool>("Inventory", "ShouldShowItemValue", true, "If this value is true it will display the value of items in inventory and containers.");
            this.ItemValueRequiredCommerceLicence = base.Config.Bind<bool>("Inventory", "ItemValueRequiredCommerceLicence", true, "If this value is true it will display the value of items only if the player have the commerce licence.");
            this.ShouldShowIfItemIsDonatedToMuseum = base.Config.Bind<bool>("Inventory", "ShouldShowIfItemIsDonatedToMuseum", true, "If this value is true it will display if fish, bugs and underwater creature in inventory and containers are donated to the Museum.");
            this.IsDonatedToMuseumRequiredSomeDonations = base.Config.Bind<bool>("Inventory", "IsDonatedToMuseumRequiredSomeDonations", true, "If this value is true it will display if the item is donated to the Museum only if the player have donated 20 fishes or 20 bugs.");
            this.IsMinimapZoomEnabled = base.Config.Bind<bool>("MiniMap", "IsMinimapZoomEnabled", true, "If this value is true, it will allow the minimap to be zoomable based on the below Config items.");
            this.MinimapZoomIn = base.Config.Bind<KeyCode>("MiniMap", "MinimapZoomIn", (KeyCode)275, "The Unity's key that will Zoom In the minimap.");
            this.MinimapZoomOut = base.Config.Bind<KeyCode>("MiniMap", "MinimapZoomOut", (KeyCode)276, "The Unity's key that will Zoom Out the minimap.");
            this.MinimapZoomStrength = base.Config.Bind<float>("MiniMap", "MinimapZoomStrength", 0.25f, "This value determine the strengthens of the Zoom.");
            this.MinimapZoomSave = base.Config.Bind<float>("MiniMap", "MinimapZoomSave", 1f, "This value save the minimap zoom. This value is changed in-game.");
            //this.IsRestingEnabled = base.Config.Bind<bool>("Resting", "IsRestingEnabled", true, "If this value is true then the player will regenerate health/stamina when he is sitting on a furniture (bed, bench, ...).");
            //this.HealthRestoreOnResting = base.Config.Bind<int>("Resting", "HealthRestoreOnResting", 1, "This value determine how much health will be regenerated when the player rest on a furniture (bed, bench, ...).");
            //this.StaminaRestoreOnResting = base.Config.Bind<float>("Resting", "StaminaRestoreOnResting", 0.25f, "This value determine how much stamina will be regenerated when the player rest on a furniture (bed, bench, ...).");
            //this.RestingTick = base.Config.Bind<float>("Resting", "RestingTick", 2f, "This value determine how many seconds are needed before the player regenerate some health/stamina.");
            this.autoPickUpKeyCode = base.Config.Bind<KeyCode>("AutoPickUp", "AutoPickUpKeyCode", (KeyCode)112, "The Unity's key that will enable/disable the auto pick-up.");
            this.IsAutoPickUpEnabled = base.Config.Bind<bool>("AutoPickUp", "IsAutoPickUpEnabled", true, "If this value is true all items dropped near the player would be automatically pick-up.");
            this.InGameTimeSpeed = base.Config.Bind<float>("Misc", "InGameTimeSpeed", 2f, "This value determines how many realtime seconds are needed before adding a minutes in-game.The game's default value is 2.0. Higher values means longer day and lower value means shorter day.");
            this.RelationshipLostOnRequestNotCompleted = base.Config.Bind<int>("Misc", "RelationshipLostOnRequestNotCompleted", 2, "This value determines how many relationship points are lost once a request has not been completed. The game's default value is 2, A NPC have at max 100 relationship points.");
            this.RelationshipGainCoefficient = base.Config.Bind<float>("Misc", "RelationshipGainCoefficient", 1f, "This value multiply the relationship points gained by NPC. If this value is set to 1.0f then nothing would be modified");
            this.HealthLostWhenNoMoreStamina = base.Config.Bind<int>("Misc", "HealthLostWhenNoMoreStamina", 25, "This value determines how many health the player will lost when he has no more stamina, It prevents from instant death when no more stamina. If it is set to 0 it will disable this feature");
            //TODO SOunds config
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000023D8 File Offset: 0x000005D8
        private void Awake()
        {
            Plugin.Instance = this;
            this.PluginLogger = base.Logger;
            bool value = this.IsModEnabled.Value;
            if (value)
            {
                this.harmony.PatchAll();
                ManualLogSource logger = base.Logger;
                bool flag;
                BepInExInfoLogInterpolatedStringHandler bepInExInfoLogInterpolatedStringHandler = new BepInExInfoLogInterpolatedStringHandler(29, 1, out flag);
                if (flag)
                {
                    bepInExInfoLogInterpolatedStringHandler.AppendLiteral("Plugin ");
                    bepInExInfoLogInterpolatedStringHandler.AppendFormatted<string>("Dinkum: Spectral's Better Vanilla - Redux");
                    bepInExInfoLogInterpolatedStringHandler.AppendLiteral(" is loaded and enabled");
                }
                logger.LogInfo(bepInExInfoLogInterpolatedStringHandler);
            }
            else
            {
                ManualLogSource logger2 = base.Logger;
                bool flag;
                BepInExInfoLogInterpolatedStringHandler bepInExInfoLogInterpolatedStringHandler = new BepInExInfoLogInterpolatedStringHandler(30, 1, out flag);
                if (flag)
                {
                    bepInExInfoLogInterpolatedStringHandler.AppendLiteral("Plugin ");
                    bepInExInfoLogInterpolatedStringHandler.AppendFormatted<string>("Dinkum: Spectral's Better Vanilla - Redux");
                    bepInExInfoLogInterpolatedStringHandler.AppendLiteral(" is loaded but disabled");
                }
                logger2.LogInfo(bepInExInfoLogInterpolatedStringHandler);
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x0000249D File Offset: 0x0000069D
        private void Start()
        {
            this.StoreModId();
            this.miniMapZoom = this.MinimapZoomSave.Value;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000024B1 File Offset: 0x000006B1
        private void OnDestroy()
        {
            this.MinimapZoomSave.Value = this.miniMapZoom;
            base.Config.Save();
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000024D4 File Offset: 0x000006D4
        private void Update()
        {
            if (IsMinimapZoomEnabled.Value)
            {
                bool keyDown = Input.GetKeyDown(this.autoPickUpKeyCode.Value);
                if (keyDown)
                {

                    CharPickUpPatches.OnAutoPickUpKeyCodePressed();
                }
                bool flag = this.MinimapZoomOut.Value > 0;
                if (flag)
                {
                    bool key = Input.GetKey(this.MinimapZoomIn.Value);
                    if (key)
                    {
                        this.miniMapZoom = Mathf.Clamp(this.miniMapZoom + this.MinimapZoomStrength.Value, 0.75f, 25f);
                    }
                    bool key2 = Input.GetKey(this.MinimapZoomOut.Value);
                    if (key2)
                    {
                        this.miniMapZoom = Mathf.Clamp(this.miniMapZoom - this.MinimapZoomStrength.Value, 0.75f, 25f);
                    }
                    // FIX#XXX: Updated Ref to using Instance (Lines 117, 120) - VaterOtter 12/30/2023
                    bool flag2 = !RenderMap.Instance.mapOpen;
                    if (flag2)
                    {
                        RenderMap.Instance.scale = this.miniMapZoom;
                    }
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public readonly Harmony harmony = new Harmony("spectrals.dinkumbettervanilla.redux");

        // Token: 0x04000002 RID: 2
        public static Plugin Instance;

        // Token: 0x04000003 RID: 3
        public ManualLogSource PluginLogger;

        // Token: 0x04000004 RID: 4
        public readonly int[] TrapIds = new int[]
        {
            302,
            697
        };

        // Token: 0x04000005 RID: 5
        public float miniMapZoom = 1f;

        // Token: 0x04000006 RID: 6
        public readonly ConfigEntry<bool> IsModEnabled;

        // Token: 0x04000007 RID: 7
        public readonly ConfigEntry<bool> IsFishStackable;

        // Token: 0x04000008 RID: 8
        public readonly ConfigEntry<bool> IsFishStorable;

        // Token: 0x04000009 RID: 9
        public readonly ConfigEntry<bool> IsUnderwaterCreatureStackable;

        // Token: 0x0400000A RID: 10
        public readonly ConfigEntry<bool> IsBugStackable;

        // Token: 0x0400000B RID: 11
        public readonly ConfigEntry<bool> IsBugStorable;

        // Token: 0x0400000C RID: 12
        public readonly ConfigEntry<bool> ShouldShowItemValue;

        // Token: 0x0400000D RID: 13
        public readonly ConfigEntry<bool> ItemValueRequiredCommerceLicence;

        // Token: 0x0400000E RID: 14
        public readonly ConfigEntry<bool> ShouldShowIfItemIsDonatedToMuseum;

        // Token: 0x0400000F RID: 15
        public readonly ConfigEntry<bool> IsDonatedToMuseumRequiredSomeDonations;

        public readonly ConfigEntry<bool> IsMinimapZoomEnabled;

        // Token: 0x04000010 RID: 16
        public readonly ConfigEntry<KeyCode> MinimapZoomIn;

        // Token: 0x04000011 RID: 17
        public readonly ConfigEntry<KeyCode> MinimapZoomOut;

        // Token: 0x04000012 RID: 18
        public readonly ConfigEntry<float> MinimapZoomStrength;

        // Token: 0x04000013 RID: 19
        public readonly ConfigEntry<float> MinimapZoomSave;

        //// Token: 0x04000014 RID: 20
        //public readonly ConfigEntry<bool> IsRestingEnabled;

        //// Token: 0x04000015 RID: 21
        //public readonly ConfigEntry<int> HealthRestoreOnResting;

        //// Token: 0x04000016 RID: 22
        //public readonly ConfigEntry<float> StaminaRestoreOnResting;

        //// Token: 0x04000017 RID: 23
        //public readonly ConfigEntry<float> RestingTick;

        // Token: 0x04000018 RID: 24
        private readonly ConfigEntry<KeyCode> autoPickUpKeyCode;

        // Token: 0x04000019 RID: 25
        public readonly ConfigEntry<bool> IsAutoPickUpEnabled;

        // Token: 0x0400001A RID: 26
        public readonly ConfigEntry<float> InGameTimeSpeed;

        // Token: 0x0400001B RID: 27
        public readonly ConfigEntry<int> RelationshipLostOnRequestNotCompleted;

        // Token: 0x0400001C RID: 28
        public readonly ConfigEntry<float> RelationshipGainCoefficient;

        // Token: 0x0400001D RID: 29
        public readonly ConfigEntry<int> HealthLostWhenNoMoreStamina;

        public readonly int modId = 306;
    }
}
