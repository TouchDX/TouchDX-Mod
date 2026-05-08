using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MelonLoader;
using HarmonyLib;
using Manager;

[assembly: MelonInfo(typeof(MaiRemoteTouchMod.MaiRemoteTouchMod), "TouchDX", "1.0.1", "Yubai")]

namespace MaiRemoteTouchMod
{
    public class MaiRemoteTouchMod : MelonMod
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern byte MapVirtualKey(uint uCode, uint uMapType);

        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_SCANCODE = 0x0008;

        private TcpListener _server;
        private Thread _serverThread;
        private bool _isRunning = false;

        public static ulong CurrentState = 0;
        public static ulong PreviousState = 0;

        // Button mapping to Virtual Key Codes (W, E, D, C, X, Z, A, Q)
        private static readonly byte[] ButtonVKs = new byte[] {
            0x57, 0x45, 0x44, 0x43, 0x58, 0x5A, 0x41, 0x51
        };

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Starting MaiRemoteTouch TCP Server on port 4321...");
            _isRunning = true;
            _serverThread = new Thread(ServerLoop);
            _serverThread.IsBackground = true;
            _serverThread.Start();

            var harmony = new HarmonyLib.Harmony("com.roo.mairemotetouch");
            harmony.PatchAll(typeof(InputManagerPatches));
            MelonLogger.Msg("Harmony Patches applied!");
        }

        private void SimulateKey(byte vk, bool isDown)
        {
            byte scanCode = MapVirtualKey(vk, 0); // MAPVK_VK_TO_VSC
            if (isDown) keybd_event(vk, scanCode, KEYEVENTF_SCANCODE, 0); // Down
            else keybd_event(vk, scanCode, KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP, 0); // Up
        }

        public override void OnUpdate()
        {
            if (CurrentState != PreviousState)
            {
                bool isInGame = false;
                try {
                    isInGame = Manager.GameManager.IsInGame;
                } catch { }

                // Diff physical buttons 0-7
                for (int i = 0; i < 8; i++)
                {
                    bool cur = (CurrentState & (1UL << i)) != 0;
                    bool prev = (PreviousState & (1UL << i)) != 0;
                    
                    // If not in game, also map A1-A8 (bits 8-15) to button presses
                    if (!isInGame)
                    {
                        bool curA = (CurrentState & (1UL << (i + 8))) != 0;
                        bool prevA = (PreviousState & (1UL << (i + 8))) != 0;
                        cur = cur || curA;
                        prev = prev || prevA;
                    }
                    
                    if (cur != prev) SimulateKey(ButtonVKs[i], cur);
                }

                // Diff Select (bit 42) -> Key '3' (0x33)
                bool curSel = (CurrentState & (1UL << 42)) != 0;
                if (curSel != ((PreviousState & (1UL << 42)) != 0)) SimulateKey(0x33, curSel);

                // Diff Test (bit 43) -> Key 'F2' (0x71)
                bool curTest = (CurrentState & (1UL << 43)) != 0;
                if (curTest != ((PreviousState & (1UL << 43)) != 0)) SimulateKey(0x71, curTest);

                // Diff Service (bit 44) -> Key 'F3' (0x72)
                bool curSrv = (CurrentState & (1UL << 44)) != 0;
                if (curSrv != ((PreviousState & (1UL << 44)) != 0)) SimulateKey(0x72, curSrv);

                // Diff Coin (bit 45) -> Key '5' (0x35)
                bool curCoin = (CurrentState & (1UL << 45)) != 0;
                if (curCoin != ((PreviousState & (1UL << 45)) != 0)) SimulateKey(0x35, curCoin);
            }
            PreviousState = CurrentState;
        }

        private void ServerLoop()
        {
            try
            {
                _server = new TcpListener(IPAddress.Any, 4321);
                _server.Start();

                while (_isRunning)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    client.NoDelay = true;
                    MelonLogger.Msg("Android Client Connected!");

                    Thread clientThread = new Thread(HandleClient);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
            }
            catch (Exception e)
            {
                if (_isRunning)
                {
                    MelonLogger.Error(string.Format("Server error: {0}", e.Message));
                }
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            bool lastInGame = false;
            bool sentInitial = false;

            try
            {
                while (_isRunning && client.Connected)
                {
                    if (stream.DataAvailable)
                    {
                        ulong state = reader.ReadUInt64(); 
                        CurrentState = state;
                    }
                    else
                    {
                        bool isInGame = false;
                        try {
                            isInGame = Manager.GameManager.IsInGame;
                        } catch { }
                        
                        if (isInGame != lastInGame || !sentInitial)
                        {
                            lastInGame = isInGame;
                            sentInitial = true;
                            byte[] packet = new byte[8] { 0x47, 0x41, 0x4D, 0x45, 0, 0, 0, (byte)(isInGame ? 1 : 0) };
                            stream.Write(packet, 0, 8);
                        }
                        Thread.Sleep(1);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                MelonLogger.Msg("Android Client Disconnected.");
                client.Close();
                CurrentState = 0; 
            }
        }

        public override void OnApplicationQuit()
        {
            _isRunning = false;
            if (_server != null) _server.Stop();
        }
    }

    public static class InputManagerPatches
    {
        private static bool IsBitSet(ulong state, int bitIndex)
        {
            if (bitIndex < 0 || bitIndex > 63) return false;
            return (state & (1UL << bitIndex)) != 0;
        }

        // ================= TOUCH PANEL =================
        // Touch is handled through Unity MeshButton in DummyTouchPanel mode.
        [HarmonyPatch(typeof(MeshButton), "Update", new System.Type[0])]
        [HarmonyPostfix]
        public static void MeshButtonUpdatePostfix(MeshButton __instance, ref InputManager.TouchPanelArea ___touchArea, ref Action<InputManager.TouchPanelArea> ___onTouchEvent, ref Action<InputManager.TouchPanelArea> ___onTouchDownEvent)
        {
            bool isInGame = false;
            try {
                isInGame = Manager.GameManager.IsInGame;
            } catch { }

            int bitIndex = (int)___touchArea + 8; 
            
            // Only trigger touch panel events if we are in game OR if it's not an A-area (A-areas are mapped to buttons out of game)
            bool isAArea = bitIndex >= 8 && bitIndex <= 15;
            bool shouldProcessTouch = isInGame || !isAArea;
            
            if (shouldProcessTouch && IsBitSet(MaiRemoteTouchMod.CurrentState, bitIndex))
            {
                if (___onTouchEvent != null) ___onTouchEvent.Invoke(___touchArea);
                
                if (!IsBitSet(MaiRemoteTouchMod.PreviousState, bitIndex))
                {
                    if (___onTouchDownEvent != null) ___onTouchDownEvent.Invoke(___touchArea);
                }
            }
        }
    }
}
