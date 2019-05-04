using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CylMem = NoRecoil.Class_Memory;
using System.Threading;

namespace NoRecoil
{
    class Program
    {
        int ShotsFired = 0;
        Vector3 Angle;
        Vector3 AimPunch;
        Vector3 OldAngle;
        IntPtr ClientDll, EngineDll;
        IntPtr LocalPlayer, ClientState;
        public void RCSXA()
        {
            while (true)
            {
                IntPtr ClientDll, EngineDll;
                IntPtr LocalPlayer, ClientState;
                if (ClientDll == IntPtr.Zero) ClientDll = Memory.GetModuleBaseAddress("client.dll");
                if (EngineDll == IntPtr.Zero) EngineDll = Memory.GetModuleBaseAddress("engine.dll");
                if (LocalPlayer == IntPtr.Zero) LocalPlayer = Memory.Read<IntPtr>(ClientDll + Offsets.BasePlayer);
                if (ClientState == IntPtr.Zero) ClientState = Memory.Read<IntPtr>(EngineDll + Offsets.ClientState);

                if (Memory.GetAsyncKeyState(0x01)) ControlSpray();
                Thread.Sleep(10);
            }
        }
        static void Main(string[] args)
        {
            
            Memory.Initialize("csgo");
            while (true)
            {
                
            }
        }
        public static Vector3 ClampAngle(Vector3 Angle)
        {
            if (Angle[0] > 89.0f)
                Angle[0] = 89.0f;

            if (Angle[0] < -89.0f)
                Angle[0] = -89.0f;

            while (Angle[1] > 180)
                Angle[1] -= 360;

            while (Angle[1] < -180)
                Angle[1] += 360;

            Angle.Z = 0;

            return Angle;
        }
        public void ControlSpray()
        {
            ShotsFired = Memory.Read<int>(LocalPlayer + Offsets.m_shotsFired);
            if (ShotsFired > 1)
            {
                Angle = Memory.Read<Vector3>(LocalPlayer + Offsets.m_aimPunchAngle);
                AimPunch = OldAngle - Angle * 2f;
                ClampAngle(AimPunch);
                Memory.Write<Vector3>((IntPtr)ClientState + Offsets.m_viewPunchAngle, AimPunch);
            }
            else
            {
                OldAngle = Memory.Read<Vector3>(ClientState + Offsets.m_viewPunchAngle);
            }
        }
    }
}
