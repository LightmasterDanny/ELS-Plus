﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using ELS.Sirens;
using Control = CitizenFX.Core.Control;
using CitizenFX.Core.Native;
using System;

namespace ELS
{
    public class ELS : BaseScript
    {
        private SirenManager _sirenManager;
        public EventHandlerDictionary EventHandlerDictionary => EventHandlers;
        private FileLoader _FileLoader;
        public ELS()
        {
            
            _FileLoader = new FileLoader(this);
            _sirenManager = new SirenManager();
            EventHandlers["onClientResourceStart"] += new Action<string>(_FileLoader.RunLoadeer);
            this.EventHandlers["ELS:SirenUpdated"] += new Action<int>(_sirenManager.UpdateSirens);

            Tick += Class1_Tick;
        }

        private async Task Class1_Tick()
        {

            if (LocalPlayer.WantedLevel > 0)
            {
                CitizenFX.Core.UI.Screen.ShowNotification("wanted level");
                LocalPlayer.WantedLevel = 0;
            }
            if (LocalPlayer.Character.IsGettingIntoAVehicle)
            {
                if (LocalPlayer.Character.VehicleTryingToEnter != null)
                {
                    _sirenManager.SetCurrentSiren(LocalPlayer.Character.VehicleTryingToEnter);
                }
            }
            if (LocalPlayer.Character.IsInVehicle())
            {
                Screen.ShowNotification(Function.Call<int>(Hash.VEH_TO_NET, LocalPlayer.Character.CurrentVehicle).ToString());
                _sirenManager.runtick();
            }
            //if (Game.IsControlJustReleased(0, Control.NextCamera))
            //{
            //    Game.DisableControlThisFrame(0, Control.NextCamera);
            //    sirenState.ToggleBlackOut();
            //}
        }
    }
}

