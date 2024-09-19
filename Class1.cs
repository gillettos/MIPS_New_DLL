using TwinCAT.Ads;

namespace MyLibrary
{
    public class PlcCommunicator
    {
        private AdsClient adsClient;

        /*
         * 
        //Is this now needed as all individual calls set the coms link up
        public PlcCommunicator()
        {
            adsClient = new AdsClient();
            adsClient.Connect("5.143.125.242.1.1", 851); // Replace with PLC's NET address and port
        }

        */

        // mc_motors_on (Enable all Axis)
        public int mc_motors_on()
        {

            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {
                client.Connect(remoteAmsNetId, remotePort);
                client.WriteValue("Main.fbAxis1.bEnable", true);  //
                //client.WriteValue("Main.fbAxis2.bEnable", true);  //
            }
            return 0;
        }



        //mc_motors_off (Disable all Axis)
        public long Mc_motors_off(ushort Axis, double new_pos)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {
                client.Connect(remoteAmsNetId, remotePort);
                client.WriteValue("Main.fbAxis1.bEnable", false);  //
                //client.WriteValue("Main.fbAxis2.bEnable", false);  //
            }
            return 0;
        }


        //mc_ifast (Sets a new speed)
        public long Mc_ifast(ushort Axis, double new_speed)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {

                string axisSpeed = $"prgMotion_1.fAx{Axis}_TargetVel";
                

                uint axisSpeedHandle = adsClient.CreateVariableHandle(axisSpeed);

                adsClient.WriteAny(axisSpeedHandle, new_speed);


                adsClient.DeleteVariableHandle(axisSpeedHandle);

            }

        }

/*

        //Mc_iMove (Moves specified axis to new absolute position)  --  Same as Mc_istop???
        public long Mc_imov(ushort Axis, double new_pos)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {
                string axisPosition = $"prgMotion_1.fAx{Axis}_GotoPosition";
                string axisMove = $"prgMotion_1.bAx{Axis}_ExMovRel";

                uint axisPositionHandle = adsClient.CreateVariableHandle(axisPosition);
                uint axisMoveHandle = adsClient.CreateVariableHandle(axisMove);

                adsClient.WriteAny(axisPositionHandle, new_pos);
                adsClient.WriteAny(axisMoveHandle, true);
                adsClient.WriteAny(axisMoveHandle, false);

                adsClient.DeleteVariableHandle(axisPositionHandle);
                adsClient.DeleteVariableHandle(axisMoveHandle);

                return 0;
            }
        }

        //mc_istop (Stop all Movement but leave enabled)
        public long Mc_istop(ushort Axis, double new_pos)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {
                string axisPosition = $"prgMotion_1.fAx{Axis}_GotoPosition";
                string axisMove = $"prgMotion_1.bAx{Axis}_ExMovRel";

                uint axisPositionHandle = adsClient.CreateVariableHandle(axisPosition);
                uint axisMoveHandle = adsClient.CreateVariableHandle(axisMove);

                adsClient.WriteAny(axisPositionHandle, new_pos);
                adsClient.WriteAny(axisMoveHandle, true);
                adsClient.WriteAny(axisMoveHandle, false);

                adsClient.DeleteVariableHandle(axisPositionHandle);
                adsClient.DeleteVariableHandle(axisMoveHandle);

                return 0;
            }
        }


        //mc_iSet (Set an Axis Position)
        public long Mc_iSet(ushort Axis, double new_pos)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {

            }

        }

        //mc_iMoveComplete
        public long Mc_iSet(ushort Axis, double new_pos)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {

            }

        }








        //Read an INT varible back
        public int ReadInteger(string variableName)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())
            {
                int variableHandle = (int)adsClient.CreateVariableHandle(variableName);
                int value = adsClient.ReadAny<int>((uint)variableHandle);
                adsClient.DeleteVariableHandle((uint)variableHandle);
                return value;
            }

        }





        public void ToggleBoolean(string variableName)
        {
            AmsNetId remoteAmsNetId = new AmsNetId("5.143.125.242.1.1"); //Dev Box
            //AmsNetId remoteAmsNetId = new AmsNetId("local");
            int remotePort = 851;

            using (AdsClient client = new AdsClient())

            {
                int variableHandle = (int)adsClient.CreateVariableHandle(variableName);
                bool currentValue = adsClient.ReadAny<bool>((uint)variableHandle);
                adsClient.WriteAny((uint)variableHandle, !currentValue);
                adsClient.DeleteVariableHandle((uint)variableHandle);
            }

        }
*/
        ~PlcCommunicator()

        {
            adsClient.Dispose();
        }
    }

}
