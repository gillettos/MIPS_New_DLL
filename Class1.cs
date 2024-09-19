using System;
using TwinCAT.Ads;

namespace MyLibrary
{
    public class PlcCommunicator
    {
        private AdsClient adsClient;


        //Is this now needed as all individual calls set the coms link up
        public PlcCommunicator()
        {
            adsClient = new AdsClient();
            adsClient.Connect("5.143.125.242.1.1", 851); // Replace with PLC's NET address and port
        }




        public int mc_ird(ushort Axis) //read axis position USING DISPLAY POSITION NOT ACTUAL which is correct???
        {
            try
            {
                string variableName = $"prgMotion_1.fAx{Axis}_DispPosition"; // Construct the variable name for the actual position of the specified axis
                uint variableHandle = adsClient.CreateVariableHandle(variableName); // Create a handle for the variable

                double actPos = adsClient.ReadAny<double>(variableHandle);
                adsClient.DeleteVariableHandle(variableHandle); // Delete the handle to free up resources

                // Return a success code or any relevant value
                return 0; // Assuming 0 indicates success
            }
            catch (System.Exception)
            {
                // Handle exceptions if necessary and return an error code
                return -1; // Assuming -1 indicates an error
            }
        }


        /// mc_info(char * szDesc,int &naxes,int &type);        // Get Information
        /// mc_init(char filename[], HWND);                     // initialise PMC


        public long mc_iSet(ushort Axis, double new_pos)  //Is this for Target Position OR Goto Position
        {
            try
            {
                string axisPosition = $"prgMotion_1.fAx{Axis}_GotoPosition";// Construct the variable name for the target velocity of the specified axis
                uint axisPosHandle = adsClient.CreateVariableHandle(axisPosition);// Create a handle for the variable

                adsClient.WriteAny(axisPosHandle, new_pos);// Write the new speed value to the variable handle
                adsClient.DeleteVariableHandle(axisPosHandle);// Delete the handle to free up resources

                // Return a success code or any relevant value
                return 0; // Assuming 0 indicates success
            }
            catch (System.Exception)
            {
                // Handle exceptions if necessary and return an error code
                return -1; // Assuming -1 indicates an error
            }

        }


        public long Mc_imov(ushort Axis, double new_pos) //Moves specified axis to new absolute position)  --  Same as Mc_istop???
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


        //mc_iMoveComplete - Currently runs of the busy var..... MAY NEED CHANGING!!!
        public long mc_imdn(int targetAxis) // wait for move complete (call after mc_imov) ??????
        {
            bool valueToRead;

            string target = $"Main.fbAxis{targetAxis}.bBusy";
            uint variableHandle = adsClient.CreateVariableHandle(target);
            valueToRead = adsClient.ReadAny<bool>(variableHandle);
            adsClient.DeleteVariableHandle(variableHandle);

            Console.WriteLine("The Drive is: " + valueToRead);

            return valueToRead ? 1 : 0; // Return 1 if true, 0 if false
        }


        //??? mc_ihkstart(void);                // start handheld keypad program 



        public long mc_ifast(ushort Axis, double new_speed)  //(Sets a new speed)
        {
            try
            {
                string axisSpeed = $"prgMotion_1.fAx{Axis}_TargetVel";// Construct the variable name for the target velocity of the specified axis
                uint axisSpeedHandle = adsClient.CreateVariableHandle(axisSpeed);// Create a handle for the variable

                adsClient.WriteAny(axisSpeedHandle, new_speed);// Write the new speed value to the variable handle
                adsClient.DeleteVariableHandle(axisSpeedHandle);// Delete the handle to free up resources

                // Return a success code or any relevant value
                return 0; // Assuming 0 indicates success
            }
            catch (System.Exception)
            {
                // Handle exceptions if necessary and return an error code
                return -1; // Assuming -1 indicates an error
            }
 
        }

        //????   mc_imrs(void);                // reset controller (class destructor does same)


        public long Mc_istop(ushort Axis, double new_pos)  //(Stop all Movement but leave enabled)
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



   
        public int mc_motors_on() //Enable all Axis
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



        public long mc_motors_off(ushort Axis, double new_pos) //Disable all Axis
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



        // ????? mc_axis_numbers(USHORT &pri, USHORT &sec);    // return pri & sec drive numbers

        // ????? mc_read_conv_factors(float &pri_conv, float &sec_conv);

        // ????? mc_set_conv_factors(float &pri_conv, float &sec_conv);

        // ????? long mc_getstatus(int* iLimit);    // Get Status Information








        /*
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

       */





        public void ToggleBoolean(string variableName)
        {

            {
                int variableHandle = (int)adsClient.CreateVariableHandle(variableName);
                bool currentValue = adsClient.ReadAny<bool>((uint)variableHandle);
                adsClient.WriteAny((uint)variableHandle, !currentValue);
                adsClient.DeleteVariableHandle((uint)variableHandle);
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






        ~PlcCommunicator()

        {
            adsClient.Dispose();
        }
    }

}
